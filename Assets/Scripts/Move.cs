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
        float allAtk = p1.attack + damage;
        float damageRate = allAtk / (allAtk + p2.defence);
        int realDamage = (int) (damageRate * allAtk);

        float missRate = (float)p2.speed / (2 * (p1.speed + p2.speed));
        int num = UnityEngine.Random.Range(0, 100);
        if (num < missRate * 100) {
            realDamage = 0;
        }

        p2.currentHP -= realDamage;
    }
    
}
