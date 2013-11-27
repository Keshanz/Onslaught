using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Onslaught
{
    class Char:Unit,IComparable
    {

        //static Dictionary<String, Skill> pSkills = new Dictionary<String, Skill>();
        public int exp;
        public int expToLevel;

        /*
        public static void fillSkills()
        {
            //go back and flesh out this skill more
            //precondition: delay is at least 1
            pSkills.Add("Basic Close Combat", new Skill("Basic Close Combat"));
        }*/

        public Char(): base()
        {
            level = 1;
            exp = 0;
            expToLevel = 30;
            str = RNG.RandInt(45,55);
            mag = RNG.RandInt(10,15);
            def = RNG.RandInt(45, 55);
            fth = RNG.RandInt(20, 30);
            ski = RNG.RandInt(45, 55);
            eva = RNG.RandInt(30, 40);
            luck = RNG.RandInt(30, 40);
            spd = RNG.RandInt(45, 55);
            rec = RNG.RandInt(4, 6);
            HP = maxHP = RNG.RandInt(60, 80);
            MP = maxMP = RNG.RandInt(25, 35);
            delay = 30;
            //filename should be based on class and ?gender?, temporary is rafflesia
            filename = "trainee";
            name = "pchar";
            addSkill("Close Quarters");
            addSkill("Lunge");
        }

        public void addSkill(String skiName)
        {
                //max number of skills is 6
                //previously added pSkills[skiName]
            if (skillList.Count <= 6)
            {
                skillList.Add(new Skill(skiName));

            }
            else
            {
                //skill replace code
            }
        }


        public void levelUp()
        {

        }


        
    }
}
