using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Onslaught
{
    
    //main enemy screen occurs from pixel lengths 200 to 800 horizontally
    // and 100 to 400 vertically
    //However, there are no problems with going outside that a little
    class Animator
    {
        public int step;
        public List<int> randInts;

        public Animator()
        {
            randInts = new List<int>();
        }

        public void AnimateSkill(String skiname, List<AnimatedSprite> aniList, SpriteBatch spriteBatch)
        {
            switch (skiname)
            {
                case "Close Quarters":
                        {

                            aniList[0].Draw(spriteBatch, new Vector2(-350 + step * (105-2*step), -50 + step * (48-step)), false);
                            aniList[0].Draw(spriteBatch, new Vector2(725 - step * (105-2*step), -50 + step * (48-step)), true);

                            break;
                        }
                case "Lunge":
                        {
                            if ((step >= 14) && (step <= 28))
                            {
                                aniList[1].Draw(spriteBatch, new Vector2(400, 175), false);
                            }
                            if (step <= 15)
                            {
                                aniList[0].Draw(spriteBatch, new Vector2(450, 300 - step * (10 - (int)(.5 * step))), false);
                            }
                        
                        }
                        break;
                case "Pound":
                        {
                            if (step <= 29)
                            {
                                if (step == 0)
                                {
                                    randInts.Add(RNG.RandInt(300, 500));
                                    randInts.Add(RNG.RandInt(200, 250));
                                    randInts.Add(RNG.RandInt(300, 500));
                                    randInts.Add(RNG.RandInt(200, 250));
                                }
                                if (step == 15)
                                {
                                    randInts[0] = RNG.RandInt(350, 450);
                                    randInts[1] = RNG.RandInt(200, 250);
                                }
                                aniList[0].Draw(spriteBatch, new Vector2(randInts[0], randInts[1]), false);
                                if ((step >= 8) && (step <= 23)) aniList[1].Draw(spriteBatch, new Vector2(randInts[2], randInts[3]), true);
                            }
                            
                        }
                        break;
                case "Thorn Strike":
                        {
                            if (step <= 31) aniList[0].Draw(spriteBatch, new Vector2(225, 100), false);
                            if ((step > 15) && (step <= 47)) aniList[1].Draw(spriteBatch, new Vector2(325, 100), false);
                            if ((step > 31) && (step <= 63)) aniList[2].Draw(spriteBatch, new Vector2(425, 100), false);
                            if ((step > 47) && (step <= 79)) aniList[3].Draw(spriteBatch, new Vector2(525, 100), false);
                            if ((step > 63) && (step <= 95)) aniList[4].Draw(spriteBatch, new Vector2(625, 100), false);
                            if ((step > 79) && (step <= 111)) aniList[5].Draw(spriteBatch, new Vector2(725, 100), false);

                        }
                        break;
            }
            step++;
        }
    }
}
