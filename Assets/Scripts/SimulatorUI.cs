using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimulatorUI : MonoBehaviour
{
    [Header("Button Values")]
    public int HealValue;
    public int DamageValue;

    public Slider BarHp;
    public TextMeshProUGUI TxtHp;
    public TextMeshProUGUI TxtAtk;
    public TextMeshProUGUI TxtDef;
    public TextMeshProUGUI TxtStr;
    public TextMeshProUGUI TxtInt;
    public TextMeshProUGUI TxtLevel;
    public TextMeshProUGUI TxtExp;

    [Header("Record Panel")]
    public GameObject PanelRecordObj;

    public Hero hero;

    private void Start()
    {
        hero = new Hero();
        RefreshStatus();
        UpdateCondition();
        Debug.Log(level);
    }

    public void ButExp_Handler()
    {
        Debug.Log(level); // Level sebelum
        hero.TotalExp += 1000;
        if (hero.HasLevelUp(level))
        {
            RefreshStatus(); // <<< Level naik
            UpdateCondition();
        }
    }

    public void BtnReset_Handler()
    {
        hero = new Hero();
        UpdateCondition();
        RefreshStatus();
    }

    public void BtnDamage_Handler()
    {
        hero.Damage = DamageValue;
        UpdateCondition();
    }

    public void BtnHeal_Handler()
    {
        hero.Damage = HealValue;
        UpdateCondition();
    }

    public void Btn_Save()
    {
        PanelRecordObj.SetActive(true);
    }

    private int level;

    private void UpdateCondition()
    {
        TxtHp.text = hero.HealthPoint.ToString();
        BarHp.value = hero.HealthPoint.Current;
    }

    public void RefreshStatus()
    {
        level = hero.Level(hero.TotalExp, out var next);
        TxtLevel.text = level.ToString();
        TxtExp.text = $"{hero.TotalExp}/{next}";

        TxtStr.text = (hero.Strength.basicSTR + hero.Strength.bonusSTR).ToString();
        TxtInt.text = (hero.Intelligence.basicINT + hero.Intelligence.bonusINT).ToString();
        TxtAtk.text = ((int)hero.AttackPower).ToString();
        TxtDef.text = ((int)hero.DefencePower).ToString();

        BarHp.maxValue = hero.HealthPoint.Maximum;
        BarHp.minValue = 0;
    }
}
