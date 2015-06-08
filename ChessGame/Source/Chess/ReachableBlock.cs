using System;
using Microsoft.Xna.Framework;

namespace ChessGame
{
    /// <summary>
    /// The reachable points which a chess piece can move to.
    /// It either contains an enemy piece, or it is empty.
    /// </summary>
    class ReachableBlock
    {
        public Point Position { get; set; }
        public enum Type { Empty, ContainsEnemyChessPiece }

        /// <summary>
        /// The type of the reachable block. Either 'Empty' or 'ContainsEnemyChessPiece'.
        /// </summary>
        public Type type { get; set; }


        /// <summary>
        /// Creates a Reachable Block
        /// </summary>
        /// <param name="position">Position of the chess piece (1,1) to (8,8).</param>
        public ReachableBlock(Point position)
        {
            Position = position;
            type = Type.Empty; //initialize it as Empty
        }

        /// <summary>
        /// Creates a Reachable Block.
        /// </summary>
        /// <param name="position">Position of the chess piece (1,1) to (8,8).</param>
        /// <param name="type">The type of the reachable block. Either 'Empty' or 'ContainsEnemyChessPiece'.</param>
        public ReachableBlock(Point position, Type type)
        {
            Position = position;
            this.type = type;
        }
    }
}