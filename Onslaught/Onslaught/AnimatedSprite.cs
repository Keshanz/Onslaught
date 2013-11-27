using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Onslaught
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int frameDelay;
        private int frameControl;
        private int direction;
        private Boolean reverse;
        
        //assumes evenly divided spriteframes
        //delay is amount of time each frame is shown
        //reverse indicates whether sprite animation will reverse frames afterward
        public AnimatedSprite(Texture2D texture, int rows, int columns, int delay)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            frameControl = 0;
            frameDelay = delay;
            totalFrames = Rows * Columns;
            reverse = false;
            direction = 1;
        }

        //if total frames is less than rows * columns
        public AnimatedSprite(Texture2D texture, int rows, int columns, int delay, int total)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            frameControl = 0;
            frameDelay = delay;
            totalFrames = total;
            reverse = false;
            direction = 1;
        }

        public AnimatedSprite(Texture2D texture, int rows, int columns, int delay, Boolean rev)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            frameControl = 0;
            frameDelay = delay;
            totalFrames = Rows * Columns;
            reverse = rev;
            direction = 1;
        }

        public void Update()
        {
            
            
            if (frameControl == frameDelay)
            {
                frameControl = 0;
                currentFrame+=direction;
                Console.WriteLine(currentFrame);
                if (currentFrame == totalFrames && !(reverse))
                    currentFrame = 0;
                if (reverse && (currentFrame == totalFrames-1 || currentFrame == 0))
                    direction = -direction;
            }
            frameControl++;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, bool isReflected)
        {
            Update();
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Begin();
            if (isReflected)
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            else
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }


    }
}
