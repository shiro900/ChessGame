using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ChessGame
{
    static class Chess
    {
        /// <summary>
        /// List of the current pieces in the game.
        /// </summary>
        static public List<Piece> Pieces { get; set; }

        /// <summary>
        /// Dictionary with the part of the texture needed (a rectangle) to draw a chess piece.
        /// </summary>
        static public Dictionary<string, Rectangle> SourceRectangleDict { get; private set; }

        /// <summary>
        /// Margin between the top-left corner of ChessBoardTexture, and the top-left block of the chess board.
        /// </summary>
        static public Point Margin { get; private set; }

        /// <summary>
        /// The size of the block of the chess board with the right and bottom border.
        /// </summary>
        static public Point BlockSize { get; private set; }

        /// <summary>
        /// The size of the chess block without its border.
        /// </summary>
        static private Point BlockNoBorderSize;

        /// <summary>
        /// The piece which the current player hovers over, null if user hovers over empty block.
        /// </summary>
        static public Piece HoverPiece { get; private set; }

        /// <summary>
        /// The piece which the currecnt player selected.
        /// </summary>
        static public Piece SelectedPiece { get; private set; }

        /// <summary>
        /// The current player's index.
        /// </summary>
        static public short CurrentPlayerIndex { get; private set; }

        static public Texture2D ChessBoardTexture { get; set; }
        static public Texture2D ChessPiecesTexture { get; set; }
        static public Texture2D SelectedBlockTexture1 { get; set; }
        static public Texture2D SelectedBlockTexture2 { get; set; }
        static public Texture2D ReachableBlockTexture1 { get; set; }
        static public Texture2D ReachableBlockTexture2 { get; set; }

        static private float hoverTimeCounter;
        static private Piece previousHoverPiece;

        // The duration of the hovering over the chess piece effect (in seconds).
        static private float hoverDuration;

        // The minimum multiplier for the darkness of the color of the hover animation 
        // value 0 results in a complete black texture
        // value 0.5 results in a darker colored texture
        // value 1 gives the original color to the texture
        static private float hover_minColorMultiplier;


        /// <summary>
        /// Initializes various variables such as the chess pieces.
        /// </summary>
        static public void Initialize()
        {
            CurrentPlayerIndex = 1;
            Margin = new Point(41, 41);
            BlockNoBorderSize = new Point(80, 80);
            BlockSize = new Point(82, 82);
            hoverDuration = 0.25f; // seconds
            hover_minColorMultiplier = 0.1f;

            // Fill up the source dictionary, depending on the texture we are using
            SourceRectangleDict = new Dictionary<string, Rectangle>(6);
            SourceRectangleDict["King"] = new Rectangle(0, 0, 64, 64);
            SourceRectangleDict["Queen"] = new Rectangle(64, 0, 64, 64);
            SourceRectangleDict["Castle"] = new Rectangle(128, 0, 64, 64);
            SourceRectangleDict["Knight"] = new Rectangle(192, 0, 64, 64);
            SourceRectangleDict["Bishop"] = new Rectangle(256, 0, 64, 64);
            SourceRectangleDict["Pawn"] = new Rectangle(320, 0, 64, 64);


            // Initialize the chess pieces positions on the chess board
            Pieces = new List<Piece>(32);

            // Player 1 has white chess pieces and is on the bottom side
            // Player 2 has black chess pieces and is on the top side

            // Setup the chess board...
            // Create the chess pieces and add them to the chess board
            for (int playerIndex = 1; playerIndex <= 2; playerIndex++)  //Foreach Player
            {
                int row, pawnsRow;

                if (playerIndex == 1) 
                    { row = 8; pawnsRow = 7; }
                else 
                    { row = 1; pawnsRow = 2; }

                Pieces.Add(new Castle(new Point(1, row), playerIndex));
                Pieces.Add(new Knight(new Point(2, row), playerIndex));
                Pieces.Add(new Bishop(new Point(3, row), playerIndex));
                Pieces.Add(new Queen(new Point(4, row), playerIndex));
                Pieces.Add(new King(new Point(5, row), playerIndex));
                Pieces.Add(new Bishop(new Point(6, row), playerIndex));
                Pieces.Add(new Knight(new Point(7, row), playerIndex));
                Pieces.Add(new Castle(new Point(8, row), playerIndex));
                for (int i = 1; i <= 8; i++)
                    Pieces.Add(new Pawn(new Point(i, pawnsRow), playerIndex));
            }
        }

        /// <summary>
        /// Updates the chess game logic.
        /// </summary>
        static public void Update()
        {
            // Reset and re-calculate HoverPiece
            HoverPiece = null;
            Point hoverBlockPosition = Util.CoordinatesToPosition(InputHandler.Position);
            foreach(Piece piece in Pieces)
            {
                if (piece.Position == hoverBlockPosition)
                {
                    HoverPiece = piece;
                    break; // we found what we were looking for
                }
            }

            // Reset hoverTimeCounter
            if (previousHoverPiece != HoverPiece)
                hoverTimeCounter = 0;

            // On mouse released
            if (InputHandler.Released)
            {
                // If we haven't selected a chess piece
                if (SelectedPiece == null)
                {
                    // If the user selected a chess block which contains an allied piece
                    if (HoverPiece != null && HoverPiece.PlayerIndex == CurrentPlayerIndex)
                    {
                        // Update the selected piece, and find its reachable blocks
                        SelectedPiece = HoverPiece;
                        SelectedPiece.FindReachableBlocks();
                    }
                }
                else // If we have selected a chess piece
                {
                    // True if the user clicked on a reachable block
                    bool clickedOnReachableBlock = false;

                    foreach (ReachableBlock block in SelectedPiece.ReachableBlocks)
                    {
                        // Check if the user selected a reachable block (from the selected chess piece) 
                        if (block.Position == hoverBlockPosition)
                        {
                            clickedOnReachableBlock = true;

                            // Move the selected piece
                            SelectedPiece.Position = hoverBlockPosition;
                            // Unselect the piece
                            SelectedPiece = null;
                            // Change the current player
                            if (CurrentPlayerIndex == 1)
                                CurrentPlayerIndex = 2;
                            else
                                CurrentPlayerIndex = 1;

                            // Remove the enemy piece from the pieces list
                            if (HoverPiece != null)
                                Pieces.Remove(HoverPiece);

                            break;
                        }
                    }

                    // If the user didn't select a reachable block
                    if (!clickedOnReachableBlock)
                    {
                        // If the user either selected an empty chess block or an enemy chess piece
                        if (HoverPiece == null || HoverPiece.PlayerIndex != CurrentPlayerIndex)
                        {
                            SelectedPiece = null; // Reset the selected piece
                        }
                        else // If the user selected an ally chess piece
                        {
                            // Update the selected piece and find its reachable blocks
                            SelectedPiece = HoverPiece;
                            SelectedPiece.FindReachableBlocks();
                        }
                    }
                }
            }

            previousHoverPiece = HoverPiece;
        }

        /// <summary>
        /// Draws the chess board.
        /// </summary>
        static public void DrawChessBoard(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ChessBoardTexture, new Rectangle(0, 0, ChessBoardTexture.Width, ChessBoardTexture.Height), Color.White);
        }

        /// <summary>
        /// Draws the chess pieces.
        /// </summary>
        static public void DrawChessPieces(SpriteBatch spriteBatch)
        {
            foreach(Piece piece in Pieces)
            {
                // Get the position of the chess piece in screen coordinates
                Vector2 position = Util.PositionToCoordinates(piece.Position);

                // Get the proper sourceRectangle according to the player's index
                // Player 1 has white chess pieces and is on the bottom side
                // Player 2 has black chess pieces and is on the top side
                Rectangle sourceRectangle = SourceRectangleDict[piece.GetType().Name];
                if (piece.PlayerIndex == 1) 
                    sourceRectangle.Y += sourceRectangle.Height;

                Vector2 origin = sourceRectangle.GetHalfSize() - BlockSize.ToVector2().GetHalfSize();

                spriteBatch.Draw(ChessPiecesTexture, position, sourceRectangle, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Draw a rectangle over the ally chess piece which the user hovers over.
        /// </summary>
        static public void DrawHoverPiece(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (SelectedPiece != null || // we don't want to draw the hover effect if the user has a chess piece selected 
                HoverPiece == null ||
                HoverPiece.PlayerIndex != CurrentPlayerIndex)
                return;

            if (hoverTimeCounter < hoverDuration)
                hoverTimeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float colorMultiplier = (hoverTimeCounter / hoverDuration) * (1 - hover_minColorMultiplier)
                + hover_minColorMultiplier; // from hover_minColorMultiplier to 1

            Vector2 position = Util.PositionToCoordinates(HoverPiece.Position);

            Texture2D texture;
            if (CurrentPlayerIndex == 1)
                texture = SelectedBlockTexture1;
            else
                texture = SelectedBlockTexture2;
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, BlockNoBorderSize.X, BlockNoBorderSize.Y), Color.White * colorMultiplier);
        }

        /// <summary>
        /// Draws a rectangle of the selected chess piece.
        /// </summary>
        static public void DrawSelectedPiece(SpriteBatch spriteBatch)
        {
            if (SelectedPiece == null)
                return;

            // Get the position of the selected piece, to screen coordinates
            Vector2 position = Util.PositionToCoordinates(SelectedPiece.Position);

            Texture2D texture;
            if (CurrentPlayerIndex == 1)
                texture = SelectedBlockTexture1;
            else
                texture = SelectedBlockTexture2;
        }

        /// <summary>
        /// Draws a rectangle over each reachable block of the selected chess piece.
        /// </summary>
        static public void DrawReachableBlocks(SpriteBatch spriteBatch)
        {
            if (SelectedPiece == null)
                return;

            foreach (ReachableBlock block in SelectedPiece.ReachableBlocks)
            {
                // Get the position of the reachable block, to screen coordinates
                Vector2 position = Util.PositionToCoordinates(block.Position);

                Texture2D texture;
                if (CurrentPlayerIndex == 1)
                    texture = ReachableBlockTexture1;
                else
                    texture = ReachableBlockTexture2;
                spriteBatch.Draw(texture, position, new Rectangle(0, 0, BlockNoBorderSize.X, BlockNoBorderSize.Y), Color.White);
            }
        }
    }
}