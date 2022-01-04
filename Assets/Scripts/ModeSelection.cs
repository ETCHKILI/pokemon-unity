using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelection : MonoBehaviour
{
    

    public void PvpMode()
    {
        BattleSystem.withAI = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PveMode()
    {
        BattleSystem.withAI = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
