using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen.Physics
{
    public class HitCircle : Hitbox
    {
        public HitCircle(GameObject gameObject) : base(gameObject)
        {
        }

        /// <summary>
        /// Creates a <see cref="HitCircle"/> Component with the specified <see cref="Circle"/>
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to which the <see cref="Circle"/> belongs</param>
        /// <param name="hitBoxCircle">The <see cref="HitCircle"/>'s <see cref="Circle"/> with location relative to the <see cref="GameObject"/>/param>
        public HitCircle(GameObject gameObject, Circle hitBoxCircle) : base(gameObject, hitBoxCircle)
        {
        }

        /// <summary>
        /// Creates a <see cref="HitCircle"/> Component with a <see cref="Circle"/> with the specified location and radius.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to which the <see cref="Rectangle"/> belongs</param>
        /// <param name="locationRelativeToObject">A <see cref="Vector2"/> representing the location relative to the <see cref="GameObject"/></param>
        /// <param name="radius">A <see cref="float"/> representing the radius of the <see cref="Circle"/>.</param>
        public HitCircle(GameObject gameObject, Vector2 locationRelativeToObject, float radius) : base(gameObject, new Circle(locationRelativeToObject, radius))
        {
        }

        /// <summary>
        /// Creates a <see cref="HitCircle"/> Component with a <see cref="Circle"/> with the specified location and radius.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to which the <see cref="Rectangle"/> belongs</param>
        /// <param name="x">A <see cref="float"/> representing the x component of the <see cref="Circle"/>'s location relative to the <see cref="GameObject"/></param>
        /// <param name="y">A <see cref="float"/> representing the y component of the <see cref="Circle"/>'s location relative to the <see cref="GameObject"/></param>
        /// <param name="radius">A <see cref="float"/> representing the radius of the <see cref="Circle"/>.</param>
        public HitCircle(GameObject gameObject, float x, float y, float radius) : base(gameObject, new Circle(x, y, radius))
        {
        }

        public override IShape Shape
        {
            get
            {
                return new Circle(Location, (float)Size);
            }
        }
    }
}
