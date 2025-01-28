using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace Final_Game
{
    public class MenuComponent : DrawableGameComponent
    {
        private SpriteBatch sb;
        private SpriteFont regularFont, hilightFont;

        
        private List<string> menuItems;
        public int SelectedIndex { get; set; }
        private Vector2 position;
        
        //set each font color
        private Color regularColor = Color.White;
        private Color hilightColor = Color.White;

        private SoundEffect click;

        private KeyboardState oldState;

        //menu component constructor
        public MenuComponent(Game game, SpriteBatch sb,
            SpriteFont regularFont, SpriteFont hilightFont,
            string[] menus) : base(game)
        {
            //set attributes
            this.sb = sb;
            this.regularFont = regularFont;
            this.hilightFont = hilightFont;

            //menu navigation sfx
            click = game.Content.Load<SoundEffect>("sounds/click");
            menuItems = menus.ToList();
            position = new Vector2(50, Shared.stage.Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                SelectedIndex++;
                if (SelectedIndex == menuItems.Count)
                {
                    SelectedIndex = 0;
                }
                click.Play();
                

            }

            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = menuItems.Count - 1;
                }
                click.Play();
                
            }



            oldState = ks;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 itemPos = position;
            sb.Begin();

            //draw menu options at itemPos
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (SelectedIndex == i)
                {
                    //highlight if selected
                    sb.DrawString(hilightFont, menuItems[i], itemPos, hilightColor);
                    itemPos.Y += hilightFont.LineSpacing;
                }
                else
                {
                    sb.DrawString(regularFont, menuItems[i], itemPos, regularColor);
                    itemPos.Y += regularFont.LineSpacing;
                }
            }
            sb.End();

            base.Draw(gameTime);
        }
    }
}
