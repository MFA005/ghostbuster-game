using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace Final_Game
{
    public class StartScene : GameScene
    {

        private Texture2D tex;
        private SpriteBatch sb;
        private MenuComponent menu;
        private Texture2D titleTex;
        

        public MenuComponent Menu { get => menu; set => menu = value; }

        public StartScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            sb = g._spriteBatch;

            // initialize menu items array
            string[] menuItems = { "Start game", "Help", "High Score", "Credits", "Quit" };

            // load startscene textures
            tex = game.Content.Load<Texture2D>("images/actionBackground");
            titleTex = game.Content.Load<Texture2D>("images/title");

            // load fonts
            SpriteFont regularFont = game.Content.Load<SpriteFont>("fonts/RegularFont");
            SpriteFont hilightFont = g.Content.Load<SpriteFont>("fonts/HilightFont");

            // initialize menu
            Menu = new MenuComponent(game, g._spriteBatch, regularFont, hilightFont, menuItems);
            Components.Add(Menu);
        }
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            Viewport viewport = GraphicsDevice.Viewport;

            // draw background
            sb.Draw(tex,
                    new Rectangle(0, 0, viewport.Width, viewport.Height),
                    Color.White);

            // draw title
            sb.Draw(titleTex, new Rectangle(0, 0, viewport.Width, viewport.Height),
                    Color.White);

            sb.End();

            base.Draw(gameTime);
        }
    }
}
