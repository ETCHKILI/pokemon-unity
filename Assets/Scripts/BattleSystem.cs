using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState {
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

    void SetupBattle() {
        playerGO = Instantiate(dragonPF, pStation);
		pPoke = playerGO.GetComponent<Pokemon>();

		enemyGO = Instantiate(grimerPF, eStation);
		ePoke = enemyGO.GetComponent<Pokemon>();

        Log(pPoke.pokeName + " vs " + ePoke.pokeName);
        pHud.SetHud(pPoke);
        eHud.SetHud(ePoke);

        SetMoveBtns();

        state = BattleState.PLAYERTURN;
        PlayerTakeTurn();
    }

    void PlayerTakeTurn() {
        if (state == BattleState.PLAYERTURN) {
            Log("It's the player's turn on the left");
        } else {
            Log("It's the player's turn on the right");
        }
        

    }

    public void OnMoveBtn(Button b) {
        if (state == BattleState.PLAYERTURN) {
            if (!pMovesBtns.Contains(b)) {
                return;
            }
            int dmg = GetDamageFromBtn(pPoke, b);
            ApplyAttack(pPoke, ePoke, dmg);
        } else if (state == BattleState.ENEMYTURN) {
            if (!eMovesBtns.Contains(b)) {
                return;
            }
            int dmg = GetDamageFromBtn(ePoke, b);
            ApplyAttack(ePoke, pPoke, dmg);
        } else {
            return;
        }
    }

    void ApplyAttack(Pokemon p1, Pokemon p2, int damage) {
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

        Log("Move Damage " + realDamage.ToString());

        
        if (p2.TakeDamage(realDamage)) {
            if (p2.pokeName.Equals("FireDragon")) {
                state = BattleState.PLAYERTURN;
                Destroy(playerGO);

                playerGO = Instantiate(flowerPF, pStation);
		        pPoke = playerGO.GetComponent<Pokemon>();
                
            } else if (p2.pokeName.Equals("Flower")) {
                state = BattleState.PLAYERTURN;
                Destroy(playerGO);

                playerGO = Instantiate(turtlePF, pStation);
		        pPoke = playerGO.GetComponent<Pokemon>();
            } else if (p2.pokeName.Equals("Turtle")) {
                state = BattleState.EWIN;
                Log("Player on the right wins");
                StartCoroutine( GoBack());
            } else if (p2.pokeName.Equals("Grimer")) {
                state = BattleState.ENEMYTURN;
                Destroy(enemyGO);

                enemyGO = Instantiate(boboPF, eStation);
		        ePoke = enemyGO.GetComponent<Pokemon>();
            }  else if (p2.pokeName.Equals("BoBo")) {
                state = BattleState.ENEMYTURN;
                Destroy(enemyGO);

                enemyGO = Instantiate(pikachuPF, eStation);
		        ePoke = enemyGO.GetComponent<Pokemon>();
            }  else if (p2.pokeName.Equals("Pikachu")) {
                state = BattleState.PWIN;
                Log("Player on the right wins");
                StartCoroutine( GoBack());
            }
        } else {
            if (state == BattleState.PLAYERTURN) {
                state = BattleState.ENEMYTURN;
            } else {
                state = BattleState.PLAYERTURN;
            }
        }

        pHud.SetHud(pPoke);
        eHud.SetHud(ePoke);
        SetMoveBtns();
    }

    int GetDamageFromBtn(Pokemon p, Button b) {
        string n = b.GetComponentInChildren<Text>().text;
        List<Move> moveList = Move.moveDict[p.pokeName];
        foreach(Move m in moveList) {
            if (m.moveName.Equals(n)) {
                return m.damage;
            }
        }
        return 0;
    }

    void SetMoveBtns() {
        for (int i = 0; i < 4; ++i) {
            pMovesBtns[i].GetComponentInChildren<Text>().text = Move.moveDict[pPoke.pokeName][i].moveName;
        }
        for (int i = 0; i < 4; ++i) {
            eMovesBtns[i].GetComponentInChildren<Text>().text = Move.moveDict[ePoke.pokeName][i].moveName;
        }
    }

    void Log(string s) {
        // dialogueText.text = pPoke.pokeName + "vs" + ePoke.pokeName;
        dialogueText.text = s;
    }


    IEnumerator GoBack() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }    
}
