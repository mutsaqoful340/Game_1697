using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimulatorUI : MonoBehaviour
{
    public Slider BarHp;
    public TextMeshProUGUI TxtHp;
    public TextMeshProUGUI TxtAtk;
    public TextMeshProUGUI TxtDef;
    public TextMeshProUGUI TxtStr;
    public TextMeshProUGUI TxtInt;
    public TextMeshProUGUI TxtLevel;
    public TextMeshProUGUI TxtExp;

    public Hero hero;

    private void Start()
    {
        hero = new Hero();

        var level = hero.Level(hero.TotalExp, out var next);
        TxtLevel.text = level.ToString();
        TxtExp.text = $"{hero.TotalExp}/{next}";

        TxtStr.text = $"{hero.Strength.basicSTR} + { hero.Strength.bonusSTR}";
        TxtInt.text = $"{hero.Intelligence.basicINT} + {hero.Intelligence.bonusINT}";
        TxtAtk.text = hero.AttackPower.ToString();
        TxtDef.text = hero.DefencePower.ToString();
        TxtHp.text = hero.HealthPoint.ToString();
    }
}
