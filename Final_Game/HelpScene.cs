using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Game
{
    public class HelpScene : GameScene
    {
        
        private Texture2D tex;
        private SpriteBatch sb;
        public HelpScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            //load texture
            tex = game.Content.Load<Texture2D>("images/help");
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            // draw texture
            sb.Draw(tex, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            sb.End();

            base.Draw(gameTime);
        }
    }
}