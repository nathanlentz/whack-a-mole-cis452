using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen.Physics
{
    public abstract class Hitbox : Component
    {
        protected IShape _shape;
        
        /// <summary>
        /// The <see cref="Hitbox"/>'s shape with location relative to the <see cref="GameObject"/>
        /// </summary>
        public virtual IShape RelativeShape
        {
            get { return _shape; }
        }

        /// <summary>
        /// The location of the <see cref="Hitbox"/> relative to the <see cref="GameObject"/>
        /// </summary>
        public virtual Vector2 RelativeLocation
        {
            get { return _shape.Location; }
        }

        /// <summary>
        /// The size of the <see cref="Hitbox"/>
        /// </summary>
        public virtual object Size
        {
            get { return _shape.Size; }
        }

        /// <summary>
        /// The absolute location of the <see cref="Hitbox"/>
        /// </summary>
        public virtual Vector2 Location
        {
            get { return new Vector2(Object.Location.X + _shape.X, Object.Location.Y + _shape.Y); }
        }

        /// <summary>
        /// The <see cref="Hitbox"/>'s shape with absolute location
        /// </summary>
        public abstract IShape Shape
        {
            get;
        }

        public Hitbox(GameObject gameObject) : base(gameObject)
        {
        }

        /// <summary>
        /// Creates a <see cref="Hitbox"/> Component with a shape of the type <see cref="T1"/>
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to which the <see cref="Hitbox"/> belongs</param>
        /// <param name="hitBoxRectangle">The <see cref="Hitbox"/>'s <see cref="IShape"/> with location relative to the <see cref="GameObject"/>/param>
        public Hitbox(GameObject gameObject, IShape shape) : base(gameObject)
        {
            _shape = shape;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Hitbox"/> is intersecting this <see cref="Hitbox"/>.
        /// </summary>
        /// <param name="otherHitbox">The <see cref="Hitbox"/> to check.</param>
        /// <returns><c>true</c> if the <see cref="Hitbox"/>es are intersecting; <c>false</c> otherwise.</returns>
        public virtual bool IsIntersecting(Hitbox otherHitbox)
        {
            return _shape.Intersects(otherHitbox.Shape);
        }
    }
}
