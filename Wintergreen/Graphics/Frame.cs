using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen.Graphics
{
    public struct Frame
    {
        /// <summary>
        /// The X and Y coordinates of the frame on the sprite sheet.
        /// </summary>
        public Point SpriteSheetCoordinates{ get; set; }

        /// <summary>
        /// How long the frame plays before advancing to the next frame.
        /// This is a float so it can be either number of updates or number of milliseconds.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Creates a Frame with the specified sprite sheet coordinates and duration
        /// </summary>
        /// <param name="spriteSheetCoordinates"></param>
        /// <param name="duration"></param>
        public Frame(Point spriteSheetCoordinates, float duration)
        {
            SpriteSheetCoordinates = spriteSheetCoordinates;
            Duration = duration;
        }

        /// <summary>
        /// Creates a Frame with the specified sprite sheet coordinates and duration
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="duration"></param>
        public Frame(int x, int y, float duration)
        {
            SpriteSheetCoordinates = new Point(x, y);
            Duration = duration;
        }
    }
}
