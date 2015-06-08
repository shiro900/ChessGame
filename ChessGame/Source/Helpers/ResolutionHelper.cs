using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessGame
{
    /// <summary>
    /// Handles different resolutions and the re-size window event.
    /// </summary>
    static class Resolution
    {
        /// <summary>
        /// Scale is used for scaling the cursor's position depending on the current game's resolution
        /// </summary>
        static public Vector2 Scale { get; private set; }
        /// <summary>
        /// ScaleMatrix is used for scaling the sprites depending on the current game's resolution
        /// </summary>
        static public Matrix ScaleMatrix { get; private set; }
        static public int GameWidth { get; private set; }
        static public int GameHeight { get; private set; }
        static public int ScreenWidth { get; private set; }
        static public int ScreenHeight { get; private set; }
        static private float GameAspectRatio;
#if WINDOWS
        static public Boolean WasResized { private get; set; }
        static private int MinimumGameWidth;
        static private int MinimumGameHeight;
        static private Boolean KeepAspectRatio;
#endif


        /// <summary>
        /// Initialize game resolution.
        /// </summary>
        static public void Initialize(GraphicsDeviceManager graphics)
        {
            GameWidth = 736;
            GameHeight = 736;
            GameAspectRatio = (float)GameWidth / GameHeight;
            CalculateMatrix(graphics);

#if WINDOWS
            // Get screen resolution
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            MinimumGameWidth = GameWidth / 2;
            MinimumGameHeight = GameHeight / 2;
            KeepAspectRatio = false;
            WasResized = false;
#endif
        }

#if WINDOWS
        /// <summary>
        /// Handle window re-size event.
        /// </summary>
        static public void Update(Game game, GraphicsDeviceManager graphics)
        {
            if (WasResized)
            {                
                // If window's width is smaller than minimumWidth
                if (game.Window.ClientBounds.Width < MinimumGameWidth)
                    graphics.PreferredBackBufferWidth = MinimumGameWidth; // set its width to minimumWidth
                else
                    graphics.PreferredBackBufferWidth = game.Window.ClientBounds.Width;

                // If window's height is smaller than minimumheight
                if (game.Window.ClientBounds.Height < MinimumGameHeight)
                    graphics.PreferredBackBufferHeight = MinimumGameHeight; // set its height to minimumHeight
                else
                    graphics.PreferredBackBufferHeight = game.Window.ClientBounds.Height;

                // Keep original aspect ratio, if enabled
                if (KeepAspectRatio)
                {
                    if ((float)graphics.PreferredBackBufferWidth / graphics.PreferredBackBufferHeight > GameAspectRatio)
                        graphics.PreferredBackBufferWidth = (int)(graphics.PreferredBackBufferHeight * (GameAspectRatio));
                    else
                        graphics.PreferredBackBufferHeight = (int)(graphics.PreferredBackBufferWidth * (1 / GameAspectRatio));
                }

                graphics.ApplyChanges();
                CalculateMatrix(graphics);
                WasResized = false;
            }
        }
#endif

        /// <summary>
        /// Calculate the resolution scale.
        /// </summary>
        static void CalculateMatrix(GraphicsDeviceManager graphics)
        {
            ScaleMatrix = Matrix.CreateScale((float)graphics.PreferredBackBufferWidth / GameWidth, (float)graphics.PreferredBackBufferHeight / GameHeight, 1f);
            Scale = new Vector2(ScaleMatrix.M11, ScaleMatrix.M22);
        }
    }
}
