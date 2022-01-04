using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    PSWITCH,
    ESWITCH,
    PWIN,
    EWIN
}


public class BattleSystem : MonoBehaviour
{
    public GameObject dragonPF;
    public GameObject flowerPF;
    public GameObject turtlePF;
    public GameObject boboPF;
    public GameObject grimerPF;
    public GameObject pikachuPF;

    GameObject playerGO;
    GameObject enemyGO;

    public Transform pStation;
    public Transform eStation;

    Pokemon pPoke;
    Pokemon ePoke;

    public BattleHud pHud;
    public BattleHud eHud;

    public List<Button> pMovesBtns;
    public List<Button> eMovesBtns;

    public Text dialogueText;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        
        SetupBattle();
    }

    void SetupBattle()
    {
        playerGO = Instantiate(dragonPF, pStation);
        pPoke = playerGO.GetComponent<Pokemon>();

        enemyGO = Instantiate(grimerPF, eStation);
        ePoke = enemyGO.GetComponent<Pokemon>();

        _Log(pPoke.pokeName+ " vs " + ePoke.pokeName);
        pHud.SetHud(pPoke);
        eHud.SetHud(ePoke);
        
        
        SetMoveBtns();
        foreach (var b in eMovesBtns)
        {   
            b.gameObject.SetActive(false);
        }
        state = BattleState.PLAYERTURN;
        PlayerTakeTurn();
    }

    void PlayerTakeTurn()
    {
        if (state == BattleState.PLAYERTURN)
        {
            Log("It's the player's turn on the left");
        }
        else
        {
            Log("It's the player's turn on the right");
        }
    }

    public void OnMoveBtn(Button b)
    {
        if (state == BattleState.PLAYERTURN)
        {
            if (!pMovesBtns.Contains(b))
            {
                return;
            }

            int dmg = GetDamageFromBtn(pPoke, b);
            ApplyAttack(pPoke, ePoke, dmg, b.GetComponentInChildren<Text>().text);
        }
        else if (state == BattleState.ENEMYTURN)
        {
            if (!eMovesBtns.Contains(b))
            {
                return;
            }

            int dmg = GetDamageFromBtn(ePoke, b);
            ApplyAttack(ePoke, pPoke, dmg, b.GetComponentInChildren<Text>().text);
        }
        else
        {
            return;
        }
    }

    void ApplyAttack(Pokemon p1, Pokemon p2, int damage, string move)
    {
        float allAtk = p1.attack + damage;
        float damageRate = allAtk / (allAtk + p2.defence);
        int realDamage = (int) (damageRate * allAtk);

        // float missRate = (float)p2.speed / (p1.speed + p2.speed);
        // int num = UnityEngine.Random.Range(0, 100);
        // if (num < missRate * 100) {
        //     Log("This move miss");
        //     realDamage = 0;
        // } else {
        //     Log("Damage " + realDamage.ToString());
        // }

        //Log("Move Damage " + realDamage.ToString());
        
        
        Log_dmg(realDamage.ToString(), move);
        
        
        if (state == BattleState.PLAYERTURN)
        {
            foreach (var b in pMovesBtns)
            {
                b.gameObject.SetActive(false);
            }
                
            foreach (var b in eMovesBtns)
            {
                b.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var b in eMovesBtns)
            {
                b.gameObject.SetActive(false);
            }
            foreach (var b in pMovesBtns)
            {
                b.gameObject.SetActive(true);

            }
        }

        if (p2.TakeDamage(realDamage))
        {
            if (p2.pokeName.Equals("FireDragon"))
            {
                state = BattleState.PLAYERTURN;
                Destroy(playerGO);

                playerGO = Instantiate(flowerPF, pStation);
                pPoke = playerGO.GetComponent<Pokemon>();
            }
            else if (p2.pokeName.Equals("Flower"))
            {
                state = BattleState.PLAYERTURN;
                Destroy(playerGO);

                playerGO = Instantiate(turtlePF, pStation);
                pPoke = playerGO.GetComponent<Pokemon>();
            }
            else if (p2.pokeName.Equals("Turtle"))
            {
                state = BattleState.EWIN;
                Log("Player on the right wins");
                StartCoroutine(GoBack());
            }
            else if (p2.pokeName.Equals("Grimer"))
            {
                state = BattleState.ENEMYTURN;
                Destroy(enemyGO);

                enemyGO = Instantiate(boboPF, eStation);
                ePoke = enemyGO.GetComponent<Pokemon>();
            }
            else if (p2.pokeName.Equals("BoBo"))
            {
                state = BattleState.ENEMYTURN;
                Destroy(enemyGO);

                enemyGO = Instantiate(pikachuPF, eStation);
                ePoke = enemyGO.GetComponent<Pokemon>();
            }
            else if (p2.pokeName.Equals("Pikachu"))
            {
                state = BattleState.PWIN;
                Log("Player on the right wins");
                StartCoroutine(GoBack());
            }
        }
        else
        {
            if (state == BattleState.PLAYERTURN)
            {
               
                state = BattleState.ENEMYTURN;
                
            }
            else
            {
               
                state = BattleState.PLAYERTURN;
               
            }
        }
        

        pHud.SetHud(pPoke);
        eHud.SetHud(ePoke);
        SetMoveBtns();
    }


    int GetDamageFromBtn(Pokemon p, Button b)
    {
        string n = b.GetComponentInChildren<Text>().text;
        List<Move> moveList = Move.moveDict[p.pokeName];
        foreach (Move m in moveList)
        {
            if (m.moveName.Equals(n))
            {
                return m.damage;
            }
        }

        return 0;
    }

    void SetMoveBtns()
    {
        for (int i = 0; i < 4; ++i)
        {
            pMovesBtns[i].GetComponentInChildren<Text>().text = Move.moveDict[pPoke.pokeName][i].moveName;
        }

        for (int i = 0; i < 4; ++i)
        {
            eMovesBtns[i].GetComponentInChildren<Text>().text = Move.moveDict[ePoke.pokeName][i].moveName;
        }
    }

    void Log(string s)
    {
        // dialogueText.text = pPoke.pokeName + "vs" + ePoke.pokeName;
        dialogueText.text = dialogueText.text + "\n" + s;
    }

    string PaintPlayer(string s)
    {
        return "<color=#14b437>" + s + "</color>";
    }

    string PaintEnemy(string s)
    {
        return "<color=#e35259>" + s + "</color>";
    }

    void Log_dmg(string dmg, string move)
    {
        if (state == BattleState.PLAYERTURN)
        {
            _Log(pPoke.pokeName + " use " + move + " deal " + dmg + " to the " + ePoke.pokeName);
        }
        else
        {
            _Log(ePoke.pokeName + " use " + move + " deal " + dmg + " to the " + pPoke.pokeName);
        }
    }

    void _Log(string s)
    {
        dialogueText.text = dialogueText.text + "\n" + s;
    }


    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}