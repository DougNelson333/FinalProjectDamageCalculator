using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageCalculator
{
    internal class BaseStatCalculation
    {
        public int CalcHPStat(double Base, double IV, double EV,double LV)
        {
            double stat = ((2 * Base + IV + Math.Floor(EV / 4)) * LV) / 100;
            stat = Math.Floor(stat) + LV + 10;
            return Convert.ToInt32(stat);
        }
        public int CalcOtherStat(double Base, double IV, double EV, double LV)
        {
            double stat = 0;
            stat = Math.Floor(((2*Base+IV+Math.Floor(EV/4))*LV)/100)+5;
            return Convert.ToInt32(stat);
        }
    }
}
