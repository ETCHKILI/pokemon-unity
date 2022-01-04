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
    public static bool withAI;
    public static List<GameObject> playerPokePack;
    public static List<GameObject> enemyPokePack;

    private int pIndex = 0;
    private int eIndex = 0;

    public Transform pStation;
	public Transform eStation;

    public BattleHud pHud;
    public BattleHud eHud;
    
    public MovePanel movePanel;
    public Text dialogueText;
    
    private GameObject playerGO;
    private GameObject enemyGO;
    private Pokemon pPoke;
    private Pokemon ePoke;
    private BattleState state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle() { 
        // Set the index of two pack
        pIndex = 0;
        eIndex = 0;

        playerPokePack = new List<GameObject>()
        {
            Resources.Load<GameObject>("prefabs/BoBo"), 
            Resources.Load<GameObject>("prefabs/FireDragon"), 
            Resources.Load<GameObject>("prefabs/Flower")
        };
        enemyPokePack = new List<GameObject>()
        {
            Resources.Load<GameObject>("prefabs/Grimer"), 
            Resources.Load<GameObject>("prefabs/Pikachu"), 
            Resources.Load<GameObject>("prefabs/Turtle")
        };
        
        
        LoadNewPFAndSetComponents();
        state = BattleState.PLAYERTURN;
        PlayerTakeTurn();
    }
    
    // Load prefabs to game object and other components
    bool LoadNewPFAndSetComponents()
    {
        if (pIndex >= playerPokePack.Count)
        {
            return false;
        }

        if (eIndex >= enemyPokePack.Count)
        {
            return false;
        }
        
        // Set GameObject and Pokemon
        if (state == BattleState.PSWITCH || state == BattleState.START)
        {
            if (playerGO) { Destroy(playerGO); }
            playerGO = Instantiate(playerPokePack[pIndex], pStation);
            pPoke = playerGO.GetComponent<Pokemon>();
        }
        if (state == BattleState.ESWITCH || state == BattleState.START)
        {
            if (enemyGO) { Destroy(enemyGO); }
            enemyGO = Instantiate(enemyPokePack[eIndex], eStation);
            ePoke = enemyGO.GetComponent<Pokemon>();
        }
        
        SetHuds();
        SetMoves();
        return true;
    }

    void SetHuds()
    {
        pHud.SetHud(pPoke);
        eHud.SetHud(ePoke);
    }

    void SetMoves()
    {
        if (state == BattleState.PLAYERTURN || state == BattleState.START)
        {
            movePanel.SetMoveButton(playerGO);
        }
        else if (!withAI)
        {
            movePanel.SetMoveButton(enemyGO);
        }
    }

    void PlayerTakeTurn() {
        if (state == BattleState.PLAYERTURN) {
            Log("It's the player's turn on the left");
        } else {
            Log("It's the player's turn on the right");
            if (withAI == true)
            {
                var ms = enemyGO.GetComponentsInChildren<Move>();
                AIMove();
            }
        }
    }

    public void AIMove()
    {
        var ms = enemyGO.GetComponentsInChildren<Move>();
        int idx = UnityEngine.Random.Range(0, 4);
        int dmg = ms[idx].ExecuteMove(ePoke, pPoke);
        Log_dmg(dmg.ToString(), ms[idx].moveName);
        CheckState();
    }

    public void OnMoveBtn(Button b)
    {
        Move m = b.GetComponent<Move>();
        int dmg = 0;
        if (withAI == true)
        {
            if (state == BattleState.PLAYERTURN)
            {
                dmg = m.ExecuteMove(pPoke, ePoke);
            }
            if (state == BattleState.ENEMYTURN)
            {
                return;
            }
        }
        else
        {
            Debug.Log(state);
            if (state == BattleState.PLAYERTURN || state == BattleState.ENEMYTURN)
            {
                dmg = state == BattleState.PLAYERTURN ? m.ExecuteMove(pPoke, ePoke) : m.ExecuteMove(ePoke, pPoke);
            }
        }
        if (state == BattleState.PLAYERTURN || state == BattleState.ENEMYTURN)
        {
            Log_dmg(dmg.ToString(), m.moveName);
            CheckState();
        }
    }

    public void CheckState()
    {
        SetHuds();
        BattleState preState = state;
        
        if (pPoke.currentHP <= 0)
        {
            pIndex++;
            state = BattleState.PSWITCH;
            if (!LoadNewPFAndSetComponents())
            {
                state = BattleState.EWIN;
            }
            
        }
        if (ePoke.currentHP <= 0)
        {
            eIndex++;
            state = BattleState.ESWITCH;
            if (!LoadNewPFAndSetComponents())
            {
                state = BattleState.PWIN;
            }
        }

        if (state == BattleState.PWIN)
        {
            BattleOver();
        } else if (state == BattleState.EWIN)
        {
            BattleOver();
        } else if (preState == BattleState.PLAYERTURN)
        {
            state = BattleState.ENEMYTURN;
            PlayerTakeTurn();
        } else if (preState == BattleState.ENEMYTURN)
        {
            state = BattleState.PLAYERTURN;
            PlayerTakeTurn();
        }
    }

    void BattleOver()
    {
        if (state == BattleState.PWIN) 
        {
            Log("Left Win");
        } else if (state == BattleState.EWIN) 
        {
            Log("Right Win");   
        }
        StartCoroutine(GoBack());
    }

    IEnumerator GoBack() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
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
            Log(pPoke.pokeName + " use " + move + " deal " + dmg + " to the " + ePoke.pokeName);
        }
        else
        {
            Log(ePoke.pokeName + " use " + move + " deal " + dmg + " to the " + pPoke.pokeName);
        }
    }

    
}
