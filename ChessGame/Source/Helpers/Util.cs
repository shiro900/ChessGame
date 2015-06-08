using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessGame
{
    static public class Util
    {
        /// <summary>
        /// Converts screen coordinates, to chess board coordinates ( (1,1) to (8,8) ).
        /// </summary>
        static public Point CoordinatesToPosition(Vector2 pos)
        {
            return ((pos - Chess.Margin.ToVector2()) / Chess.BlockSize.ToVector2()).ToPoint() + new Point(1, 1);
        }

        /// <summary>
        /// Converts chess board coordinates ( (1,1) to (8,8) ), to screen coordinates.
        /// </summary>
        static public Vector2 PositionToCoordinates(Point pos)
        {
            return (Chess.Margin + new Point((pos.X - 1) * Chess.BlockSize.X, (pos.Y - 1) * Chess.BlockSize.Y)).ToVector2();
        }

        /// <summary>
        /// Texture2D class extension, get half size of a Texture2D
        /// </summary>
        static public Vector2 GetHalfSize(this Texture2D texture)
        {
            return new Vector2(texture.Width / 2, texture.Height / 2);
        }

        /// <summary>
        /// Vector2 class extension, get half size of a Vector2
        /// </summary>
        static public Vector2 GetHalfSize(this Vector2 vector)
        {
            return new Vector2(vector.X / 2, vector.Y / 2);
        }

        /// <summary>
        /// Rectangle class extension, get half size of a Rectangle
        /// </summary>
        static public Vector2 GetHalfSize(this Rectangle rect)
        {
            return new Vector2(rect.Width / 2, rect.Height / 2);
        }
    }
}
