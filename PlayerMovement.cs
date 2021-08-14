using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace Final_Game
{
    class PlayerMovement
    {
        Texture2D spriteTexture;
        float timer = 0f;
        float interval = 100f;
        int currentFrame = 1;
        int spriteWidth = 13;
        int rowHeight, tick = 0;//pacman 13x13, ghosts 14x14
        int spriteHeight = 13;
        Vector2 position;
        Rectangle sourceRect;
        Vector2 origin;

        //

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Origin
        {
            get { return position; }
            set { position = value; }
        }
        public Texture2D Texture
        {
            get { return spriteTexture; }
            set { spriteTexture = value; }
        }
        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public PlayerMovement(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
        {
            this.spriteTexture = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
        }

        KeyboardState currentKeys;
        KeyboardState previousKeys;

        public void HandleSpriteMovement(GameTime gameTime)
        {
            previousKeys = currentKeys;
            currentKeys = Keyboard.GetState();
            sourceRect = new Rectangle(currentFrame * spriteWidth, rowHeight, spriteWidth, spriteHeight);
            interval = 50;
            if (currentKeys.GetPressedKeys().Length == 0)
            {
                currentFrame = 2;
                rowHeight = 0;
            }
            

            if (currentKeys.IsKeyDown(Keys.Right) == true)
            {
                AnimateRight(gameTime);
                rowHeight = 0;
            }

            else if (currentKeys.IsKeyDown(Keys.Left) == true)
            {
                AnimateLeft(gameTime);
                rowHeight = 13;
            }

            else if (currentKeys.IsKeyDown(Keys.Down) == true)
            {
                AnimateDown(gameTime);
                rowHeight = 39;
            }

            else if (currentKeys.IsKeyDown(Keys.Up) == true)
            {
                AnimateUp(gameTime);
                rowHeight = 26;
            }
            if (currentKeys.IsKeyDown(Keys.Space))
            {
                DyingAnimate(gameTime);
                timer = 0;
            }
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                
        }

        public void AnimateRight(GameTime gameTime)
        {

            if (currentKeys != previousKeys)
            {
                currentFrame = 1;
            }


            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 2)
                {
                    currentFrame = 0;
                }

                timer = 0f;
            }

        }

        public void AnimateUp(GameTime gameTime)
        {
            if (currentKeys != previousKeys)
            {
                currentFrame = 1;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 2)
                {
                    currentFrame = 0;
                }

                timer = 0f;

            }
        }

        public void AnimateDown(GameTime gameTime)
        {
            if (currentKeys != previousKeys)
            {
                currentFrame = 1;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 2)
                {
                    currentFrame = 0;
                }

                timer = 0f;

            }

        }

        public void AnimateLeft(GameTime gameTime)
        {

            if (currentKeys != previousKeys)
            {
                currentFrame = 1;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 2)
                {
                    currentFrame = 0;
                }

                timer = 0f;

            }

        }
        public void DyingAnimate(GameTime gameTime)
        {
            tick++;
            if (tick % 5 == 0)
            {
                currentFrame++;
            }
        }

    }
}







