using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace Final_Game
{
    class GhostMovement
    {
        Texture2D spriteTexture;
        float timer = 0f;
        float interval = 100f;
        int currentFrame = 0;
        int spriteWidth = 14;
        int rowHeight = 65;//pacman 13x13, ghosts 14x14
        int spriteHeight = 14;
        int speed = 1;
        Rectangle sourceRect;
        Vector2 position;
        Vector2 origin;

        //fields

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

        public GhostMovement(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
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

            if (currentKeys.GetPressedKeys().Length == 0)
            {

                if (currentFrame > 0 && currentFrame < 3)
                {
                    currentFrame = 1;
                }

            }
                interval = 100;

            if (currentKeys.IsKeyDown(Keys.Right) == true)
            {
                AnimateRight(gameTime);
                rowHeight = 0;
                position.X-=speed;
            }

            else if (currentKeys.IsKeyDown(Keys.Left) == true)
            {
                AnimateLeft(gameTime);
                position.X+=speed;
            }

            else if (currentKeys.IsKeyDown(Keys.Down) == true)
            {
                AnimateDown(gameTime);
                position.Y-=speed;
            }

            else if (currentKeys.IsKeyDown(Keys.Up) == true)
            {
                AnimateUp(gameTime);
                position.Y+=speed;
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
                currentFrame = 4;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 5)
                {
                    currentFrame = 4;
                }

                timer = 0f;

            }
        }

        public void AnimateDown(GameTime gameTime)
        {
            if (currentKeys != previousKeys)
            {
                currentFrame = 6;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 7)
                {
                    currentFrame = 6;
                }

                timer = 0f;

            }

        }

        public void AnimateLeft(GameTime gameTime)
        {

            if (currentKeys != previousKeys)
            {
                currentFrame = 2;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 3)
                {
                    currentFrame = 2;
                }

                timer = 0f;

            }

        }

    }
}



