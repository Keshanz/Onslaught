using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Onslaught
{
    abstract public class Unit
    {
        public struct status
        {

            public int magnitude;
            public int duration;
            public status( int mag, int dur)
            {

                magnitude = mag;
                duration = dur;
            }
        };

        public struct statusHolder
        {
            public status knockedOut;
            //no constructor needed most likely.  Everything initializes to default values
        };

        public statusHolder statusList;

        public double[] buffList;
       /*public double strBuff;
        public double magBuff;
        public double defBuff;
        public double fthBuff;
        public double skiBuff;
        public double evaBuff;
        public double luckBuff;
        public double spdBuff;*/

        //careful with actual and base stats (buff consideration)

        public int number;
        public int affil;
        public string name;
        public string type;

        //for character, level is... level.. and a prerequisite to learning skills, class changing, etc., may also factor into skill dmg/acc or something
        //for monster, level is generally to be used as a stat and exp multiplier of sorts (level 4 is stronger than level 3, etc)
        public int level;
        
        public int HP;
        public int MP;
        public int maxHP;
        public int maxMP;
        public int str;
        public int mag;
        public int def;
        public int fth;
        public int ski;
        public int eva;
        public int luck;
        public int spd;
        public int rec;

        public double delay;

        public List<Skill> skillList;

        //name used for picture purposes mainly
        public String filename;

        public Unit() {
            buffList = new double[8];
            int i;
            for (i = 0; i<8;i++)
            {
                buffList[i] = 1;
                
            }
            //suscept to changes... obviously
            skillList = new List<Skill>(1);
            statusList = new statusHolder();
            name = "No name";
            type = "No class";
            
        }

        /*
        public void loseHP(int dmg)
        {
            if (dmg < 0) dmg = 0;
            Console.WriteLine(dmg);
            HP -= dmg;
        }

        public void die()
        {
            Console.WriteLine("Shit be dead bro!");
        }*/

        public int CompareTo(Object obj)
        {
            Unit uni = (Unit)obj;
            if ((double)delay / spd > (double)uni.delay / uni.spd) return 1;
            else if ((double)delay / spd < (double)uni.delay / uni.spd) return -1;
            else return 0;

        }

        public override String ToString()
        {
            return (delay + " " + str + " " + mag + " " + def + " " + fth + " " + spd + "" + maxHP + "" + maxMP);
        }
    }
}
