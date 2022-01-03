using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    public string moveName;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Move(string moveName, int damage) {
        this.moveName = moveName;
        this.damage = damage;
    }

    public void Set(Move ots)
    {
        this.moveName = ots.moveName;
        this.damage = ots.damage;
    }

    public void ExecuteMove(Pokemon p1, Pokemon p2)
    {
        
    }
    
}
