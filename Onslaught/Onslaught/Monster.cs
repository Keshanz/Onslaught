using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Onslaught
{
    class Monster: Unit, IComparable
    {
        public int expOnDeath;
        private static double levelMultiplier = .05;

        public Monster():base()
        {
            
        }

        public Monster(String area):base()
        {
            affil = -1;
            randomMonster(area);
            
        }

        

        public void randomMonster(String area)
        {
            //int to determine the monster that spawns
            int rand = RNG.RandInt(1,100);
            if (area.Equals("Beginner Meadows"))
            {
                //random monster generation begins here
                if (rand <= 60) makeMonster("Buns");
                else if (rand <= 75) makeMonster("Slime");
                else makeMonster("Rafflesia");
            }
            
        }

        public void makeMonster(String nam)
        {
            //idea is to keep all states around some base value and let leveling scale stats accordingly
            int avgLevel;
            if (nam.Equals("Slime"))
            {
                filename = "slime";
                name = "Slime";
                type = "Slime";
                level = RNG.RandInt(2, 4);
                avgLevel = 3;
                str = (int)(RNG.RandInt(50, 60)*(levelMultiplier*(level-avgLevel)+1));
                mag = (int)(RNG.RandInt(35, 45)*(levelMultiplier*(level-avgLevel)+1));
                def = (int)(RNG.RandInt(10, 15)*(levelMultiplier*(level-avgLevel)+1));
                fth = (int)(RNG.RandInt(37, 42)*(levelMultiplier*(level-avgLevel)+1));
                ski = (int)(RNG.RandInt(20, 30)*(levelMultiplier*(level-avgLevel)+1));
                eva = (int)(RNG.RandInt(65, 75)*(levelMultiplier*(level-avgLevel)+1));
                luck = (int)(RNG.RandInt(35, 45)*(levelMultiplier*(level-avgLevel)+1));
                spd = (int)(RNG.RandInt(30, 40)*(levelMultiplier*(level-avgLevel)+1));
                rec = (int)(RNG.RandInt(4, 6)*(levelMultiplier*(level-avgLevel)+1));
                HP = maxHP = (int)(RNG.RandInt(60, 70)*(levelMultiplier*(level-avgLevel)+1));
                MP = maxMP = (int)(RNG.RandInt(30, 35)*(levelMultiplier*(level-avgLevel)+1));
                delay = 40;
                expOnDeath = (int)(30 * (2*levelMultiplier * (level - avgLevel) + 1));
                skillList.Add(new Skill("Pound"));
            }
            //change values later, current is the same as slime
            if (nam.Equals("Buns"))
            {

                filename = "buns";
                name = "Buns";
                type = "Buns";
                level = RNG.RandInt(1, 3);
                avgLevel = 2;
                str = (int)(RNG.RandInt(40, 50) * (levelMultiplier * (level - avgLevel) + 1));
                mag = (int)(RNG.RandInt(0, 5) * (levelMultiplier * (level - avgLevel) + 1));
                def = (int)(RNG.RandInt(25, 30) * (levelMultiplier * (level - avgLevel) + 1));
                fth = (int)(RNG.RandInt(25, 30) * (levelMultiplier * (level - avgLevel) + 1));
                ski = (int)(RNG.RandInt(40, 45) * (levelMultiplier * (level - avgLevel) + 1));
                eva = (int)(RNG.RandInt(50, 55) * (levelMultiplier * (level - avgLevel) + 1));
                luck = (int)(RNG.RandInt(60, 80) * (levelMultiplier * (level - avgLevel) + 1));
                spd = (int)(RNG.RandInt(55, 65) * (levelMultiplier * (level - avgLevel) + 1));
                rec = (int)(RNG.RandInt(4, 6) * (levelMultiplier * (level - avgLevel) + 1));
                HP = maxHP = (int)(RNG.RandInt(30, 40) * (levelMultiplier * (level - avgLevel) + 1));
                MP = maxMP = (int)(RNG.RandInt(15, 20) * (levelMultiplier * (level - avgLevel) + 1));
                delay = 20;
                expOnDeath = (int)(10 * (2*levelMultiplier * (level - avgLevel) + 1));
                skillList.Add(new Skill("Pound"));
            }

            if (nam.Equals("Rafflesia"))
            {

                filename = "rafflesia";
                name = "Rafflesia";
                type = "Rafflesia";
                level = RNG.RandInt(1, 3);
                avgLevel = 3;
                str = (int)(RNG.RandInt(25, 35) * (levelMultiplier * (level - avgLevel) + 1));
                mag = (int)(RNG.RandInt(64, 68) * (levelMultiplier * (level - avgLevel) + 1));
                def = (int)(RNG.RandInt(35, 40) * (levelMultiplier * (level - avgLevel) + 1));
                fth = (int)(RNG.RandInt(20, 25) * (levelMultiplier * (level - avgLevel) + 1));
                ski = (int)(RNG.RandInt(30, 35) * (levelMultiplier * (level - avgLevel) + 1));
                eva = (int)(RNG.RandInt(50, 55) * (levelMultiplier * (level - avgLevel) + 1));
                luck = (int)(RNG.RandInt(35, 45) * (levelMultiplier * (level - avgLevel) + 1));
                spd = (int)(RNG.RandInt(40, 50) * (levelMultiplier * (level - avgLevel) + 1));
                rec = (int)(RNG.RandInt(4, 6) * (levelMultiplier * (level - avgLevel) + 1));
                HP = maxHP = (int)(RNG.RandInt(40, 50) * (levelMultiplier * (level - avgLevel) + 1));
                MP = maxMP = (int)(RNG.RandInt(50, 60) * (levelMultiplier * (level - avgLevel) + 1));
                delay = 30;
                expOnDeath = (int)(20 * (2*levelMultiplier * (level - avgLevel) + 1));
                skillList.Add(new Skill("Thorn Strike"));
            }
        }


    }
}
