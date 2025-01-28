using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace Final_Game
{
    public class ActionScene : GameScene
    {
        private SpriteBatch sb;

        // Declare Textures
        private Texture2D ghostTexture;
        private Texture2D backgroundTexture;
        private Texture2D gameOverTexture;
        private Texture2D bat;

        private List<Ghost> ghosts; // Declare Ghost list

        //declare type of particles
        private List<Particle> clickParticles;
        private List<Particle> missParticles;
        public Song actionMusic;
        private float spawnTimer; // time between each ghost spawn
        private int score; // total ghosts clicked
        private HighScoreManager highScoreManager; // declare an instance of highScoreManager
        private float gameTimeElapsed;
        private int missedCount; // To track missed ghosts
        public bool gameOver; // To track whether the game is over
        private ButtonState previousMouseState = ButtonState.Released;

        // sfx
        private SoundEffect missSound;
        private SoundEffect clickSound;


        public ActionScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            sb = g._spriteBatch;

            // load content
            backgroundTexture = game.Content.Load<Texture2D>("images/actionBackground");
            ghostTexture = game.Content.Load<Texture2D>("images/ghost");
            gameOverTexture = game.Content.Load<Texture2D>("images/gameover");
            bat = game.Content.Load<Texture2D>("images/bat");
            missSound = game.Content.Load<SoundEffect>("sounds/miss");
            clickSound = game.Content.Load<SoundEffect>("sounds/poof");

            //initialize lists
            ghosts = new List<Ghost>();
            clickParticles = new List<Particle>();
            missParticles = new List<Particle>();
            score = 0;// initialize score
            highScoreManager = new HighScoreManager(); // new highscore manager instance
            spawnTimer = 2f; // Time between ghost spawns
            missedCount = 0; // Start with no missed ghosts
            gameOver = false; // game over flag
        }

        public override void Update(GameTime gameTime)
        {

            if (gameOver)
            {
                UpdateParticles(gameTime, missParticles);
                if (!highScoreManager.isHighScoreAdded)
                {
                    highScoreManager.AddHighScore(score);  // Add the score to the high score manager
                }
                return; // Stop updating if the game is over
            }
            base.Update(gameTime);

            // Update elapsed game time
            gameTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Manage ghost spawn timing
            spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnTimer <= 0f)
            {
                // Spawn a new random ghost
                Random rand = new Random();
                int radius = rand.Next(30, 50); // Random radius between 30 and 50
                Vector2 position = new Vector2(rand.Next(100, (int)(Shared.stage.X - 100)), rand.Next(100, (int)(Shared.stage.Y - 100))); // Random position
                ghosts.Add(new Ghost(position, radius));// add new ghost with position and radius to ghosts list
                spawnTimer = MathHelper.Clamp(2f - (gameTimeElapsed / 30f), 0.5f, 2f); // Cap spawnTimer to a minimum of 0.5f
            }

            foreach (Ghost ghost in ghosts.ToArray())
            {
                // Timer decrease and removal check based on the capped value
                if (ghost.Timer <= 0.6f && !ghost.IsClicked)
                {
                    missedCount++; // Increment missed count
                    missSound.Play();
                    TriggerExplosion(ghost.Position, missParticles);//trigger explosion animation with miss particles
                    ghosts.Remove(ghost); // Remove the ghost
                }
            }

            // Check for mouse click on each ghost
            MouseState mouseState = Mouse.GetState();
            foreach (Ghost ghost in ghosts)
            {

                if (ghost.IsMouseOver(mouseState) && !ghost.IsClicked)
                {
                    // Check for the transition from Released to Pressed
                    if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState == ButtonState.Released)
                    {
                        ghost.IsClicked = true;
                        clickSound.Play();
                        score++; // Increment score when a ghost is clicked
                        TriggerExplosion(ghost.Position, clickParticles);// Trigger explosion animation when clicked with click particle
                        ghosts.Remove(ghost);
                        break; // Stop checking further ghosts after the first click
                    }
                }
                // if ghost is missed decrement score to avoid spam clicking
                else if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState == ButtonState.Released)
                {
                    score--;
                }
            }

            // Check if game is over
            if (missedCount >= 3)
            {

                gameOver = true; // End the game when 3 ghosts are missed

            }

            previousMouseState = mouseState.LeftButton;
            // Update all remaining ghosts
            foreach (Ghost ghost in ghosts)
            {
                ghost.Update(gameTime);
            }

            // Update particles for animation of each event
            UpdateParticles(gameTime, missParticles);
            UpdateParticles(gameTime, clickParticles);

        }
        /// <summary>
        /// trigger animation at given position
        /// </summary>
        /// <param name="position"> position where the explosion takes place</param>
        /// <param name="particles"> type of particles used in animation </param>
        private void TriggerExplosion(Vector2 position, List<Particle> particles)
        {
            Random rand = new Random();
            int particleCount = 50; // Number of particles to generate

            for (int i = 0; i < particleCount; i++)
            {
                particles.Add(new Particle
                {
                    Position = position,
                    Velocity = new Vector2(
                        (float)(rand.NextDouble() * 2 - 1) * 200,
                        (float)(rand.NextDouble() * 2 - 1) * 200
                    ),
                    Color = Color.White,
                    LifeTime = 1.0f // Particle will exist for 1 second
                });
            }
        }
        /// <summary>
        /// update particles with time
        /// </summary>
        /// <param name="gameTime">variavble to manage timing of game</param>
        /// <param name="particles"> type of particle to be used in explosion</param>
        private void UpdateParticles(GameTime gameTime, List<Particle> particles)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = particles.Count - 1; i >= 0; i--)
            {
                // if particles lifetime decrease to 0 remove it
                particles[i].LifeTime -= deltaTime;
                if (particles[i].LifeTime <= 0)
                {
                    particles.RemoveAt(i);
                    continue;
                }

                particles[i].Position += particles[i].Velocity * deltaTime; // particles move away from center with deltatime
                particles[i].Color *= 0.95f; // Fade out over time
            }
        }

        /// <summary>
        /// draw particles when event occurs
        /// </summary>
        /// <param name="tex"> texture of particle</param>
        /// <param name="particles">type of particle for event</param>
        private void DrawParticles(Texture2D tex, List<Particle> particles)
        {
            foreach (var particle in particles)
            {
                
                sb.Draw(tex, particle.Position, null, particle.Color, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 0);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            
            Viewport viewport = GraphicsDevice.Viewport;

            //draw background for scene
            sb.Draw(backgroundTexture,
                    new Rectangle(0, 0, viewport.Width, viewport.Height),
                    Color.White);

            // Draw ghosts
            foreach (Ghost ghost in ghosts)
            {
                sb.Draw(ghostTexture, new Rectangle((int)ghost.Position.X - ghost.Radius, (int)ghost.Position.Y - ghost.Radius, ghost.Radius * 2, ghost.Radius * 2), Color.White);
            }

            // Odraw score and missed count
            SpriteFont font = Game.Content.Load<SpriteFont>("fonts/ScoreFont");
            SpriteFont highlight = Game.Content.Load<SpriteFont>("fonts/HilightFont");


            if (!gameOver)
            {
                sb.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);
                sb.DrawString(font, "Missed: " + missedCount, new Vector2(10, 40), Color.White);

            }

            // If the game is over, show "Game Over" screen with score and instructions
            if (gameOver)
            {
                sb.Draw(gameOverTexture, new Rectangle(0, 0, viewport.Width, viewport.Height),
                    Color.White);
                sb.DrawString(highlight, "Score: " + score, new Vector2((viewport.Width / 2) - 50, viewport.Height / 2), Color.White);
                sb.DrawString(font, "Press Enter to Go to Main Menu", new Vector2(Shared.stage.X / 2 - 93, Shared.stage.Y - 70), Color.White);
                sb.DrawString(font, "Press Escape to Exit Game", new Vector2(Shared.stage.X / 2 - 80, Shared.stage.Y - 110), Color.White);
            }
            // Draw particles for each type of animation
            DrawParticles(ghostTexture, missParticles);//ghost miss animation
            DrawParticles(bat, clickParticles);// ghost click animation

            sb.End();

            base.Draw(gameTime);
        }
    }
}
