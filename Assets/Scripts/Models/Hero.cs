using System;
using UnityEngine.Assertions.Must;

[Serializable]
public class Hero
{
    public int Uid;
    public string Name;
    public string Description;
    public int Speed;

    public UnitPoint HealthPoint;
    public UnitPoint MagicPoint;
    public float AttackPower;
    public float DefencePower;
    public int StrBase;
    public int StrBonus;
    public int IntBase;
    public int IntBonus;
    public int TotalExp;

    // Strenght (HealthPoint, AttackPower, DefencePower)
    // Intelligence (MagicPoint, AttackPower)
    // TotalExp

    // Properties <- Encapsulation (Teori)

    [Serializable]
    public struct UnitPoint
    {
        public int Current;
        public int Maximum;

        public new readonly string ToString()
        {
            return $"{Current} / {Maximum}";
        }
    }

    public Hero()
    {
        Uid = 1;
        Name = "Nobel Knight";
        Description = "A holy knight form a far land";
        Speed = 5;
        Strength = (10, 0);
        Intelligence = (10, 0);
        TotalExp = 0;
    }

    public (int basicSTR, int bonusSTR) Strength 
    {  
        get
        {
            return (baseStr, bonus.Strength);
        }
        set
        {
            baseStr += value.basicSTR;
            bonus.Strength += value.bonusSTR;
            if (baseStr + bonus.Strength > 999)
            {
                bonus.Strength = 999 - baseStr;
            }
            else if (baseStr + bonus.Strength < 0)
            {
                bonus.Strength = 0;
            }
            HealthPoint.Maximum += (baseStr + bonus.Strength) * 350;
            HealthPoint.Current = HealthPoint.Maximum;
            AttackPower += (baseStr + bonus.Strength) * 1.5f;
            DefencePower += (baseStr + bonus.Strength) * 1.5f;
        }
    }

    public (int basicINT, int bonusINT) Intelligence 
    {
        get
        {
            return (baseInt, bonus.Intelligence);
        }
        set
        {
            baseInt += value.basicINT;
            bonus.Intelligence += value.bonusINT;
            if (baseInt + bonus.Intelligence > 999)
            {
                bonus.Intelligence = 999 - baseInt;
            }
            else if (baseInt + bonus.Intelligence < 0)
            {
                bonus.Intelligence = 0;
            }
            MagicPoint.Maximum += (baseInt + bonus.Intelligence) * 30;
            MagicPoint.Current = MagicPoint.Maximum;
            AttackPower += (baseInt + bonus.Intelligence) * 1.5f;
        }
    }

    private int baseStr;
    private int baseInt;
    private BonusStat bonus;

    public struct BonusStat
    {
        public int Strength;
        public int Intelligence;
    }

    public int Level(int exp, out int next)
    {
        // To prevent Exp gain less or equal 0
        if (exp <= 0)
        {
            exp = 1;
        }
        var level = (int)Math.Pow(exp, 0.25);
        next = (int)Math.Pow(level + 1, 4);
        return level;
    }

    public bool HasLevelUp(int before)
    {
        var after = (int)Math.Pow(TotalExp, 0.25);
        if (after > before)
        {
            var n = after - before;
            Strength = (n, 0);
            Intelligence = (n, 0);
            return true; // Jika naik level
        }
        return false; // Jika tidak naik level
    }

    public float Damage
    {
        set
        {
            var damage = value - DefencePower;
            HealthPoint.Current -= (int)damage;
            if (HealthPoint.Current < 0)
            {
                HealthPoint.Current = 0; // Mencegah terjadinya value minus
            }
            if (HealthPoint.Current > HealthPoint.Maximum)
            {
                HealthPoint.Current = HealthPoint.Maximum;
            }
        }
    }
}