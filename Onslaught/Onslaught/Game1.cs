using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

/*To Do List
 * 1. All advance, all retreat
 * 2. Experience and level up
 * 3. Flesh out Monster skills and AI
 * 4. Char death 
 * 5. Experience and level up
 * 6. Classes
 * 7. General cleanup of a bunch of shit: Null distance, numbers over monsters, etc/.
 * 8. Making shit appear in the profile boxes
 * 9. Parantheses in Formula Evaluator/ Order of Operations (may be unnecessary)
 *
 *
 */

/*Ideas/Things to Remember
 * 
 * Skill and spd control crit hit chance
 * Skill also controls hit along with level.  Formula for hit% is (100 * (1 + (Level - En.Level)/10) + Skill - En.Eva)
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * */

namespace Onslaught
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        List<Char> P1Chars;
        List<Char> P2Chars;
        List<Monster> Mons;
        List<Unit> AllUnits;
        List<Unit> eneList;
        int[][] DistGrid;
        Unit curUnit;
        Skill curSkill;
        Dictionary<int, int> damageList;
        List<int> dmgList;
        Dictionary<String, Texture2D> spriteList;

        MouseState mouse;
        Boolean isClicking;

        Animator animator;
        List<AnimatedSprite> animations;

        Texture2D mainscr, bluTur, redTur, purTur, skillBox, bar, fillBar, barEnd, arrow;
        Texture2D cc,vc,mm,ff,vf;
        Texture2D buns, rafflesia, slime;
        Texture2D trainee;
        Texture2D background;
        Texture2D  miss;

        SpriteFont infoFont;
        SpriteFont skillFont;

        //AnimatedSprite animation;

        FormulaEvaluator evaluator;

        String area;
        String info;

        //float used for "animation" of death
        float alphaController;

        //would very much like to make a better system for choosing enemy units...
        enum Phase
        {
            Initialization,
            TurnStart,
            SkillChoice,
            Animation,
            Resolution,
            EnemyChoice,
            Info
        }
        Phase phase;
        String EnemyChoiceHelper;

        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1000;
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            //Char.fillSkills();
            phase = Phase.Initialization;

            animator = new Animator();

            evaluator = new FormulaEvaluator();

            area = "Beginner Meadows";

            P1Chars = new List<Char>();
            P2Chars = new List<Char>();
            Mons = new List<Monster>();
            eneList = new List<Unit>();
            dmgList = new List<int>();

            int i;
            for (i = 0; i <= 2; i++)
            {
                P1Chars.Add(new Char());
                P1Chars[i].number = i;
                P1Chars[i].affil = 1;
                P1Chars[i].name += P1Chars[i].number;
                P2Chars.Add(new Char());
                P2Chars[i].number = i;
                P2Chars[i].affil = 2;
                P2Chars[i].name += P2Chars[i].number;
                Mons.Add(new Monster(area));
                Mons[i].number = i + 3;

                Mons[i].name += Mons[i].number;
                
            }
            //delete later maybe?
            /*
            for (i = 3; i <= 4; i++)
            {
               
                Mons.Add(new Monster(area));
                Mons[i].number = i + 3;
                Mons[i].affil = -1;
                Mons[i].name += Mons[i].number;

            }*/

                      
            AllUnits = new List<Unit>();
            AllUnits.AddRange(P1Chars);
            AllUnits.AddRange(P2Chars);
            AllUnits.AddRange(Mons);
            AllUnits.Sort();

            //Console.WriteLine(AllUnits[0].name);

            DistGrid = new int[][] 
            {
                new int[] {RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),0,0},
                new int[] {RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),0,0},
                new int[] {RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5),0,0},
                new int[] {RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5)},
                new int[] {RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5)},
                new int[] {RNG.RandInt(1,5),RNG.RandInt(1,5),RNG.RandInt(1,5)},
                new int[] {0,0,0},
                new int[] {0,0,0}
            };

            
            

            TurnStart();
        }



        public void TurnStart()
        {
            phase = Phase.TurnStart;
            curUnit = AllUnits[0];
            //for now, skip monster units
            eneList.Clear();
            if (curUnit.affil == 1)
            {
                eneList.AddRange(P2Chars);
                eneList.AddRange(Mons);

            }
            else if (curUnit.affil == 2)
            {
                eneList.AddRange(P1Chars);
                eneList.AddRange(Mons);
            }
            else
            {
                eneList.AddRange(P1Chars);
                eneList.AddRange(P2Chars);
            }
            
        }


        //takes care of delay 
        public void TurnEnd()
        {
    
            

            //maybe put another phase here for showing that the monster actually spawns
            
            if ((Mons.Count != 5) && (RNG.RandInt(1, 1 + (int) (1.5 * Mons.Count)) == 1))
            {
               
                //assumes that player spots will never be nonexistant
                int b = 3;
                while (DistGrid[0][b] != 0)
                    b++;

                for (int a = 0; a < 3; a++)
                {
                    DistGrid[b][a] = RNG.RandInt(1, 5);
                    DistGrid[a][b] = RNG.RandInt(1, 5);
                }
                Monster mon = new Monster(area);
                mon.number = b;
                mon.name += mon.number;
                Mons.Add(mon);
                AllUnits.Add(mon);
                ShowInfo(mon.name + " has spawned!");
                //Console.WriteLine(mon.name + " has spawned!");

            }

            int i;
            int j;

            AllUnits.Sort();   
            while (AllUnits[0].delay > 0)
            {
                for (i = 0; i <= AllUnits.Count - 1; i++)
                {
                    AllUnits[i].delay -= AllUnits[i].spd/50.0;
                    for(j = 0; j < 7; j++)
                    {

                        if (AllUnits[i].buffList[j] > 1) AllUnits[i].buffList[j] -= .002;
                        else if (AllUnits[i].buffList[j] < 1) AllUnits[i].buffList[j] += .002;
                        
                        if (Math.Abs(AllUnits[i].buffList[j] - 1) < .001) AllUnits[i].buffList[j] = 1;
                    }
                }
            }
                    
            


            //for now, put turnstart here
            TurnStart();
           
        }

        //calculates and returns array of damage with damages corresponding to enemylist
        //actual damage dealing is also done in this step
        List<int> calculateDmgs(List<Unit> enList)
        {
            int dmg ;
            List<int> dList = new List<int>();
            int rows = 0;
            int cols = 0;
            
            for (int b = 0; b < enList.Count; b++)
            {
                if ((curUnit.affil==1)||(curUnit.affil==-1 && b >= 3))
                {
                    rows = curUnit.number;
                    cols = enList[b].number;
                }
                else if ((curUnit.affil == 2)||(curUnit.affil==-1 && b <= 2))
                {
                    rows = enList[b].number;
                    cols = curUnit.number;
                }
                if (DistGrid[rows][cols] <= 2 && curSkill.C_pow > 0)
                {
                    Console.WriteLine("Close attack initiated");
                    dmg = (int)Math.Round(evaluator.Evaluate(curSkill.dmgForm, curUnit, enList[b]) * curSkill.C_pow * .01, 0);

                }
                else if (DistGrid[rows][cols] == 3 && curSkill.M_pow > 0)
                {
                    Console.WriteLine("Mid-range attack initiated");
                    dmg = (int)Math.Round(evaluator.Evaluate(curSkill.dmgForm, curUnit, enList[b]) * curSkill.M_pow * .01, 0);

                }
                else if (DistGrid[rows][cols] >= 4 && curSkill.F_pow > 0)
                {
                    Console.WriteLine("Far attack initiated");
                    dmg = (int)Math.Round(evaluator.Evaluate(curSkill.dmgForm, curUnit, enList[b]) * curSkill.F_pow * .01, 0);

                }
                else
                {
                    dmg = -1;
                    dList.Add(dmg);
                    continue;
                }

                //evasion check
                if (RNG.RandInt(1, 100) > ((int)(100 * (1 + (curUnit.level - enList[b].level) / 10.0)) + curSkill.accBoost + curUnit.ski - enList[b].eva))
                {
                    dmg = -999;
                    dList.Add(dmg);
                    continue;
                }



                /*
                enList[b].loseHP(dmg);
                if (enList[b].HP <= 0)
                {
                    //if it is a monster, remove it from play, grant exp, etc.
                    if (enList[b].affil == -1)
                    {
                        AllUnits.Remove(enList[b]);
                        Char curChar = (Char)curUnit;
                        Monster curMons = (Monster)enList[b];
                        curChar.exp += curMons.expOnDeath;
                        //level up implementation goes hier
                        
                        
                    }
                    //if it is a player, knock out for a period of time depending on the amount of excessive damage
                    if (enList[b].affil != -1)
                    {
                        if (enList[b].HP <= -enList[b].maxHP)
                        {
                            enList[b].statusList.knockedOut.duration = 5;
                        }
                        else if (enList[b].HP <= -.5 * enList[b].maxHP)
                        {
                            enList[b].statusList.knockedOut.duration = 4;
                        }
                        else if (enList[b].HP <= -.25 * enList[b].maxHP)
                        {
                            enList[b].statusList.knockedOut.duration = 3;
                        }
                        else
                        {
                            enList[b].statusList.knockedOut.duration = 2;
                        }
                        enList[b].HP = 0;
                        //for now, there is no particular bonus for killing enemy players, maybe implement some sort of
                        //skill point system later where players get sp instead of exp.... or just add exp...
                    }
                }
                */

                if (dmg < 0) dmg = 0;
                /*
                if (enList[b].affil == -1)
                Console.WriteLine("Player " + curUnit.affil + " has dealt " + dmg + " damage to Player " + enList[b].affil + "'s unit " + (enList[b].number-3));
                else
                Console.WriteLine("Player " + curUnit.affil + " has dealt " + dmg + " damage to Player " + enList[b].affil + "'s unit " + enList[b].number );
                */

                dList.Add(dmg);
            }
            return dList;
        }

        //for some reason I decided to only make this for the current unit...Might be better to make it for any two
        protected int getDistance(Unit un)
        {
            if ((curUnit.affil == 1)||(curUnit.affil == -1 && un.affil != 1))
            {
                return DistGrid[curUnit.number][un.number];
            }
            else
            {
                return DistGrid[un.number][curUnit.number];
            }
            
        }

        //sets the distance of the current unit with an enemy unit
        protected void setDistance(Unit un, int newDist)
        {
            if ((curUnit.affil == 1) || (curUnit.affil == -1 && un.affil != 1))
            {
                DistGrid[curUnit.number][un.number] = newDist;
            }
            else
            {
                DistGrid[un.number][curUnit.number] = newDist;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            //TurnStart();
            //AllUnits[0].delay = 50;
            //Console.WriteLine(P1Chars[0].buffList[0]);
            //Console.WriteLine(dummy.buffList[0]);
            //TurnEnd();

            mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                isClicking = true;
                Console.WriteLine(mouse.X + " " + mouse.Y);
            }
            
            switch (phase)
            {


                case Phase.TurnStart:
                    if ((curUnit.affil != -1) && (isClicking && mouse.LeftButton == ButtonState.Released))
                    {
                        if (new Rectangle(280,415,100,30).Contains(mouse.X, mouse.Y))
                        {
                            
                            phase = Phase.SkillChoice;

                        }
                        else if (new Rectangle(170, 415, 100, 30).Contains(mouse.X, mouse.Y))
                        {
                            EnemyChoiceHelper = "Advance";
                            phase = Phase.EnemyChoice;
                        }
                        else if (new Rectangle(170, 455, 100, 30).Contains(mouse.X, mouse.Y))
                        {
                            AllAdvance();
                            curUnit.delay += 50;
                            TurnEnd();
                        }
                        else if (new Rectangle(170, 495, 100, 30).Contains(mouse.X, mouse.Y))
                        {
                            EnemyChoiceHelper = "Retreat";
                            phase = Phase.EnemyChoice;
                        }
                        else if (new Rectangle(170, 535, 100, 30).Contains(mouse.X, mouse.Y))
                        {
                            AllRetreat();
                            curUnit.delay += 50;
                            TurnEnd();
                        }
                    }
                    if (curUnit.affil == -1)
                    {
                        animations = new List<AnimatedSprite>();
                        ShowInfo(curUnit.name + " is thinking about what to do.");
                        MonsterAI();
                        if (animations.Count != 0)
                        {
                            phase = Phase.Animation;
                        }
                        else
                        {
                            TurnEnd();
                        }
                    }
                    break;
                case Phase.SkillChoice:
                    {

                        //add bulk later, essentially more buttons and shiz


                        
                        if ((isClicking) && (mouse.LeftButton == ButtonState.Released) && (mouse.X >= 165) && (mouse.X <= 385))
                        {
                            try
                            {
                                curSkill = curUnit.skillList[(int)((mouse.Y - 421) / 20)];

                                ExecuteSkill();
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                            }
                        }
                        if (mouse.RightButton == ButtonState.Pressed)
                        {
                            phase = Phase.TurnStart;
                        }
                    }
                    break;
                case Phase.Animation:
                    {
                        if (animator.step == curSkill.stepLimit) phase = Phase.Resolution;
                    }
                    break;
                case Phase.Resolution:
                    {


                        if ((isClicking) && (mouse.LeftButton == ButtonState.Released))
                        {
                            for (int i = 0; i < eneList.Count; i++)
                            {
                                if (dmgList[i] > 0)
                                {
                                    eneList[i].HP -= dmgList[i];
                                    if (eneList[i].HP <= 0)
                                    {
                                        eneList[i].HP = 0;
                                        if (eneList[i].affil == -1)
                                        {
                                            //animation on death!
                                            alphaController = 1;
                                            ShowInfo(eneList[i].name + " has died! " + curUnit.name + " has gained " + ((Monster)(eneList[i])).expOnDeath + " experience!");

                                            Char curChar = (Char)curUnit;
                                            curChar.exp += ((Monster)(eneList[i])).expOnDeath;

                                            while (curChar.exp >= curChar.expToLevel)
                                            {
                                                curChar.exp -= curChar.expToLevel;
                                                curChar.expToLevel = (int)(curChar.expToLevel * 1.11);
                                                LevelUp(curChar);
                                            }

                                            for (int a = 0; a <= 2; a++)
                                            {
                                               
                                                DistGrid[eneList[i].number][a] = 0;
                                                DistGrid[a][eneList[i].number] = 0;
                                            }
                                            

                                            Mons.Remove((Monster) eneList[i]);
                                            AllUnits.Remove(eneList[i]);
                                            eneList.Remove(eneList[i]);
                                            dmgList.RemoveAt(i);
                                            i--;


                                        }

                                    }
                                }
                            }
                            TurnEnd();
                        }
                    }
                    break;
                case Phase.EnemyChoice:
                    {
                        //25 for the delay as of now
                        if ((isClicking) && (mouse.LeftButton == ButtonState.Released) && (new Rectangle(165, 421, 220, 15 * eneList.Count).Contains(mouse.X,mouse.Y)))
                        {
                            if ((EnemyChoiceHelper.Equals("Advance"))&&(getDistance(eneList[(mouse.Y-421)/15]) >= 2)) Advance(eneList[(mouse.Y - 421) / 15]);
                            else if ((EnemyChoiceHelper.Equals("Retreat"))&&(getDistance(eneList[(mouse.Y-421)/15]) <= 4)) Retreat(eneList[(mouse.Y - 421) / 15]);
                            curUnit.delay += 25;
                            TurnEnd();
                        }
                        
                    }
                    break;
                default:
                    //Console.WriteLine("No User Input Required");
                    break;
                    
            }
            if (mouse.LeftButton == ButtonState.Released) isClicking = false;

            base.Update(gameTime);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteList = new Dictionary<string, Texture2D>();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("BeginnerMeadowsBackground");
            mainscr = Content.Load<Texture2D>("MainScreen");
            skillBox = Content.Load<Texture2D>("Skillbox");
            bluTur = Content.Load<Texture2D>("blueturn");
            redTur = Content.Load<Texture2D>("redturn");
            purTur = Content.Load<Texture2D>("purpturn");
            arrow = Content.Load<Texture2D>("Arrow");
            bar = Content.Load<Texture2D>("Bar");
            barEnd = Content.Load<Texture2D>("BarEnd");
            fillBar = Content.Load<Texture2D>("Fillbar");

            vc = Content.Load<Texture2D>("VC");
            cc = Content.Load<Texture2D>("C");
            mm = Content.Load<Texture2D>("M");
            ff = Content.Load<Texture2D>("F");
            vf = Content.Load<Texture2D>("VF");
            rafflesia = Content.Load<Texture2D>("Rafflesia");
            slime = Content.Load<Texture2D>("Slime");
            buns = Content.Load<Texture2D>("Buns");

            miss = Content.Load<Texture2D>("Miss");

            trainee = Content.Load<Texture2D>("Trainee");


            infoFont = Content.Load<SpriteFont>("Arial");
            skillFont = Content.Load<SpriteFont>("Impact");

            spriteList.Add("slime", slime);
            spriteList.Add("buns", buns);
            spriteList.Add("rafflesia", rafflesia);
            spriteList.Add("trainee", trainee);
 
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            // Add your drawing code here
            
            

            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(mainscr, new Vector2(0,0), Color.White);
            
            
            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < DistGrid[a].Length; b++)
                {
                    //change picture to null distance later
                    if (DistGrid[a][b] == 0) spriteBatch.Draw(cc, new Vector2(630 + 25 * b, 390 + a * 25), Color.White);

                    if (DistGrid[a][b] == 1) spriteBatch.Draw(vc, new Vector2(630 + 25 * b, 390 + a * 25), Color.White);
                    if (DistGrid[a][b] == 2) spriteBatch.Draw(cc, new Vector2(630 + 25 * b, 390 + a * 25), Color.White);
                    if (DistGrid[a][b] == 3) spriteBatch.Draw(mm, new Vector2(630 + 25 * b, 390 + a * 25), Color.White);
                    if (DistGrid[a][b] == 4) spriteBatch.Draw(ff, new Vector2(630 + 25 * b, 390 + a * 25), Color.White);
                    if (DistGrid[a][b] == 5) spriteBatch.Draw(vf, new Vector2(630 + 25 * b, 390 + a * 25), Color.White);
                }
            }
            
            
            for (int a = 6; a >= 0; a--)
            {
                //Console.WriteLine(AllUnits.Count);
                try
                {
                    if (AllUnits[a].affil == 1) spriteBatch.Draw(redTur, new Vector2(170 + 92 * a, 60), Color.White);
                    if (AllUnits[a].affil == 2) spriteBatch.Draw(bluTur, new Vector2(170 + 92 * a, 60), Color.White);
                    if (AllUnits[a].affil == -1) spriteBatch.Draw(purTur, new Vector2(170 + 92 * a, 60), Color.White);
                    DrawString(AllUnits[a].name, new Rectangle(224 + 92 * a - (int)(AllUnits[a].name.Length/2.0*5) , 63, 80, 20),infoFont);


                }
                catch (Exception e)
                {/*
                    if (AllUnits[a%AllUnits.Count].affil == 1) spriteBatch.Draw(redTur, new Vector2(170 + 92 * a, 60), Color.White);
                    if (AllUnits[a%AllUnits.Count].affil == 2) spriteBatch.Draw(bluTur, new Vector2(170 + 92 * a, 60), Color.White);
                    if (AllUnits[a%AllUnits.Count].affil == -1) spriteBatch.Draw(purTur, new Vector2(170 + 92 * a, 60), Color.White);
                    DrawString(AllUnits[a].name, new Rectangle(224 + 92 * a - (int)(AllUnits[a].name.Length / 2.0 * 5), 63, 80, 20),infoFont);*/
                }
            }
            List<Unit>[] temp = new List<Unit>[5] {new List<Unit>(),new List<Unit>(),new List<Unit>(),new List<Unit>(),new List<Unit>()};            
            List<int>[] temp2 = new List<int>[5] { new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>() };
            for (int a = 0; a < eneList.Count; a++)
            {
                    //Console.WriteLine(getDistance(eneList[a]));
                    temp[getDistance(eneList[a]) - 1].Add(eneList[a]);
                    if (phase == Phase.Resolution) temp2[getDistance(eneList[a]) - 1].Add(dmgList[a]);
            }
            
            for (int a = 4; a >= 0; a--)
            {
                int disStart = (int) (520-(85*temp[a].Count/2.0));
                for (int b = 0; b < temp[a].Count; b++)
                {
                    //Console.WriteLine(a);
                    if (temp[a][b].HP <= 0)
                    {
                        spriteBatch.Draw(spriteList[temp[a][b].filename], new Vector2(disStart + (b * 85), 85 + ((4 - a) * 60)), Color.White * alphaController);
                    }
                    else
                    spriteBatch.Draw(spriteList[temp[a][b].filename], new Vector2(disStart + (b * 85), 85 + ((4-a) * 60)), Color.White);

                    if ((phase == Phase.Resolution) && (temp2[a][b] != -1))
                    {
                        //this will obviously be changed to better text later
                        if (temp2[a][b] == -999) spriteBatch.Draw(miss, new Rectangle(disStart + (b * 85) - 10, 105 + ((4 - a) * 60), 60, 20), Color.White);
                        //DrawString("MISS", new Rectangle(disStart + (b * 85), 95 + ((4 - a) * 60), 40, 40), infoFont);
                        else
                            DrawDmg(temp2[a][b], disStart + (b * 85) - 10, 105 + ((4 - a) * 60));
                           // DrawString("" + temp2[a][b], new Rectangle(disStart + (b * 85) , 95 + ((4 - a) * 60), 40, 40), infoFont);
                    }
                    //meh placement
                    //used for drawing the arrow when shit needs to be drawn
                    if (phase == Phase.EnemyChoice)
                    {
                        if ((new Rectangle(165, 421, 220, 15 * eneList.Count).Contains(mouse.X,mouse.Y))&&(eneList[(mouse.Y-421)/15].Equals(temp[a][b])))
                        {
                            spriteBatch.Draw(arrow, new Rectangle(disStart + 10 + (b * 85), 70 + ((4-a) * 60), 20, 20), Color.White);
                        }
                        /*
                        for(int i = 0; i < eneList.Count;i++)
                        if (new Rectangle(165, 421 + i * 15, 220, 15).Contains(mouse.X, mouse.Y))
                        {

                        }*/
                    }
                }
            }
            for (int i = 0; i < P1Chars.Count; i++)
            {
                spriteBatch.Draw(spriteList[P1Chars[i].filename], new Vector2(65, 70 + 180 * i), Color.White);
                DrawString(P1Chars[i].name, new Vector2(85, 135 + 180 * i),infoFont);
                DrawString("Level " + P1Chars[i].level, new Vector2(85, 150 + 180 * i), infoFont);
                DrawString("HP: ", new Rectangle(25, 170 + 180 * i, 40, 40), infoFont);
                DrawString("MP: ", new Rectangle(25, 190 + 180 * i, 40, 40), infoFont);
                DrawString("XP: ", new Rectangle(25, 210 + 180 * i, 40, 40), infoFont);
                spriteBatch.Draw(bar, new Rectangle(45, 170 + 180 * i, 100, 15), Color.Red);
                spriteBatch.Draw(fillBar, new Rectangle(45, 170 + 180 * i, (int)(99 * P1Chars[i].HP / (double)(P1Chars[i].maxHP)), 15), new Rectangle(0, 0, (int)(99 * P1Chars[i].HP / (double)(P1Chars[i].maxHP)), 15), Color.Red);
                if ((int)(99 * P1Chars[i].HP / (double)(P1Chars[i].maxHP))>3) spriteBatch.Draw(barEnd, new Vector2(43 + (int)(99 * P1Chars[i].HP / (double)(P1Chars[i].maxHP)), 170 + 180 * i), Color.Red);
                spriteBatch.Draw(bar, new Rectangle(45, 190 + 180 * i, 100, 15), Color.Blue);
                spriteBatch.Draw(fillBar, new Rectangle(45, 190 + 180 * i, (int)(99 * P1Chars[i].MP / (double)(P1Chars[i].maxMP)), 15), new Rectangle(0, 0, (int)(99 * P1Chars[i].MP / (double)(P1Chars[i].maxMP)), 15), Color.Blue);
                if ((int)(99 * P1Chars[i].MP / (double)(P1Chars[i].maxMP)) > 3)  spriteBatch.Draw(barEnd, new Vector2(43 + (int)(99 * P1Chars[i].MP / (double)(P1Chars[i].maxMP)), 190 + 180 * i), Color.Blue);
                spriteBatch.Draw(bar, new Rectangle(45, 210 + 180 * i, 100, 15), Color.YellowGreen);
                spriteBatch.Draw(fillBar, new Rectangle(45, 210 + 180 * i, (int)(99 * P1Chars[i].exp / (double)(P1Chars[i].expToLevel)), 15), new Rectangle(0, 0, (int)(99 * P1Chars[i].exp / (double)(P1Chars[i].expToLevel)), 15), Color.YellowGreen);
                if ((int)(99 * P1Chars[i].exp / (double)(P1Chars[i].expToLevel)) > 3)  spriteBatch.Draw(barEnd, new Vector2(43 + (int)(99 * P1Chars[i].exp / (double)(P1Chars[i].expToLevel)), 210 + 180 * i), Color.YellowGreen);
                DrawString(P1Chars[i].HP + "/" + P1Chars[i].maxHP, new Vector2(95, 170 + 180 * i), infoFont);
                DrawString(P1Chars[i].MP + "/" + P1Chars[i].maxMP, new Vector2(95, 190 + 180 * i), infoFont);
                //DrawString(P1Chars[i].exp + "/" + P1Chars[i].expToLevel, new Vector2(95, 210 + 180 * i), infoFont);
            }
            for (int i = 0; i < P2Chars.Count; i++)
            {
                spriteBatch.Draw(spriteList[P2Chars[i].filename], new Vector2(895, 70 + 180 * i), Color.White);
                DrawString(P2Chars[i].name, new Vector2(915, 135 + 180 * i), infoFont);
                DrawString("Level " + P2Chars[i].level, new Vector2(915, 150 + 180 * i), infoFont);
                DrawString("HP: ", new Rectangle(855, 170 + 180 * i, 40, 40), infoFont);
                DrawString("MP: ", new Rectangle(855, 190 + 180 * i, 40, 40), infoFont);
                DrawString("XP: ", new Rectangle(855, 210 + 180 * i, 40, 40), infoFont);
                spriteBatch.Draw(bar, new Rectangle(875, 170 + 180 * i, 100, 15), Color.Red);
                spriteBatch.Draw(fillBar, new Rectangle(875, 170 + 180 * i, (int)(99 * P2Chars[i].HP / (double)(P2Chars[i].maxHP)), 15), new Rectangle(0, 0, (int)(99 * P2Chars[i].HP / (double)(P2Chars[i].maxHP)), 15), Color.Red);
                if ((int)(99 * P2Chars[i].HP / (double)(P2Chars[i].maxHP)) > 3) spriteBatch.Draw(barEnd, new Vector2(873 + (int)(99 * P2Chars[i].HP / (double)(P2Chars[i].maxHP)), 170 + 180 * i), Color.Red);
                spriteBatch.Draw(bar, new Rectangle(875, 190 + 180 * i, 100, 15), Color.Blue);
                spriteBatch.Draw(fillBar, new Rectangle(875, 190 + 180 * i, (int)(99 * P2Chars[i].MP / (double)(P2Chars[i].maxMP)), 15), new Rectangle(0, 0, (int)(99 * P2Chars[i].MP / (double)(P2Chars[i].maxMP)), 15), Color.Blue);
                if ((int)(99 * P2Chars[i].MP / (double)(P2Chars[i].maxMP)) > 3) spriteBatch.Draw(barEnd, new Vector2(873 + (int)(99 * P2Chars[i].MP / (double)(P2Chars[i].maxMP)), 190 + 180 * i), Color.Blue);
                spriteBatch.Draw(bar, new Rectangle(875, 210 + 180 * i, 100, 15), Color.YellowGreen);
                spriteBatch.Draw(fillBar, new Rectangle(875, 210 + 180 * i, (int)(99 * P2Chars[i].exp / (double)(P2Chars[i].expToLevel)), 15), new Rectangle(0, 0, (int)(99 * P2Chars[i].exp / (double)(P2Chars[i].expToLevel)), 15), Color.YellowGreen);
                if ((int)(99 * P2Chars[i].exp / (double)(P2Chars[i].expToLevel)) > 3) spriteBatch.Draw(barEnd, new Vector2(873 + (int)(99 * P2Chars[i].exp / (double)(P2Chars[i].expToLevel)), 210 + 180 * i), Color.YellowGreen);
                DrawString(P2Chars[i].HP + "/" + P2Chars[i].maxHP, new Vector2(925, 170 + 180 * i), infoFont);
                DrawString(P2Chars[i].MP + "/" + P2Chars[i].maxMP, new Vector2(925, 190 + 180 * i), infoFont);
                //DrawString(P2Chars[i].exp + "/" + P2Chars[i].expToLevel, new Vector2(925, 210 + 180 * i), infoFont);
            }

            spriteBatch.End();

            switch (phase) 
            {
                case Phase.SkillChoice:
                    {
                        //220 x 168 skillbox
                        //leave 10 on both sides for margins
                        //120/6 about 20 per choice with text at height 12 pxls (space between text 4 + 4)
                        //6 choices for skills, one for a back button
                        //back button is larger, 28 total actually
                        spriteBatch.Begin();
                        spriteBatch.Draw(skillBox, new Vector2(165, 407), Color.White);
                        for (int i = 0; i < curUnit.skillList.Count; i++)
                        {
                            DrawString(curUnit.skillList[i].name, new Rectangle(275-(int)(skillFont.MeasureString(curUnit.skillList[i].name).X/2),421 + (20 * i),220,20), skillFont);
                            if (new Rectangle(165, 421 + i * 20, 220, 20).Contains(mouse.X,mouse.Y))
                            {
                                DrawString("C: " + curUnit.skillList[i].C_pow + "%", new Rectangle(400, 395, 60, 20), infoFont);
                                DrawString("M: " + curUnit.skillList[i].M_pow + "%", new Rectangle(475, 395, 60, 20), infoFont);
                                DrawString("F: " + curUnit.skillList[i].F_pow + "%", new Rectangle(550, 395, 60, 20), infoFont);
                                DrawString(curUnit.skillList[i].description, new Rectangle(400, 410, 275, 400), infoFont);
                            }
                        }
                        
                        spriteBatch.End();
                        break;
                    }
                case Phase.EnemyChoice:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(skillBox, new Vector2(165, 407), Color.White);
                        for (int i = 0; i < eneList.Count; i++)
                        {
                            DrawString(eneList[i].name, new Rectangle(275 - (int)(skillFont.MeasureString(eneList[i].name).X / 2), 421 + (15 * i), 220, 20), skillFont);
                            
                        }

                        spriteBatch.End();
                        break;
                    }
                case Phase.Animation:
                    {
                        //Animation works by taking in a bunch of paramaters stored in Skill's inner class, Anim, such as the name of the spritesheet, number of rows, total frames etc.
                        //The program then forms a list of animatedsprites for the animator to use
                        //The animation parameters are currently at the end of the skillchoice phase in update
                        
                        animator.AnimateSkill(curSkill.GetName(), animations, spriteBatch);
                        //animation.Draw(spriteBatch, curSkill.GetLocation());
                    }
                    break;
                case Phase.Resolution:
                    {
                        //moved to more suitable location (after all the monster sprites are drawn)... Reorganize this shit later i suppose 
                    }
                    break;
                case Phase.Info:
                    {
                        alphaController -= (float) (.04);
                        spriteBatch.Begin();
                        DrawString(info, new Rectangle(400, 395, 275, 400), infoFont);
                        spriteBatch.End();
                    }
                    break;
                default:
                    {
                        
                    }
                    break;
            }
        }

        protected void DrawDmg(int dam, int x, int y)
        {
            String damage = "" + dam;
            for (int i = damage.Length-1; i >= 0; i--)
            {

                spriteBatch.Draw(Content.Load<Texture2D>("dmg" + damage.Substring(i, 1)), new Rectangle(x + (i*15), y, 20, 20), Color.White);
                

            }
        }

        //draws a string within the boundaries of a box and automaticallly wordwraps
        //precondition: string fits within box as a whole
        protected void DrawString(string str, Rectangle txtbox, SpriteFont font)
        {
            
            int yvalue = txtbox.Top;
            int lineWidth = (int) (txtbox.Width / font.MeasureString("R").X);
            //spriteBatch.Begin();
            //Console.WriteLine(str);
            while (str.Length > lineWidth)
            {
                if (str[lineWidth-1].Equals(' '))
                {
                spriteBatch.DrawString(font, str.Substring(0, lineWidth), new Vector2(txtbox.Left, yvalue), Color.Black);
                str = str.Substring(lineWidth);
                }
                else if (str[lineWidth].Equals(' '))
                {
                    spriteBatch.DrawString(font, str.Substring(0, lineWidth), new Vector2(txtbox.Left, yvalue), Color.Black);
                    str = str.Substring(lineWidth+1);
                }
                else
                {
                    int spChar = lineWidth;

                    while (!str[spChar].Equals(' '))
                    {
                        spChar--;
                    }
                    spriteBatch.DrawString(font, str.Substring(0, spChar), new Vector2(txtbox.Left, yvalue), Color.Black);
                    str = str.Substring(spChar + 1);
                }
                yvalue += (int)(font.MeasureString("L").Y + 2);
            }
            spriteBatch.DrawString(font, str.Substring(0, str.Length), new Vector2(txtbox.Left, yvalue), Color.Black);
            //spriteBatch.End();
        }

        protected void DrawString(string str, Vector2 center, SpriteFont font)
        {
            
         
                spriteBatch.DrawString(font, str, new Vector2(center.X - (int) (.5 * font.MeasureString(str).X), center.Y), Color.Black);
            
        }

        protected void ShowInfo(String information)
        {
            Phase temp = phase;
            phase = Phase.Info;
            info = information;
            while (mouse.LeftButton != ButtonState.Pressed)
            {
                

                Tick();
                //BeginDraw();
                //Update(new GameTime());
                //Draw(new GameTime());
                //EndDraw();
                mouse = Mouse.GetState();
            }
            info = "";
            phase = temp;
            while (mouse.LeftButton == ButtonState.Pressed)
            {
                mouse = Mouse.GetState();
            }
            
        }

        protected void ExecuteSkill()
        {
            if (curSkill.skiType.Equals("Att"))
            {


                dmgList.Clear();

                dmgList = calculateDmgs(eneList);

            }
            //animation = new AnimatedSprite(Content.Load<Texture2D>(curSkill.name), curSkill.rows, curSkill.cols, curSkill.spriteDelay, curSkill.totalFrames);
            curUnit.delay += curSkill.delayAmnt;
            animator.step = 0;
            animator.randInts.Clear();
            animations = new List<AnimatedSprite>();

            //animates all animations that the skill defines.  A little unwieldy but whatevz
            for (int i = 0; i < curSkill.animations.Count; i++)
            {
                animations.Add(new AnimatedSprite(Content.Load<Texture2D>(curSkill.animations[i].name), curSkill.animations[i].rows, curSkill.animations[i].cols, curSkill.animations[i].spriteDelay, curSkill.animations[i].rev));
            }
            phase = Phase.Animation;
        }

        protected void Advance(Unit un)
        {
            if (getDistance(un) != 1) setDistance(un, getDistance(un) - 1);
        }
        protected void Retreat(Unit un)
        {
            if (getDistance(un) != 5) setDistance(un, getDistance(un) + 1);
        }
        protected void AllAdvance()
        {
            foreach (Unit un in eneList)
            {
                Advance(un);
            }
        }
        protected void AllRetreat()
        {
            foreach (Unit un in eneList)
            {
                Retreat(un);
            }
        }
        protected void MonAllAdv()
        {
            AllAdvance();
            ShowInfo(curUnit.name + " advanced toward all units.");
            curUnit.delay += 50;
        }
        protected void MonAllRet()
        {
            AllRetreat();
            ShowInfo(curUnit.name + " retreats from all enemy units.");
            curUnit.delay += 50;
        }
        protected void MonExecuteSkill(Skill ski)
        {
            ShowInfo(curUnit.name + " used " + ski.name);
            curSkill = ski;
            ExecuteSkill();
        }

        protected void MonsterAI()
        {
            int numClose = 0;
            int numMid = 0;
            int numFar = 0;
            for (int i = 0; i < eneList.Count; i++)
            {
                if (getDistance(eneList[i]) >= 4) numFar++;
                else if (getDistance(eneList[i]) == 3) numMid++;
                else if (getDistance(eneList[i]) >= 1) numClose++;
            }
            switch (curUnit.type)
            {
                case "Buns":
                    {
                        if ((numClose <= 1)||(numMid>=3))
                        {
                            MonAllAdv();
                        }
                        else if (RNG.RandInt(1, 100) <= 90)
                        {
                            MonExecuteSkill(curUnit.skillList[0]);
                        }
                        else
                        {
                            ShowInfo(curUnit.name + " did nothing.");
                            curUnit.delay += 25;
                        }
                    }
                    break;
                case "Rafflesia":
                    {
                        if (numFar <= 1)
                        {
                            MonAllRet();
                        }
                        else if (RNG.RandInt(1, 100) <= 80)
                        {
                            MonExecuteSkill(curUnit.skillList[0]);
                        }
                        else
                        {
                            ShowInfo(curUnit.name + " did nothing.");
                            curUnit.delay += 25;
                        }
                    }
                    break;
                case "Slime":
                    {
                        if (numFar >= 4)
                        {
                            MonAllAdv();
                        }
                        else 
                        {
                            MonExecuteSkill(curUnit.skillList[0]);
                        }
                        
                    }
                    break;


            }
        }

        protected void LevelUp(Char cha)
        {
            int oldStr = cha.str;
            int oldMag = cha.mag;
            int oldDef = cha.def;
            int oldFth = cha.fth;
            int oldSpd = cha.spd;
            int oldLuk = cha.luck;
            int oldSki = cha.ski;
            int oldEva = cha.eva;
            int oldHP = cha.maxHP;
            int oldMP = cha.maxMP;
            int oldRec = cha.rec;

            Console.WriteLine(cha);
            
            for (int i = 0; i <= 2; i++)
            {
                if (RNG.RandInt(1,100) <= cha.probStatGain[0]) cha.str ++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[1]) cha.mag++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[2]) cha.def++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[3]) cha.fth++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[4]) cha.ski++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[5]) cha.eva++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[6]) cha.luck++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[7]) cha.spd++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[8]) cha.rec++;
                if (RNG.RandInt(1, 100) <= cha.probStatGain[9]) cha.maxHP += RNG.RandInt(1,4);
                if (RNG.RandInt(1, 100) <= cha.probStatGain[10]) cha.maxMP += RNG.RandInt(1,2);
            }
            cha.level++;
            Console.WriteLine(cha);
            
            ShowInfo("Level up!");
            //Show a bunch of the level up stuff here, maybe put in like a whole small phase for it. Players should
            //feel good for leveling up
        }
    }
}
