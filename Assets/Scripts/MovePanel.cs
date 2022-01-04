using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class MovePanel : MonoBehaviour
{
    public GameObject panel;

    public void SetMoveButton(GameObject go)
    {
        var buttonList = new List<Button>(panel.GetComponentsInChildren<Button>());
        var moveList = new List<Move>(go.GetComponentsInChildren<Move>());
        for (int i = 0; i < Math.Min(moveList.Count, buttonList.Count); i++)
        {
            buttonList[i].gameObject.AddComponent<Move>();
            var m = buttonList[i].gameObject.GetComponent<Move>();
            m.Set(moveList[i]);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
