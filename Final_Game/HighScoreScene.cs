using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Final_Game
{
    public class HighScoreScene : GameScene
    {
        private HighScoreManager highScoreManager;
        private SpriteBatch sb;
        private SpriteFont font;
        private Texture2D title;

        public HighScoreScene(Game game) : base(game)
        {
            sb = ((Game1)game)._spriteBatch;
            highScoreManager = new HighScoreManager();
            

        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
            // initialize array highScores using highScoreManager
            string[] highScores = highScoreManager.LoadHighScores();
            
            sb.Begin();

            // load font and title
            font = Game.Content.Load<SpriteFont>("fonts/ScoreFont");
            title = Game.Content.Load<Texture2D>("images/highscoretitle");

            // Draw title for high scores
            sb.Draw(title, new Rectangle(190, 10 ,400, 400),
                    Color.White);

            // Draw the high scores list
            for (int i = 0; i < highScores.Length; i++)
            {
                sb.DrawString(font, $"{i + 1}. {highScores[i]}", new Vector2(Shared.stage.X / 2 - 60, 150 + (i * 30)), Color.White);
            }

            // instruction to navigate back to Main Menu
            sb.DrawString(font, "Press Escape to Go Back", new Vector2(Shared.stage.X / 2 - 100, Shared.stage.Y - 60), Color.White);

            sb.End();
        }
    }
}
