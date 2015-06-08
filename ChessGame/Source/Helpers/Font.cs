using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessGame
{
    static class Font
    {
        /// <summary>
        /// Dictionary with the part of the texture needed (a rectangle) to draw a character.
        /// </summary>
        static private Dictionary<char, Rectangle> Letters;

        /// <summary>
        /// Texture of the font sprite sheet.
        /// </summary>
        static public Texture2D Texture { get; set; }

        private const int charWidth = 55;
        private const int charHeight = 62;
        private const float letterSpacing = 24; // letter spacing
        private const int rows = 9; // rows of the font sprite sheet
        private const int columns = 10; // columns of the font sprite sheet
        private const int emptySpaceSize = 18; // 18 pixel empty space
        private static Vector2 origin = new Vector2(15, 13); // top-left margin of the text


        /// <summary>
        /// Fills up the Letters dictionary.
        /// </summary>
        static public void Initialize()
        {
            Letters = new Dictionary<char, Rectangle>(35);
            // Sequence of the characters in the font sprite sheet
            String charSequence = "*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz!\"#$%&'()";

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    Letters[charSequence[i * columns + j]] = new Rectangle(j * charWidth, i * charHeight, charHeight, charWidth);
        }

        /// <summary>
        /// Draws text on the screen.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch object.</param>
        /// <param name="str">The text we want to draw.</param>
        /// <param name="size">The size of the font.</param>
        /// <param name="position">Top-left corner of the drawing position.</param>
        /// <param name="color">Color of the font.</param>
        static public void DrawString(SpriteBatch spriteBatch, String str, float size, Vector2 position, Color color)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != ' ')
                {
                    if (i != 0)
                        position += new Vector2(size * letterSpacing, 0);

                    try
                    {
                        spriteBatch.Draw(Texture, position, Letters[str[i]], color, 0f, origin, size, SpriteEffects.None, 0f);
                    }
                    catch
                    {
                        Debug.WriteLine("Character `" + str[i] + "` not found in FontSprite.");
                    }
                }
                else
                {
                    position += new Vector2(emptySpaceSize * size, 0);
                }
            }
        }
    }
}
