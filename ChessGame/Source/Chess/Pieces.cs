using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ChessGame
{
    /// <summary>
    /// The king chess piece.
    /// </summary>
    class King : Piece
    {
        public King(Point position, int playerIndex) : base(position, playerIndex) { }

        override public void FindReachableBlocks()
        {
            ReachableBlocks = new List<ReachableBlock>(8);
            List<Point> possiblePositions = new List<Point>(8);

            // Left column of blocks
            possiblePositions.Add(new Point(Position.X - 1, Position.Y - 1));
            possiblePositions.Add(new Point(Position.X - 1, Position.Y));
            possiblePositions.Add(new Point(Position.X - 1, Position.Y + 1));
            // Middle column of blocks
            possiblePositions.Add(new Point(Position.X, Position.Y - 1));
            possiblePositions.Add(new Point(Position.X, Position.Y + 1));
            // Right column of blocks
            possiblePositions.Add(new Point(Position.X + 1, Position.Y - 1));
            possiblePositions.Add(new Point(Position.X + 1, Position.Y));
            possiblePositions.Add(new Point(Position.X + 1, Position.Y + 1));

            // Fill ReachableBlocks list 
            foreach (Point pos in possiblePositions)
                    AddReachableBlock(pos);
        }
    }


    /// <summary>
    /// The queen chess piece.
    /// </summary>
    class Queen : Piece
    {
        public Queen(Point position, int playerIndex) : base(position, playerIndex) { }

        override public void FindReachableBlocks()
        {
            ReachableBlocks = new List<ReachableBlock>(27);

            FindReachableBlocks_Horizontal();
            FindReachableBlocks_Vertical();
            FindReachableBlocks_Diagonal();
        }
    }


    /// <summary>
    /// The castle chess piece.
    /// </summary>
    class Castle : Piece
    {
        public Castle(Point position, int playerIndex) : base(position, playerIndex) { }

        override public void FindReachableBlocks()
        {
            ReachableBlocks = new List<ReachableBlock>(14);

            FindReachableBlocks_Horizontal();
            FindReachableBlocks_Vertical();
        }
    }


    /// <summary>
    /// The bishop chess piece.
    /// </summary>
    class Bishop : Piece
    {
        public Bishop(Point position, int playerIndex) : base(position, playerIndex) { }

        override public void FindReachableBlocks()
        {
            ReachableBlocks = new List<ReachableBlock>(13);

            FindReachableBlocks_Diagonal();
        }
    }


    /// <summary>
    /// The knight chess piece.
    /// </summary>
    class Knight : Piece
    {
        public Knight(Point position, int playerIndex) : base(position, playerIndex) { }

        override public void FindReachableBlocks()
        {
            ReachableBlocks = new List<ReachableBlock>(8);
            List<Point> possiblePositions = new List<Point>(8);

            // Top-left quarter
            possiblePositions.Add(new Point(Position.X - 2, Position.Y - 1));
            possiblePositions.Add(new Point(Position.X - 1, Position.Y - 2));
            // Top-right quarter
            possiblePositions.Add(new Point(Position.X + 1, Position.Y - 2));
            possiblePositions.Add(new Point(Position.X + 2, Position.Y - 1));
            // Bottom-right quarter
            possiblePositions.Add(new Point(Position.X + 2, Position.Y + 1));
            possiblePositions.Add(new Point(Position.X + 1, Position.Y + 2));
            // Bottom-left quarter
            possiblePositions.Add(new Point(Position.X - 1, Position.Y + 2));
            possiblePositions.Add(new Point(Position.X - 2, Position.Y + 1));

            // Fill ReachableBlocks list 
            foreach (Point pos in possiblePositions) 
                    AddReachableBlock(pos);
        }
    }


    /// <summary>
    /// The pawn chess piece.
    /// </summary>
    class Pawn : Piece
    {
        public Pawn(Point position, int playerIndex) : base(position, playerIndex) { }

        override public void FindReachableBlocks()
        {
            List<Point> possiblePositions = new List<Point>(3);
            ReachableBlocks = new List<ReachableBlock>(3);
            Point topBlockPosition;

            // Top direction
            if (PlayerIndex == 1)
            {
                // Top-left block
                possiblePositions.Add(new Point(Position.X - 1, Position.Y - 1));
                // Top block
                topBlockPosition = new Point(Position.X, Position.Y - 1);
                // Top-right block
                possiblePositions.Add(new Point(Position.X + 1, Position.Y - 1));
            }
            else // Bottom direction
            {
                // Top-left block
                possiblePositions.Add(new Point(Position.X - 1, Position.Y + 1));
                // Top block
                topBlockPosition = new Point(Position.X, Position.Y + 1);
                // Top-right block
                possiblePositions.Add(new Point(Position.X + 1, Position.Y + 1));
            }

            // Fill ReachableBlocks list 
            foreach (Point pos in possiblePositions)
                AddReachableBlock(pos);
            AddReachableBlock_Pawn(topBlockPosition); // Needs to be added with a special function because Pawns can't kill opponent chess pieces directly above them
        }
    }
}