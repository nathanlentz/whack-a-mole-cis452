using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen.Physics
{
    public static class Shape
    {
        public static bool AreIntersecting(Rectangle r1, Rectangle r2)
        {
            return r2.Left < r1.Right &&
                   r1.Left < r2.Right &&
                   r2.Top < r1.Bottom &&
                   r1.Top < r2.Bottom;
        }

        public static bool AreIntersecting(Rectangle r, Circle c)
        {
            float halfWidth = r.Width / 2;
            float xDistance = Math.Abs(r.Center.X - c.CenterX);
            if (xDistance > c.Radius + halfWidth)
                return false;

            float halfHeight = r.Height / 2;
            float yDistance = Math.Abs(r.Center.Y - c.CenterY);
            if (yDistance > c.Radius + halfHeight)
                return false;

            if (xDistance <= halfWidth || yDistance <= halfHeight)
                return true;

            return (xDistance - halfWidth) * (xDistance - halfWidth)
                + (yDistance - halfHeight) * (yDistance - halfHeight)
                <= c.Radius * c.Radius;
        }

        public static bool AreIntersecting(Circle c1, Circle c2)
        {
            return Vector2.DistanceSquared(c1.Center, c2.Center) <= (c1.Radius + c2.Radius) * (c1.Radius + c2.Radius);
        }
    }
}
