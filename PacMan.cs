
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Final_Game
{
    //Display all sprites (done 5/24/19)
    //Sound effects (intro done 5/9, ghost done 5/10)
    //Make pacman move (done 5/2/19)
    //make pacman move off screen onto other side (done 5/7/19)
    //Generate pellets (done 5/12/19)
    //make pellets disappear (done 5/17/19)
    //pellet sound effects (done 5/19/19)
    //add pellet points (done 5/23/19)
    //Collision detection (stressed over for like 3 weeks)
    //-pellet collision (5/23/19)
    //draw rectangles for background collision (done 5/31/19)
    //dying animation w/ space bar & sound (done 5/24/19)
    //use all gamestates (done 6/2/19)
    //replay after game over (6/3/19)
    //Ghost AI (myeahhhhh thats a no from me)
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            Begin,
            LevelStart,
            Playing,
            Success,
            RoundOver,
            GameOver,
        }

        GameState gameState;
        Texture2D background, spritesheet, pixel;
        List<Rectangle> pelletRects;
        List<Rectangle> backRects;
        Rectangle backRect, pacmanRect; //hitboxes

        SoundEffectInstance ghostInstance, pacDeadInstance, pacEatInstance;
        SoundEffect welcomeMusic, ghost, eat, dead;

        SpriteFont gameFont;
        PlayerMovement playerMove;
        KeyboardState keys, oldKeys = new KeyboardState();
        Vector2 position = new Vector2(208, 385);
        Vector2 startPellet = new Vector2(36, 70);
        Color pacColor = Color.White;
        bool notStarted, levelNotStarted = true;
        int tick, pelletsLeft, score, level = 0;
        int lives = 3;
        bool death, collision = false;


        #region Pellet Collision
        private bool PelletCollision(Texture2D spritesheet, Texture2D pixel, Rectangle pacmanRect, Rectangle rect)
        {
            Color[] colorData1 = new Color[spritesheet.Width * spritesheet.Height];
            Color[] colorData2 = new Color[pixel.Width * pixel.Height];
            spritesheet.GetData<Color>(colorData1);
            pixel.GetData<Color>(colorData2);

            int top, bottom, left, right;

            top = Math.Max(pacmanRect.Top, rect.Top);
            bottom = Math.Min(pacmanRect.Bottom, rect.Bottom);
            left = Math.Max(pacmanRect.Left, rect.Left);
            right = Math.Min(pacmanRect.Right, rect.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color A = colorData1[(y - pacmanRect.Top) * (pacmanRect.Width) + (x - pacmanRect.Left)];
                    Color B = colorData2[(y - rect.Top) * (rect.Width) + (x - rect.Left)];

                    if (A.A != 0 && B.A != 0)
                        return true;
                }
            }
            return false;
        }
        #endregion
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 552;
            graphics.PreferredBackBufferWidth = 428;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameFont = Content.Load<SpriteFont>("gameFont");
            gameState = GameState.Begin;
            background = Content.Load<Texture2D>("background");
            pixel = Content.Load<Texture2D>("Pixel");
            backRect = new Rectangle(0, 40, 428, 476); //allows offset of 40px for score space and lives
            pelletRects = new List<Rectangle>();
            backRects = new List<Rectangle>();
            notStarted = true;
            #region background rectangles
            backRects.Add(new Rectangle(1, 55, 428, 10));//bck
            backRects.Add(new Rectangle(18, 55, 10, 188));//bck
            backRects.Add(new Rectangle(400, 55, 10, 188));//bck
            backRects.Add(new Rectangle(400, 266, 10, 230));//bck
            backRects.Add(new Rectangle(18, 266, 10, 230));//bck
            backRects.Add(new Rectangle(1, 492, 428, 10));//bck
            backRects.Add(new Rectangle(43, 82, 48, 26));//c1r1
            backRects.Add(new Rectangle(43, 128, 48, 26));//c1r2
            backRects.Add(new Rectangle(0, 174, 89, 69));//c1r3
            backRects.Add(new Rectangle(0, 266, 89, 69));//c1r4
            backRects.Add(new Rectangle(43, 355, 48, 26));//c1r5
            backRects.Add(new Rectangle(24, 402, 22, 23));//c1r5.5
            backRects.Add(new Rectangle(66, 369, 25, 56));//c1r6
            backRects.Add(new Rectangle(45, 448, 136, 23));//c1r7
            backRects.Add(new Rectangle(113, 82, 68, 26));//c2r1
            backRects.Add(new Rectangle(113, 128, 24, 116));//c2r2
            backRects.Add(new Rectangle(113, 266, 24, 69));//c2r3
            backRects.Add(new Rectangle(113, 174, 68, 26));//c2r4
            backRects.Add(new Rectangle(113, 355, 68, 26));//c2r5
            backRects.Add(new Rectangle(113, 402, 24, 46));//c2r6
            backRects.Add(new Rectangle(202, 62, 24, 46));//c3r1
            backRects.Add(new Rectangle(157, 131, 113, 21));//c3r2
            backRects.Add(new Rectangle(202, 145, 24, 54));//c3r3
            backRects.Add(new Rectangle(157, 312, 113, 21));//c3r4
            backRects.Add(new Rectangle(202, 327, 24, 54));//c3r5
            backRects.Add(new Rectangle(157, 403, 113, 21));//c3r6
            backRects.Add(new Rectangle(202, 417, 24, 54));//c3r7
            backRects.Add(new Rectangle(292, 131, 24, 116));//c4r1
            backRects.Add(new Rectangle(248, 174, 68, 26));//c4r2
            backRects.Add(new Rectangle(292, 266, 24, 69));//c4r3
            backRects.Add(new Rectangle(248, 355, 68, 26));//c4r4
            backRects.Add(new Rectangle(248, 82, 68, 26));//c4r0 oops
            backRects.Add(new Rectangle(292, 402, 24, 46));//c4r5
            backRects.Add(new Rectangle(248, 448, 136, 23));//c4r6
            backRects.Add(new Rectangle(338, 82, 48, 26));//c5r1
            backRects.Add(new Rectangle(338, 128, 48, 26));//c5r2
            backRects.Add(new Rectangle(338, 174, 89, 69));//c5r3
            backRects.Add(new Rectangle(338, 266, 89, 69));//c5r4
            backRects.Add(new Rectangle(338, 355, 48, 26));//c5r5
            backRects.Add(new Rectangle(382, 402, 22, 23));//c5r6
            backRects.Add(new Rectangle(338, 369, 25, 56));//c5r4

            #endregion

            //creates 19 rows of 17 pellets
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    pelletRects.Add(new Rectangle((int)startPellet.X + 22 * j, (int)startPellet.Y + 23 * i, 3, 3));
                }
            }

            spritesheet = Content.Load<Texture2D>("sprite sheet");
            playerMove = new PlayerMovement(spritesheet, 1, 13, 13);
            welcomeMusic = Content.Load<SoundEffect>("pacbegin");
            dead = Content.Load<SoundEffect>("pacdead");
            ghost = Content.Load<SoundEffect>("pacghost");
            eat = Content.Load<SoundEffect>("paceat");
            ghostInstance = ghost.CreateInstance();
            pacDeadInstance = dead.CreateInstance();
            pacEatInstance = eat.CreateInstance();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            keys = Keyboard.GetState();

            pacmanRect = new Rectangle((int)position.X, (int)position.Y, 13, 13);

            switch (gameState)
            {
                case GameState.Begin: //starts game, NOT LEVEL
                    if (notStarted == true) // determines if starting tune is played or not
                    {
                        welcomeMusic.Play();
                        notStarted = false;
                    }
                    tick++;
                    if (tick > 240)
                    {
                        gameState = GameState.Playing;
                        levelNotStarted = false;
                        tick = 0;
                    }

                    break;
                case GameState.Playing:
                    playerMove.HandleSpriteMovement(gameTime);
                    if (pelletRects.Count() == 140) //checks if all aisle pellets are eaten
                    {
                        gameState = GameState.Success;
                        levelNotStarted = true;
                    }

                    #region collision
                    for (int i = 0; i < pelletRects.Count(); i++)//checks pellet collision & removes as needed
                    {
                        if (pacmanRect.Intersects(pelletRects[i]))
                        {
                            pelletRects.RemoveAt(i);
                            pelletsLeft--;
                            score += 10;
                            i--;
                            pacEatInstance.Play();
                        }
                    }
                    for (int i = 0; i > backRects.Count(); i++)
                    {
                        if (pacmanRect.Intersects(backRects[i]))
                        {
                            collision = true;
                        }
                        else
                            collision = false;
                    }
                    #endregion
                    #region movement
                    if (!collision)
                    {
                        if (keys.IsKeyDown(Keys.Right))
                        {
                            position.X++;
                            if (position.Y >= 243 && position.Y <= 252)
                            {
                                if (position.X > 428 + 13)
                                {
                                    position.X = -13;
                                }
                                if (position.X < -13)
                                {
                                    position.X = 428 + 13;
                                }
                            }
                        }
                        else if (keys.IsKeyDown(Keys.Left))
                        {
                            position.X--;
                            if (position.Y >= 243 && position.Y <= 252)
                            {
                                if (position.X > 428 + 13)
                                {
                                    position.X = -13;
                                }
                                if (position.X < -13)
                                {
                                    position.X = 428 + 13;
                                }
                            }
                        }

                        else if (keys.IsKeyDown(Keys.Up))
                        {
                            position.Y--;
                        }
                        else if (keys.IsKeyDown(Keys.Down))
                        {
                            position.Y++;
                        }

                    }

                    if (keys.IsKeyDown(Keys.Space))
                    {
                        death = true;
                        tick++;
                        if (pacDeadInstance.State == SoundState.Stopped)
                        {
                            pacDeadInstance.Play();
                        }
                        if (tick >= 78)
                        {
                            gameState = GameState.RoundOver;
                        }
                        
                    }
                    #endregion


                    //loops ghost music        
                    if (ghostInstance.State == SoundState.Stopped && !death)
                    {
                        ghostInstance.Play();
                    }
                    break;
                case GameState.Success:
                    tick++;
                    if (tick > 120)
                    {
                        if (levelNotStarted) //executes 1 bout to generate pellets and reposition for next level
                        {
                            for (int i = 0; i < 19; i++)
                            {
                                for (int j = 0; j < 17; j++)
                                {
                                    pelletRects.Add(new Rectangle((int)startPellet.X + 22 * j, (int)startPellet.Y + 23 * i, 3, 3));
                                }
                            }
                            levelNotStarted = false;
                            level++;
                        }
                        position = new Vector2(208, 385);
                        gameState = GameState.LevelStart;
                        tick = 0;
                    }
                    break;
                case GameState.LevelStart://alows for delay before level start
                    tick++;
                    if (tick > 60)
                    {
                        gameState = GameState.Playing;
                        tick = 0;
                    }
                    break;

                case GameState.RoundOver:
                    if (lives <= 3 && lives > 0)
                    {
                        lives--;
                        gameState = GameState.LevelStart;
                        tick = 0;
                    }
                    else if (lives == 0)
                    {
                        gameState = GameState.GameOver;
                    }
                    break;

                case GameState.GameOver:
                    if (keys.IsKeyDown(Keys.Y))
                    {
                        gameState = GameState.Begin;
                        notStarted = true;
                        levelNotStarted = true;
                        lives = 3;
                        tick = 0;
                        score = 0;
                    }
                    break;
            }


            keys = oldKeys;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            switch (gameState) //draws pacman placeholder during intro sequence
            {
                case GameState.Begin:
                    spriteBatch.Draw(spritesheet, new Vector2((int)position.X, (int)position.Y), new Rectangle(26, 0, 13, 13), Color.White);
                    spriteBatch.DrawString(gameFont, "PLAYER ONE", new Vector2(160, 198), Color.Cyan);
                    spriteBatch.DrawString(gameFont, "READY!", new Vector2(183, 288), Color.Yellow);
                    break;
                case GameState.LevelStart:
                    spriteBatch.Draw(spritesheet, new Vector2((int)position.X, (int)position.Y), new Rectangle(26, 0, 13, 13), Color.White);
                    spriteBatch.DrawString(gameFont, "READY!", new Vector2(183, 288), Color.Yellow);
                    break;
                case GameState.GameOver:
                    spriteBatch.DrawString(gameFont, "GAME OVER", new Vector2(165, 270), Color.White);
                    spriteBatch.DrawString(gameFont, "SCORE: " + score.ToString(), new Vector2(160, 290), Color.White);
                    spriteBatch.DrawString(gameFont, "PLAY AGAIN? Y/N", new Vector2(135, 310), Color.White);
                    break;
            }
            if (!(gameState == GameState.GameOver))
            {
                foreach (Rectangle rect in pelletRects)
                {
                    spriteBatch.Draw(pixel, rect, Color.White);
                }
                spriteBatch.Draw(background, backRect, Color.White);
                spriteBatch.DrawString(gameFont, "SCORE: " + score.ToString(), new Vector2(3, 10), Color.White);
                spriteBatch.DrawString(gameFont, "LEVEL " + level.ToString(), new Vector2(350, 525), Color.Yellow);
                spriteBatch.Draw(playerMove.Texture, position, playerMove.SourceRect, pacColor, 0f, playerMove.Origin, 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(pixel, new Rectangle(204, 226, 20, 8), Color.HotPink);

                //lives display
                if (lives == 3)
                {
                    spriteBatch.Draw(playerMove.Texture, new Vector2(10, 525), new Rectangle(13, 0, 13, 13), Color.White);
                    spriteBatch.Draw(playerMove.Texture, new Vector2(36, 525), new Rectangle(13, 0, 13, 13), Color.White);
                    spriteBatch.Draw(playerMove.Texture, new Vector2(62, 525), new Rectangle(13, 0, 13, 13), Color.White);
                }
                else if (lives == 2)
                {
                    spriteBatch.Draw(playerMove.Texture, new Vector2(10, 525), new Rectangle(13, 0, 13, 13), Color.White);
                    spriteBatch.Draw(playerMove.Texture, new Vector2(36, 525), new Rectangle(13, 0, 13, 13), Color.White);
                }
                else if (lives == 1)
                {
                    spriteBatch.Draw(playerMove.Texture, new Vector2(10, 525), new Rectangle(13, 0, 13, 13), Color.White);
                }

                //ghosts
                spriteBatch.Draw(spritesheet, new Vector2(188, 247), new Rectangle(56, 93, 14, 14), Color.White);//teal
                spriteBatch.Draw(spritesheet, new Vector2(206, 247), new Rectangle(84, 79, 14, 14), Color.White);//pink
                spriteBatch.Draw(spritesheet, new Vector2(224, 247), new Rectangle(56, 107, 14, 14), Color.White);

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}




