using System;

[Serializable]
public class Hero
{
    public int Uid;
    public string Name;
    public string Description;
    public int Speed;

    public int HealthPoint;
    public int MagicPoint;
    public int AttackPower;
    public int DefencePower;
    public int TotalExp;

    // Strenght (HealthPoint, AttackPower, DefencePower)
    // Intelligence (MagicPoint, AttackPower)
    // TotalExp

    // Properties <- Encapsulation (Teori)
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
            HealthPoint += (baseStr + bonus.Strength) * 350;
            AttackPower += (baseStr + bonus.Strength) * 10;
            DefencePower += (baseStr + bonus.Strength) * 7;
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
            MagicPoint += (baseInt + bonus.Intelligence) * 30;
            AttackPower += (baseInt + bonus.Intelligence) * 10;
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
}