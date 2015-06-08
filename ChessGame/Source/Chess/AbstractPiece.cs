using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ChessGame
{    
    /// <summary>
    /// Abstract implementation of a chess piece.
    /// </summary>
    abstract class Piece
    {
        /// <summary>
        /// Position of the chess piece (1,1) to (8,8).
        /// </summary>
        public Point Position { get; set; }

        /// Player with index 1 is bottom (white pieces)
        /// Player with index 2 is top (black pieces)
        /// 
        /// <summary>
        /// Index of the player that the piece belongs to.
        /// </summary>
        public int PlayerIndex { get; private set; }

        /// <summary>
        /// List of the reachable blocks for the chess piece.
        /// </summary>
        public List<ReachableBlock> ReachableBlocks { get; set; }


        /// <summary>
        /// Creates a chess piece.
        /// </summary>
        /// <param name="position">Position of the chess piece (1,1) to (8,8).</param>
        /// <param name="playerIndex">Index of the player that the piece belongs to.</param>
        protected Piece(Point position, int playerIndex)
        {
            Position = position;
            PlayerIndex = playerIndex;
        }

        /// The calculation of the reachable blocks differs from chess piece to chess piece
        /// therefore its marked as abstract and implemented accordingly on every chess piece type (castle, king, queen, etc.)
        /// 
        /// <summary>
        /// Finds the blocks which a piece can reach.
        /// </summary>
        abstract public void FindReachableBlocks();

        /// <summary>
        /// Adds the block of the given position to the ReachableBlock list, unless that block contains an ally chess piece.
        /// </summary>
        /// <param name="pos">The given position of the block.</param>
        /// <returns>Returns true if the given position is the position of a non-empty block or the position is out of the chess board.</returns>
        protected bool AddReachableBlock(Point pos)
        {
            bool blockIsEmpty = true; // True if there are no chess pieces on pos
            bool blockOutOfBounds = true; // True if pos is out of the chess board

            // Check if pos is a position inside the chess board
            if (pos.X >= 1 && pos.Y >= 1 && pos.X <= 8 && pos.Y <= 8)
            {
                blockOutOfBounds = false;

                foreach (Piece piece in Chess.Pieces)
                {
                    if (pos == piece.Position)
                    {
                        if (PlayerIndex != piece.PlayerIndex)
                            ReachableBlocks.Add(new ReachableBlock(pos, ReachableBlock.Type.ContainsEnemyChessPiece));

                        blockIsEmpty = false; // pos is not empty
                        break; // break the loop
                    }
                }

                if (blockIsEmpty)
                    ReachableBlocks.Add(new ReachableBlock(pos));
            }

            return !blockIsEmpty || blockOutOfBounds;
        }

        /// <summary>
        /// Adds the block of the given position to the ReachableBlock list, specifically for the Pawn chess piece.
        /// </summary>
        /// <param name="pos">The given position of the block.</param>
        protected void AddReachableBlock_Pawn(Point pos)
        {
            bool blockIsEmpty = true; // True if there are no chess pieces on pos

            // Check if pos is a position inside the chess board
            if (pos.X >= 1 && pos.Y >= 1 && pos.X <= 8 && pos.Y <= 8)
            {
                foreach (Piece piece in Chess.Pieces)
                {
                    if (pos == piece.Position)
                    {
                        blockIsEmpty = false; // pos is not empty
                        break; // break the loop
                    }
                }

                if (blockIsEmpty) 
                    ReachableBlocks.Add(new ReachableBlock(pos));
            }
        }

        /// <summary>
        /// Finds the reachable blocks in any horizontal direction.
        /// </summary>
        protected void FindReachableBlocks_Horizontal()
        {
            // Right direction
            Point pos = Position;
            for (int i = 0; i < 7; i++) {
                pos.X += 1;
                if (AddReachableBlock(pos)) break;
            }

            // Left direction
            pos = Position;
            for (int i = 0; i < 7; i++) {   
                pos.X -= 1;
                if (AddReachableBlock(pos)) break;
            }
        }

        /// <summary>
        /// Finds the reachable blocks in any vertical direction.
        /// </summary>
        protected void FindReachableBlocks_Vertical()
        {
            // Bottom direction
            Point pos = Position;
            for (int i = 0; i < 7; i++) {
                pos.Y += 1;
                if (AddReachableBlock(pos)) break;
            }

            // Top direction
            pos = Position;
            for (int i = 0; i < 7; i++) {
                pos.Y -= 1;
                if (AddReachableBlock(pos)) break;
            }
        }

        /// <summary>
        /// Finds the reachable blocks in any diagonal direction.
        /// </summary>
        protected void FindReachableBlocks_Diagonal()
        {
            // Top-right direction
            Point pos = Position;
            for (int i = 0; i < 7; i++)
            {
                pos += new Point(1, -1);
                if (AddReachableBlock(pos)) break;
            }

            // Top-left direction
            pos = Position;
            for (int i = 0; i < 7; i++)
            {
                pos += new Point(-1, -1);
                if (AddReachableBlock(pos)) break;
            }

            // Bottom-right direction
            pos = Position;
            for (int i = 0; i < 7; i++) {
                pos += new Point(1, 1);
                if (AddReachableBlock(pos)) break;
            }

            // Bottom-left direction
            pos = Position;
            for (int i = 0; i < 7; i++)
            {
                pos += new Point(-1, 1);
                if (AddReachableBlock(pos)) break;
            }
        }
    }
}