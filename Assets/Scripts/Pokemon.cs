using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokemonType {
    WATER,
    Fire,
    Ground,
    Grass
}

public class Move {
    public static Dictionary<string, List<Move>> moveDict = new Dictionary<string, List<Move>> { 
        {"FireDragon", new List<Move>{new Move("1.Split Fire", 2), new Move("2.Fire Spring",4), new Move("3.Fire Blast",8), new Move("4.Holy Fire",10)}},
        {"Flower", new List<Move>{new Move("1.", 2), new Move("Grass Grow",4), new Move("羞花",8), new Move("落英",10)}},
        {"Turtle", new List<Move>{new Move("强硬", 2), new Move("坚石",4), new Move("地裂",8), new Move("山崩",10)}},
        {"Grimer", new List<Move>{new Move("1.Bug", 2), new Move("2.Mucus",4), new Move("3.Mud",8), new Move("4.Stench",10)}},
        {"BoBo", new List<Move>{new Move("拍击", 2), new Move("展翅",4), new Move("厉啄",8), new Move("翔天",10)}},
        {"Pikachu", new List<Move>{new Move("雷击", 2), new Move("电涌",4), new Move("万伏",8), new Move("天雷",10)}}
    };

    public string moveName;
    public int damage;

    public Move(string moveName, int damage) {
        this.moveName = moveName;
        this.damage = damage;
    }

}

public class Pokemon : MonoBehaviour {
    public string pokeName;
    public PokemonType type;

    public int attack;
    public int defence;
    public int currentHP;
    public int maxHP;
    public int speed;

    public bool TakeDamage(int dmg) {
        currentHP -= dmg;
        if (currentHP <= 0) {
            return true;
        }
        return false;
    }

    public bool HasNext() {
        if (pokeName.Equals("Turtle") || pokeName.Equals("Pikachu")) {
            return false;
        } else {
            return true;
        }
    }

}



