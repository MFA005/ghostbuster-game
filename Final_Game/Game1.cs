using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Reflection.Metadata.Ecma335;

namespace Final_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        
        private KeyboardState previousKeyboardState;

        
        // Declare Scenes
        private StartScene startScene;
        private HelpScene helpScene;
        private CreditsScene creditsScene;
        private ActionScene actionScene;
        private HighScoreScene highScoreScene;

        // Declare music
        private Song backgroundMusic;
        private Song actionMusic;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            //initialize stage
            Shared.stage = new Vector2(_graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //load music
            backgroundMusic = Content.Load<Song>("sounds/backgroundmusic");
            actionMusic = Content.Load<Song>("sounds/actionmusic");

            //initialize scenes
            startScene = new StartScene(this);
            Components.Add(startScene);

            helpScene = new HelpScene(this);
            Components.Add(helpScene);

            creditsScene = new CreditsScene(this);
            Components.Add(creditsScene);

            actionScene = new ActionScene(this);
            Components.Add(actionScene);

            highScoreScene = new HighScoreScene(this);
            Components.Add(highScoreScene);

            // Show start scene and play background music  
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            startScene.show();
        }

        protected override void Update(GameTime gameTime)
        {
            
            KeyboardState currentKeyboardState = Keyboard.GetState();

            
            if (startScene.Enabled)
            {
                // selected index
                int selectedIndex = startScene.Menu.SelectedIndex;

                // if enter is pressed on start scene
                if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    // hide start scene and show scene depending on selected index
                    switch (selectedIndex)
                    {
                        case 0:
                            actionScene = new ActionScene(this);
                            Components.Add(actionScene);
                            startScene.hide();
                            actionScene.show();

                            //play action music and stop menu music
                            MediaPlayer.Stop();
                            MediaPlayer.Play(actionMusic);
                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Volume = 0.1f;
                            break;
                        case 1:
                            startScene.hide();
                            helpScene.show();
                            break;
                        case 2:
                            startScene.hide();
                            highScoreScene.show();
                            break;
                        case 3:
                            startScene.hide();
                            creditsScene.show();
                            break;
                        case 4:
                            Exit();
                            break;
                    }
                }
                
            }
            //if game over on action scene
            if (actionScene.Enabled && actionScene.gameOver)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    actionScene.hide();
                    startScene.show(); // Show start screen when enter is pressed

                    //stop action music and play menu music
                    MediaPlayer.Stop();
                    MediaPlayer.Play(backgroundMusic);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.1f;

                }
                // exit game if escape is pressed
                if (currentKeyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
            }

            // return to menu when escape is pressed
            if (helpScene.Enabled && currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                helpScene.hide();
                startScene.show();

            }
            else if (creditsScene.Enabled && currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                creditsScene.hide();
                startScene.show();
            }
            else if (highScoreScene.Enabled && currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                highScoreScene.hide();
                startScene.show();
            }

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            base.Draw(gameTime);
        }
    }
}
