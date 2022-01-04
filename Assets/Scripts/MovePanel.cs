using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Text = UnityEngine.UI.Text;

public class MovePanel : MonoBehaviour
{
    public GameObject panel;

    public void SetMoveButton(GameObject go)
    {
        var buttonList = new List<Button>(panel.GetComponentsInChildren<Button>());
        var moveList = new List<Move>(go.GetComponentsInChildren<Move>());
        for (int i = 0; i < Math.Min(moveList.Count, buttonList.Count); i++)
        {
            var m = buttonList[i].gameObject.AddComponent<Move>();
            m.Set(moveList[i]);
            Debug.Log(m);
            Debug.Log(m.moveName);
            buttonList[i].GetComponentInChildren<Text>().text = m.moveName;
            Debug.Log(buttonList[i].GetComponentInChildren<Text>().text);

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
