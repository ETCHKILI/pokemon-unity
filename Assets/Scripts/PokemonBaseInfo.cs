using System;
public class PokemonBaseInfo {
    public struct stage {
        int evoluteLevel;
        string evoluteId;
    }
    public string pokemonId;
    public string name;
    public int level;
    public bool canEvolute = true;
    public stage[] stages;
    public int baseHP;
    public int baseATK;
    public int baseDEF;
    public int baseSP;
    public int HPIncrement;
    public int ATKIncrement;
    public int DEFIncrement;
    public int SPIncrement;
    public Skill[] skills;

    public void upgrade () {
        this.level++;
    }
}