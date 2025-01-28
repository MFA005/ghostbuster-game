using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Final_Game
{
    public class Ghost
    {
        public Vector2 Position { get; set; } // Position of the ghost 
        public int Radius { get; set; } // Size of the ghost
        public float Timer { get; set; }  // Timer to control how long the ghost stays on the screen
        public bool IsClicked { get; set; }  // Track if the ghost is clicked

        public Ghost(Vector2 position, int radius)
        {
            //set attributes
            Position = position;
            Radius = radius;
            Timer = 1.75f;  // Each ghost stays on screen for 1.75 seconds by default
            IsClicked = false;
        }

        /// <summary>
        /// Method to check if the mouse is hovering over the ghost 
        /// </summary>
        /// <param name="mouseState"> current mouse state </param>
        /// <returns></returns>
        public bool IsMouseOver(MouseState mouseState)
        {
            float distance = Vector2.Distance(mouseState.Position.ToVector2(), Position);
            return distance <= Radius;
        }

        public void Update(GameTime gameTime)
        {
            // timer gradually decreases as game time elapses and caps at 0.6s if ghost isnt clicked
            if (!IsClicked)
            {
                Timer = MathHelper.Clamp(Timer - (float)gameTime.ElapsedGameTime.TotalSeconds, 0.6f, float.MaxValue);
            }
        }
    }
}

