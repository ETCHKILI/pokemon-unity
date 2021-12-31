using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    public Text nameText;
    public Text hpText;
    public Slider hpSlider;

    public void SetHud(Pokemon p) {
        nameText.text = p.pokeName;
        hpText.text = p.currentHP + "/" + p.maxHP;
        hpSlider.maxValue = p.maxHP;
        hpSlider.value = p.currentHP;
    }

    public void SetHP(int hp) {
        hpSlider.value = hp;
    }
}
