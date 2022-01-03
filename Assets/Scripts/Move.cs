using UnityEngine;
public class Move:MonoBehaviour{
    public enum Effect{
        //pure attack
        ATTACK,
        //recover self HP
        HEAL,
        //improve self properties
        STRENGTHEN,
        //reduce enemy's properties
        WEAKEN,
        //reduce enemy's properties and HP
        POISON
    }

    public string skillName;
    public string description;
    public int damage;
    public int maxPP;
    public int currentPP;
    //hitRate = 50 means 50% 
    public int hitRate;
    public Effect effect;
    public int unlockLevel;

    public Move(string skillName, string description, int damage, int maxPP, 
        int hitRate, Effect effect, int unlockLevel) {
        this.skillName = skillName;
        this.description = description;
        this.damage = damage;
        this.maxPP = maxPP;
        this.currentPP = this.maxPP;
        this.hitRate = hitRate;
        this.effect = effect;
        this.unlockLevel = unlockLevel;
    }

    public Move(){

    }
}
