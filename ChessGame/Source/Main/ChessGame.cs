using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessGame
{
    /// <summary>
    /// Contains the game initialization and the game loop.
    /// Game Initialization is achieved by calling Game() constructor, then Initialize() and then LoadContent() in this sequence.
    /// Game loop is achieved by calling the pair Update(); and Draw(); producing 1 frame at a time.
    /// Sometimes Draw() is skipped depending on the workload and hardware specs, but Draw() is not called if Update() is halfway completed. 
    /// </summary>
    public class ChessGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// The game constructor. It is called before Initialize().
        /// </summary>
        public ChessGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
#if WINDOWS
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += delegate { Resolution.WasResized = true; };
            graphics.PreferredBackBufferWidth = 736; // window's width
            graphics.PreferredBackBufferHeight = 736; // window's height
#endif
#if __ANDROID__
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = Resolution.ScreenWidth;//window's width
            graphics.PreferredBackBufferHeight = Resolution.ScreenHeight;//window's height
#endif
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Initialize is called once per game and allows the game to perform any non-graphic initialization it needs to before starting to run.
        /// 'base.Initialize' will enumerate through any game components and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Resolution.Initialize(graphics);
            Chess.Initialize();
            Font.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent is called once per game and is the place to load all of the content, for example textures.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which will be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Chess.ChessBoardTexture = Content.Load<Texture2D>("ChessBoard");
            Chess.ChessPiecesTexture = Content.Load<Texture2D>("ChessPieces");
            Font.Texture = Content.Load<Texture2D>("FontSprite");
            // Player 1 colors
            Chess.SelectedBlockTexture1 = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Chess.SelectedBlockTexture1.SetData<Color>(new Color[] { new Color(170, 170, 170) * 0.6f });
            Chess.ReachableBlockTexture1 = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Chess.ReachableBlockTexture1.SetData<Color>(new Color[] { new Color(170, 170, 170) * 0.6f });
            // Player 2 colors
            Chess.SelectedBlockTexture2 = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Chess.SelectedBlockTexture2.SetData<Color>(new Color[] { new Color(171, 149, 89) * 0.6f });
            Chess.ReachableBlockTexture2 = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Chess.ReachableBlockTexture2.SetData<Color>(new Color[] { new Color(171, 149, 89) * 0.6f });
        }

        /// <summary>
        /// Updates the game logic.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update(this, graphics);
#if WINDOWS
            Resolution.Update(this, graphics);
#endif
            Chess.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game components.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Resolution.ScaleMatrix);
            Chess.DrawChessBoard(spriteBatch);
            Chess.DrawChessPieces(spriteBatch);
            Chess.DrawHoverPiece(spriteBatch, gameTime);
            Chess.DrawSelectedPiece(spriteBatch);
            Chess.DrawReachableBlocks(spriteBatch);

            Color fontColor;
            if (Chess.CurrentPlayerIndex == 1) 
                fontColor = new Color(212, 208, 225);
            else 
                fontColor = new Color(192, 169, 107);
            Font.DrawString(spriteBatch, "Player " + Chess.CurrentPlayerIndex, 0.75f, new Vector2(32, 5), fontColor);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}