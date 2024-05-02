using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageCalculator
{
    internal class DamageCalculation
    {
        Dictionary<string, Dictionary<string, double>> typeChart = new Dictionary<string, Dictionary<string, double>>
{
    // Normal type interactions
    { "Normal", new Dictionary<string, double>
        {
            { "Rock", 0.5 },
            { "Steel", 0.5 },
            { "Ghost", 0.0 }
        }
    },
    // Fire type interactions
    { "Fire", new Dictionary<string, double>
        {
            { "Grass", 2.0 },
            { "Ice", 2.0 },
            { "Bug", 2.0 },
            { "Steel", 2.0 },
            { "Fire", 0.5 },
            { "Water", 0.5 }
        }
    },
    // Water type interactions
    { "Water", new Dictionary<string, double>
        {
            { "Fire", 2.0 },
            { "Ground", 2.0 },
            { "Rock", 2.0 },
            { "Water", 0.5 },
            { "Grass", 0.5 },
            { "Dragon", 0.5 }
        }
    },
    // Electric type interactions
    { "Electric", new Dictionary<string, double>
        {
            { "Water", 2.0 },
            { "Flying", 2.0 },
            { "Electric", 0.5 },
            { "Grass", 0.5 },
            { "Dragon", 0.5 },
            { "Ground", 0.0 }
        }
    },
    // Grass type interactions
    { "Grass", new Dictionary<string, double>
        {
            { "Water", 2.0 },
            { "Ground", 2.0 },
            { "Rock", 2.0 },
            { "Fire", 0.5 },
            { "Grass", 0.5 },
            { "Poison", 0.5 },
            { "Flying", 0.5 },
            { "Bug", 0.5 },
            { "Dragon", 0.5 },
            { "Steel", 0.5 }
        }
    },
    // Ice type interactions
    { "Ice", new Dictionary<string, double>
        {
            { "Grass", 2.0 },
            { "Ground", 2.0 },
            { "Flying", 2.0 },
            { "Dragon", 2.0 },
            { "Fire", 0.5 },
            { "Water", 0.5 },
            { "Ice", 0.5 },
            { "Steel", 0.5 }
        }
    },
    // Fighting type interactions
    { "Fighting", new Dictionary<string, double>
        {
            { "Normal", 2.0 },
            { "Ice", 2.0 },
            { "Rock", 2.0 },
            { "Dark", 2.0 },
            { "Steel", 2.0 },
            { "Poison", 0.5 },
            { "Flying", 0.5 },
            { "Psychic", 0.5 },
            { "Bug", 0.5 },
            { "Fairy", 0.5 },
            { "Ghost", 0.0 }
        }
    },
    // Poison type interactions
    { "Poison", new Dictionary<string, double>
        {
            { "Grass", 2.0 },
            { "Fairy", 2.0 },
            { "Poison", 0.5 },
            { "Ground", 0.5 },
            { "Rock", 0.5 },
            { "Ghost", 0.5 },
            { "Steel", 0.0 }
        }
    },
    // Ground type interactions
    { "Ground", new Dictionary<string, double>
        {
            { "Fire", 2.0 },
            { "Electric", 2.0 },
            { "Poison", 2.0 },
            { "Rock", 2.0 },
            { "Steel", 2.0 },
            { "Grass", 0.5 },
            { "Ice", 0.5 },
            { "Water", 0.5 },
            { "Flying", 0.0 }
        }
    },
    // Flying type interactions
    { "Flying", new Dictionary<string, double>
        {
            { "Grass", 2.0 },
            { "Fighting", 2.0 },
            { "Bug", 2.0 },
            { "Electric", 0.5 },
            { "Rock", 0.5 },
            { "Steel", 0.5 }
        }
    },
    // Psychic type interactions
    { "Psychic", new Dictionary<string, double>
        {
            { "Fighting", 2.0 },
            { "Poison", 2.0 },
            { "Psychic", 0.5 },
            { "Steel", 0.5 },
            { "Dark", 0.0 }
        }
    },
    // Bug type interactions
    { "Bug", new Dictionary<string, double>
        {
            { "Grass", 2.0 },
            { "Psychic", 2.0 },
            { "Dark", 2.0 },
            { "Fire", 0.5 },
            { "Fighting", 0.5 },
            { "Flying", 0.5 },
            { "Poison", 0.5 },
            { "Ghost", 0.5 },
            { "Steel", 0.5 },
            { "Fairy", 0.5 }
        }
    },
    // Rock type interactions
    { "Rock", new Dictionary<string, double>
        {
            { "Fire", 2.0 },
            { "Ice", 2.0 },
            { "Flying", 2.0 },
            { "Bug", 2.0 },
            { "Fighting", 0.5 },
            { "Ground", 0.5 },
            { "Steel", 0.5 }
        }
    },
    // Ghost type interactions
    { "Ghost", new Dictionary<string, double>
        {
            { "Psychic", 2.0 },
            { "Ghost", 2.0 },
            { "Dark", 0.5 },
            { "Normal", 0.0 }
        }
    },
    // Dragon type interactions
    { "Dragon", new Dictionary<string, double>
        {
            { "Dragon", 2.0 },
            { "Steel", 0.5 },
            { "Fairy", 0.0 }
        }
    },
    // Dark type interactions
    { "Dark", new Dictionary<string, double>
        {
            { "Psychic", 2.0 },
            { "Ghost", 2.0 },
            { "Fighting", 0.5 },
            { "Dark", 0.5 },
            { "Fairy", 0.5 }
        }
    },
    // Steel type interactions
    { "Steel", new Dictionary<string, double>
        {
            { "Ice", 2.0 },
            { "Rock", 2.0 },
            { "Fairy", 2.0 },
            { "Fire", 0.5 },
            { "Water", 0.5 },
            { "Electric", 0.5 },
            { "Steel", 0.5 }
        }
    },
    // Fairy type interactions
    { "Fairy", new Dictionary<string, double>
        {
            { "Fighting", 2.0 },
            { "Dragon", 2.0 },
            { "Dark", 2.0 },
            { "Fire", 0.5 },
            { "Poison", 0.5 },
            { "Steel", 0.5 }
        }
    }
};
        public double STABMult(string type1, string type2, string moveType)
        {
            if (type1 == moveType || type2 == moveType)
            {
                return 1.5;
            }
            return 1;
        }
        public double typeEffectiveness(string type, string moveType)
        {
            double effectiveness = 1.0;

            if (typeChart.ContainsKey(moveType) && typeChart[moveType].ContainsKey(type))
            {
                effectiveness = typeChart[moveType][type];
            }

            return effectiveness;
        }
        public double calcMaxDamage(double LV, double BP,string type1, string type2, string type3, string type4,string moveType,double ATK, double DEF)
        {
            double stab = STABMult(type1, type2, moveType);
            double effectiveness1 = typeEffectiveness(moveType,type3);
            double effectiveness2 = typeEffectiveness(moveType,type4);
            double totalEffectiveness = effectiveness1 * effectiveness2;
            double random = 1;
            double start = ((2 * LV) / 5) + 2;
            start = (start * BP)*(ATK/DEF);
            start = (start / 50) + 2;
            start = start * random * stab * totalEffectiveness;
            return start;
        }
        public double calcMinDamage(double LV, double BP, string type1, string type2, string type3, string type4, string moveType, double ATK, double DEF)
        {
            double stab = STABMult(type1, type2, moveType);
            double effectiveness1 = typeEffectiveness(moveType, type3);
            double effectiveness2 = typeEffectiveness(moveType, type4);
            double totalEffectiveness = effectiveness1 * effectiveness2;
            double random = .85;
            double start = ((2 * LV) / 5) + 2;
            start = (start * BP) * (ATK / DEF);
            start = (start / 50) + 2;
            start = start * random * stab * totalEffectiveness;
            return start;
        }

    }
}
