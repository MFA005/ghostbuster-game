using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Game
{
    public abstract class GameScene : DrawableGameComponent
    {
        public List<GameComponent> Components { get; set; }

        /// <summary>
        /// hides scene in view
        /// </summary>
        public virtual void hide()
        {
            Visible = false;
            Enabled = false;
        }
        /// <summary>
        /// shows scene
        /// </summary>
        public virtual void show()
        {
            
            Enabled = true;
            Visible = true;
        }

        protected GameScene(Game game) : base(game)
        {
            //initialize components list
            Components = new List<GameComponent>();
            hide();
        }

        public override void Update(GameTime gameTime)
        {
            //update gamecomponent if its showing
            foreach (GameComponent item in Components)
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //draw each components drawablegamecomponents
            foreach (GameComponent item in Components)
            {
                if (item is DrawableGameComponent)
                {
                    DrawableGameComponent comp = (DrawableGameComponent)item;
                    if (comp.Visible)
                    {
                        comp.Draw(gameTime);
                    }
                }
            }
            base.Draw(gameTime);
        }

    }
}
