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
    public static List<GameObject> playerPokePack;
    public static List<GameObject> enemyPokePack;

    private int pIndex = 0;
    private int eIndex = 0;

    // public GameObject playerPF;
    // public GameObject enemyPF;

    public Transform pStation;
	public Transform eStation;

    public BattleHud pHud;
    public BattleHud eHud;

    // public List<Button> pMovesBtns;
    // public List<Button> eMovesBtns;
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
        if (state == BattleState.PSWITCH)
        {
            if (playerGO) { Destroy(playerGO); }
            playerGO = Instantiate(playerPokePack[pIndex], pStation);
            pPoke = playerGO.GetComponent<Pokemon>();
        }
        if (state == BattleState.ESWITCH)
        {
            if (enemyGO) { Destroy(enemyGO); }
            enemyGO = Instantiate(enemyPokePack[eIndex], eStation);
            ePoke = enemyGO.GetComponent<Pokemon>();
        }
        
        SetHuds();
        return true;
    }

    void SetHuds()
    {
        pHud.SetHud(pPoke);
        eHud.SetHud(ePoke);
        movePanel.SetMoveButton(playerGO);
    }
    
    
    void PlayerTakeTurn() {
        if (state == BattleState.PLAYERTURN) {
            Log("It's the player's turn on the left");
        } else {
            Log("It's the player's turn on the right");
        }
    }

    public void OnMoveBtn(Button b)
    {
        b.GetComponent<Move>().ExecuteMove(pPoke, ePoke);
        SetHuds();
        CheckState();
    }

    public void CheckState()
    {
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
        } else if (preState == BattleState.ENEMYTURN)
        {
            state = BattleState.PLAYERTURN;
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

    void Log(string s) {
        dialogueText.text = s;
    }
    
    IEnumerator GoBack() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }    
}
