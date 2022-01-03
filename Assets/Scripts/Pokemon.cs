//using System.Collections;
//using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public enum PokemonType {
    WATER,
    FIRE,
    GROUND,
    GRASS,
    FLY,
    POISON,

    ELECTRICITY
}

public enum PokemonChracter {
    NAUGHTY,
    AGGRESIVE,
    TIMID,
    CLEVER
}

public class Pokemon : MonoBehaviour {
    public string pokeName;
    //may give name to a pokemon
    public string nickName;
    public PokemonType type;
    public int attack;
    public int defence;
    public int currentHP;
    public int maxHP;
    public int speed;
    public int specialAttack;
    public int specialDefence;
    public int level = 1;
    public int totalExp = 1;
    public static int SKILLCNT = 12;
    public static int INITSKILLCNT = 4;

    public static int TOTALSKILLCNT = 4;
    public int firstEvoLev;
    public int secondEvoLev;
    // For upgradibg from levl i-1 -> level i, it needs the totalExp >= expList[i]
    public static int[] expList = new int[101];
    public Move[] skillPool = new Move[SKILLCNT];
    public Move[] masterSkill = new Move[TOTALSKILLCNT];

    public PokemonChracter character;

    


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

    public void gainExp(int increaseExp){
        int tmpExp = this.totalExp + increaseExp;
        this.totalExp = tmpExp;
        while(tmpExp >= expList[this.level]){
            this.level++;
            switch(this.character){
                case PokemonChracter.NAUGHTY:
                    this.attack += 2;
                    this.defence += 2;
                    this.specialAttack += 1;
                    this.specialDefence += 1;
                    break;
                case PokemonChracter.AGGRESIVE:
                    this.attack += 2;
                    this.defence += 1;
                    this.specialAttack += 2;
                    this.specialDefence += 1;
                    break;
                case PokemonChracter.TIMID:
                    this.attack += 1;
                    this.defence += 2;
                    this.specialAttack += 1;
                    this.specialDefence += 2;
                    break;
                case PokemonChracter.CLEVER:
                    this.attack += 1;
                    this.defence += 1;
                    this.specialAttack += 2;
                    this.specialDefence += 2;
                    break;
            }
        }
    }

    //expList is generated from 1/2*x*x

    public void initSkillPool(string fileName){
        int i=0;
        using (System.IO.StreamReader file = System.IO.File.OpenText(fileName)){
            using (JsonTextReader reader = new JsonTextReader(file)){
                JArray ja = (JArray)JToken.ReadFrom(reader);
                
                for (; i < ja.Count; i++){
                    Move move = new Move();
                    JObject o = JObject.Parse(ja[i].ToString());
                    move.skillName = o["name"].ToString();
                    move.description = o["description"].ToString();
                    move.damage = (int)o["power"];
                    move.description = o["description"].ToString();
                    move.maxPP = (int)o["pp"];
                    move.currentPP = move.maxPP;
                    move.hitRate = (int)o["probability"];
                    move.effect = (Move.Effect)Enum.Parse(typeof(Move.Effect), o["effect"].ToString());
                    move.unlockLevel = (int)o["unlockLevel"];

                    this.skillPool[i] = move;
                }
                int j=0;
                for (; j < INITSKILLCNT; j++){
                    this.masterSkill[j] = this.skillPool[j];
                }
            }
        }
    }
    
    public Pokemon(){
        int i=0;
        for(;i<101;i++){
            expList[i] = (int)i*i/2+1;
        }
        System.Random rd = new System.Random();
        int j = rd.Next(0,3);
        switch(j){
            case 0:
                this.character = PokemonChracter.NAUGHTY;
                break;
            case 1:
                this.character = PokemonChracter.AGGRESIVE;
                break;
            case 2:
                this.character = PokemonChracter.TIMID;
                break;
            case 3:
                this.character = PokemonChracter.CLEVER;
                break;
        }
        int k = rd.Next(0,5);
        switch(k){
            case 0:
                this.initSkillPool("Assets//Scripts//skill//Turtle.json");
                break;
            case 1:
                this.initSkillPool("Assets//Scripts//skill//Flower.json");
                break;
            case 2:
                this.initSkillPool("Assets//Scripts//skill//PiKaChu.json");
                break;
            case 3:
                this.initSkillPool("Assets//Scripts//skill//Grimer.json");
                break;
            case 4:
                this.initSkillPool("Assets//Scripts//skill//BoBo.json");
                break;
            case 5:
                this.initSkillPool("Assets//Scripts//skill//FireDragon.json");
                break;
        }
        
    }
}





