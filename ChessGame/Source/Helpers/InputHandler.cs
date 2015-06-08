using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace ChessGame
{
    /// <summary>
    /// Handles inputs for both windows and mobile.
    /// </summary>
    static class InputHandler
    {
        static public Vector2 Position { get { return position / Resolution.Scale; } }
        static public Boolean Released { get; private set; }
        static public Boolean Pressed { get; private set; }
        static private Vector2 position;
#if WINDOWS
        static private MouseState PreviousMouseState;
        static private KeyboardState PreviousKeyboardState;
#endif


        /// <summary>
        /// Updates the inputs.
        /// </summary>
        static public void Update(Game game, GraphicsDeviceManager graphics)
        {
            // Reset old states
            Pressed = false;
            Released = false;

#if __ANDROID__
            //Touch position
            foreach (TouchLocation touchLocation in TouchPanel.GetState())
            {
                if (touchLocation.State == TouchLocationState.Moved)
                    position = touchLocation.Position;
                else if (touchLocation.State == TouchLocationState.Pressed)
                {
                    position = touchLocation.Position;
                    Pressed = true;
                }
                else if (touchLocation.State == TouchLocationState.Released)
                {
                    position = touchLocation.Position;
                    Released = true;
                }
            }
#endif
#if WINDOWS
            // Get new states
            KeyboardState currentKeyboardState = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();

            // Position
            position = new Vector2(currentMouseState.X, currentMouseState.Y);
            // Pressed
            if (currentMouseState.LeftButton == ButtonState.Pressed)
                Pressed = true;
            // Released
            else if (PreviousMouseState.LeftButton == ButtonState.Pressed)
                Released = true;
            // Esc - exit game
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                game.Exit();
            // F11 - fullscreen game
            if (PreviousKeyboardState.IsKeyDown(Keys.F11) && !currentKeyboardState.IsKeyDown(Keys.F11))
            {
                if (game.Window.IsBorderless == true)
                {
                    game.Window.Position = new Point((Resolution.ScreenWidth - Resolution.GameWidth) / 2, ((Resolution.ScreenHeight - Resolution.GameHeight) / 2) - 40);
                    game.Window.IsBorderless = false;
                    graphics.PreferredBackBufferWidth = Resolution.GameWidth;
                    graphics.PreferredBackBufferHeight = Resolution.GameHeight;
                    graphics.ApplyChanges();
                }
                else
                {
                    game.Window.Position = new Point(0, 0);
                    game.Window.IsBorderless = true;
                    graphics.PreferredBackBufferWidth = Resolution.ScreenWidth;
                    graphics.PreferredBackBufferHeight = Resolution.ScreenHeight;
                    graphics.ApplyChanges();
                }
            }

            PreviousMouseState = currentMouseState;
            PreviousKeyboardState = currentKeyboardState;
#endif
        }
    }
}
