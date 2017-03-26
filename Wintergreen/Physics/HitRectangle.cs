using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen.Physics
{
    public class HitRectangle : Hitbox
    {
        public HitRectangle(GameObject gameObject) : base(gameObject)
        {
        }

        /// <summary>
        /// Creates a <see cref="HitRectangle"/> Component with the specified <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to which the <see cref="Rectangle"/> belongs.</param>
        /// <param name="hitBoxRectangle">The <see cref="HitRectangle"/>'s <see cref="Rectangle"/> with location relative to the <see cref="GameObject"/>.</param>
        public HitRectangle(GameObject gameObject, Rectangle hitBoxRectangle) : base(gameObject, hitBoxRectangle)
        {
        }

        /// <summary>
        /// Creates a <see cref="HitRectangle"/> Component with a <see cref="Rectangle"/> with the specified location and size.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to which the <see cref="Rectangle"/> belongs.</param>
        /// <param name="locationRelativeToObject">A <see cref="Vector2"/> representing the location relative to the <see cref="GameObject"/>.</param>
        /// <param name="hitBoxSize">The size of the <see cref="Rectangle"/>.</param>
        public HitRectangle(GameObject gameObject, Vector2 locationRelativeToObject, Vector2 rectangleSize) : base(gameObject, new Rectangle(locationRelativeToObject, rectangleSize))
        {
        }

        /// <summary>
        /// Creates a <see cref="HitRectangle"/> Component with a <see cref="Rectangle"/> with the specified location and size.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to which the <see cref="Rectangle"/> belongs.</param>
        /// <param name="x">A <see cref="float"/> representing the x component of the <see cref="Rectangle"/>'s location relative to the <see cref="GameObject"/>.</param>
        /// <param name="y">A <see cref="float"/> representing the y component of the <see cref="Rectangle"/>'s location relative to the <see cref="GameObject"/>.</param>
        /// <param name="width">A <see cref="float"/> representing the width of the <see cref="Rectangle"/>.</param>
        /// <param name="height">A <see cref="float"/> representing the height of the <see cref="Rectangle"/>.</param>
        public HitRectangle(GameObject gameObject, float x, float y, float width, float height) : base(gameObject, new Rectangle(x, y, width, height))
        {
        }

        public override IShape Shape
        {
            get
            {
                return new Rectangle(Location, (Vector2)Size);
            }
        }
    }
}
