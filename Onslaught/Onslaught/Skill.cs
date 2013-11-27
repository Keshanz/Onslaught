using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Onslaught
{
    
    
    public class Skill
    {
        public class Anim
        {
            public String name;
            public int rows;
            public int cols;
            public int spriteDelay;
            public Boolean rev;
            //length of an individual animation
            //public int steps;

            public Anim(String nam, int r, int c, int del, Boolean re)
            {
                name = nam;
                rows = r;
                cols = c;
                
                spriteDelay = del;
                rev = re;
               // steps = st;
            }
           

        }


        public int C_pow;
        public int M_pow;
        public int F_pow;
        public int cost;
        public int delayAmnt;
        public int accBoost;
        public String dmgForm;
        public String name;
        public String skiType;
        //Animations in string form of the name of the spritesheet
        public List<Anim> animations;
        //the length of the total animation for the skill
        public int stepLimit;
        public String description;

        //
        //public Texture2D spriteSheet;
        //public int rows;
        //public int cols;
        //public int totalFrames;
        //public int spriteDelay;
        //public Vector2 aniLoc;



        public Skill(String nam)
        {
            name = nam;
            makeSkill(nam);
            
        }

        private void makeSkill(String nam)
        {
            switch (nam)
            {

                case "Close Quarters":
                    {
                        skiType = "Att";
                        C_pow = 50;
                        M_pow = 0;
                        F_pow = 0;
                        cost = 8;
                        delayAmnt = 50;
                        accBoost = 5;
                        dmgForm = "1.1 * str - eDef + 6";
                        animations = new List<Anim> { new Anim("Slash", 1, 1, 0, false) };
                        stepLimit = 26;
                        description = "A basic physical move that attacks enemies close by.  Has a slight accuracy boost.  Other than that, it's a very basic and standard attack. Average delay.";

                        //rows = 3;
                        //cols = 3;
                        //totalFrames = 9;
                        //spriteDelay = 0;
                        // aniLoc = new Vector2(50,50);
                    }
                    break;
                case "Lunge":
                    {
                        skiType = "Att";
                        C_pow = 20;
                        M_pow = 40;
                        F_pow = 0;
                        cost = 10;
                        delayAmnt = 30;
                        accBoost = 0;
                        dmgForm = "ski + str * .5 - eDef + 5";
                        animations = new List<Anim> { new Anim("ArrowBulletSheet", 1, 8, 2, false), new Anim("Hit",1,3,5,false) };
                        stepLimit = 30;
                        description = "Lunges at your opponents.  Hits both close and mid-range, but damage is a little low.  A good move for dealing small bits of damage across the board. Uses both skill and strength. Below average delay. Pierces.";
                    }
                    break;
                case "Pound":
                    {
                        skiType = "Att";
                        C_pow = 60;
                        M_pow = 0;
                        F_pow = 0;
                        cost = 5;
                        delayAmnt = 40;
                        dmgForm = "str - eDef + 3";
                        animations = new List<Anim> { new Anim("Hit", 1, 3, 5, false), new Anim("Hit", 1, 3, 5, false) };
                        stepLimit = 35;
                    }
                    break;
                case "Thorn Strike":
                    {
                        skiType = "Att";
                        C_pow = 0;
                        M_pow = 0;
                        F_pow = 60;
                        cost = 10;
                        delayAmnt = 60;
                        dmgForm = "str + mag - eDef - eFth";
                        animations = new List<Anim> { new Anim("ThornSheet", 1, 16, 2, false), new Anim("ThornSheet", 1, 16, 2, false), new Anim("ThornSheet", 1, 16, 2, false), new Anim("ThornSheet", 1, 16, 2, false), new Anim("ThornSheet", 1, 16, 2, false), new Anim("ThornSheet", 1, 16, 2, false) };
                        stepLimit = 120;
                    }
                    break;
            }
        }


        //useless now
        public String GetName()
        {
            return name;
        }
        /*
        public Vector2 GetLocation()
        {
            return aniLoc;
        }

        public int GetRows()
        {
            return rows;
        }

        public int GetCols()
        {
            return cols;
        }

        public int GetSpriteDelay()
        {
            return spriteDelay;
        }

        public int GetTotalFrames()
        {
            return totalFrames;
        }
         * */
    }
}
