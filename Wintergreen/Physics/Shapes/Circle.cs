using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Wintergreen.ExtensionMethods;

namespace Wintergreen.Physics
{
    public struct Circle : IEquatable<Circle>, IShape
    {
        #region Private Fields

        private static Circle emptyCircle = new Circle();
        
        /// <summary>
        /// The x coordinate of the leftmost point of this <see cref="Circle"/>.
        /// </summary>
        private float _x;

        /// <summary>
        /// The y coordinate of the topmost point of this <see cref="Circle"/>.
        /// </summary>
        private float _y;

        /// <summary>
        /// The radius of this <see cref="Circle"/>.
        /// </summary>
        private float _radius;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a <see cref="Circle"/> with X=0, Y=0, Radius=0.
        /// </summary>
        public static Circle Empty
        {
            get { return emptyCircle; }
        }

        /// <summary>
        /// The x coordinate of the leftmost point of this <see cref="Circle"/>.
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// The y coordinate of the leftmost point of this <see cref="Circle"/>.
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// The radius of this <see cref="Circle"/>.
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        /// Returns the x coordinate of the leftmost point of this <see cref="Circle"/>.
        /// </summary>
        public float Left
        {
            get { return _x; }
        }

        /// <summary>
        /// Returns the x coordinate of the rightmost point of this <see cref="Circle"/>.
        /// </summary>
        public float Right
        {
            get { return _x + 2 * _radius; }
        }

        /// <summary>
        /// Returns the y coordinate of the topmost point of this <see cref="Circle"/>.
        /// </summary>
        public float Top
        {
            get { return _y; }
        }

        /// <summary>
        /// Returns the y coordinate of the bottommost point of this <see cref="Circle"/>.
        /// </summary>
        public float Bottom
        {
            get { return _y + 2 * _radius; }
        }

        /// <summary>
        /// Whether or not this <see cref="Circle"/> has a <see cref="_radius"/> of 0
        /// and a <see cref="Location"/> of (0, 0).
        /// </summary>
        public bool IsEmpty
        {
            get { return _x == 0 && _y == 0 && _radius == 0; }
        }

        /// <summary>
        /// The top-left coordinates of this <see cref="Circle"/>.
        /// </summary>
        public Vector2 Location
        {
            get
            {
                return new Vector2(_x, _y);
            }
            set
            {
                _x = value.X;
                _y = value.Y;
            }
        }

        /// <summary>
        /// A <see cref="Vector2"/> located in the center of this <see cref="Circle"/>.
        /// </summary>
        public Vector2 Center
        {
            get { return new Vector2(_x + _radius, _y + _radius); }
        }

        /// <summary>
        /// The x coordinate of the center of this <see cref="Circle"/>
        /// </summary>
        public float CenterX
        {
            get { return _x + _radius; }
        }

        /// <summary>
        /// The y coordinate of the center of this <see cref="Circle"/>
        /// </summary>
        public float CenterY
        {
            get { return _y + _radius; }
        }

        /// <summary>
        /// A <see cref="float"/> representing the radius of this <see cref="Circle"/> 
        /// </summary>
        public object Size
        {
            get { return _radius; }
            set { _radius = (float)value; }
        }

        /// <summary>
        /// A <see cref="float"/> representing the diameter of this <see cref="Circle"/>
        /// </summary>
        public float Diameter
        {
            get { return (2 * _radius); }
        }

        /// <summary>
        /// A <see cref="float"/> representing the area of this <see cref="Circle"/>
        /// </summary>
        public float Area
        {
            get { return (float)(Math.PI * _radius * _radius); }
        }

        /// <summary>
        /// A <see cref="float"/> representing the circumference of this <see cref="Circle"/>
        /// </summary>
        public float Circumference
        {
            get { return (float)(2 * Math.PI * _radius); }
        }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="Circle"/> struct, with the specified
        /// location and radius.
        /// </summary>
        /// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="Circle"/>.</param>
        /// <param name="radius">The radius of the created <see cref="Circle"/>.</param>
        public Circle(Vector2 location, float radius)
        {
            _x = location.X;
            _y = location.Y;
            _radius = radius;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Circle"/> struct, with the specified
        /// position and radius.
        /// </summary>
        /// <param name="x">The x coordinate of the top-left corner of the created <see cref="Circle"/>.</param>
        /// <param name="y">The y coordinate of the top-left corner of the created <see cref="Circle"/>.</param>
        /// <param name="radius">The radius of the created <see cref="Circle"/>.</param>
        public Circle(float x, float y, float radius)
        {
            _x = x;
            _y = y;
            _radius = radius;
        }

        #endregion Constructors

        #region Operators

        /// <summary>
        /// Compares whether two <see cref="Circle"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="Circle"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="Circle"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Circle a, Circle b)
        {
            return ((a._x == b._x) && (a._y == b._y) && (a._radius == b._radius));
        }

        /// <summary>
        /// Compares whether two <see cref="Circle"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="Circle"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="Circle"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Circle a, Circle b)
        {
            return !(a == b);
        }

        #endregion Operators

        #region Public Methods

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="Circle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(int x, int y)
        {
            return (x - CenterX) * (x - CenterX) + (y - CenterY) * (y - CenterY) <= _radius * _radius;
        }

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="Circle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(float x, float y)
        {
            return (x - CenterX) * (x - CenterX) + (y - CenterY) * (y - CenterY) <= _radius * _radius;
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Circle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="Circle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Point value)
        {
            return (value.X - CenterX) * (value.X - CenterX) + (value.Y - CenterY) * (value.Y - CenterY) <= _radius * _radius;
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Circle"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="Circle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref Point value, out bool result)
        {
            result = ((value.X - CenterX) * (value.X - CenterX) + (value.Y - CenterY) * (value.Y - CenterY) <= _radius * _radius);
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Circle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="Circle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Vector2 value)
        {
            return (value.X - CenterX) * (value.X - CenterX) + (value.Y - CenterY) * (value.Y - CenterY) <= _radius * _radius;
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Circle"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="Circle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref Vector2 value, out bool result)
        {
            result = ((value.X - CenterX) * (value.X - CenterX) + (value.Y - CenterY) * (value.Y - CenterY) <= _radius * _radius);
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="IShape"/> lies within the bounds of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="value">The <see cref="IShape"/> to check for inclusion in this <see cref="Circle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="IShape"/>'s bounds lie entirely inside this <see cref="Circle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(IShape value)
        {
            if (value is Circle)
                return Vector2.DistanceSquared(Center, value.Center) <= (_radius - ((Circle)value)._radius) * (_radius - ((Circle)value)._radius);
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="IShape"/> lies within the bounds of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="value">The <see cref="IShape"/> to check for inclusion in this <see cref="Circle"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="IShape"/>'s bounds lie entirely inside this <see cref="Circle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref IShape value, out bool result)
        {
            if (value is Circle)
                result = (Vector2.DistanceSquared(Center, value.Center) <= (_radius - ((Circle)value)._radius) * (_radius - ((Circle)value)._radius));
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Circle) && this == ((Circle)obj);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Circle"/>.
        /// </summary>
        /// <param name="other">The <see cref="Circle"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Circle other)
        {
            return this == other;
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Circle"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Circle"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 911;
                hash = hash * 421 + _x.GetHashCode();
                hash = hash * 421 + _y.GetHashCode();
                hash = hash * 421 + _radius.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Adjusts the edges of this <see cref="Circle"/> by specified horizontal and vertical amounts. 
        /// </summary>
        /// <param name="amount">Value to adjust the radius.</param>
        public void Inflate(int amount)
        {
            _x -= amount;
            _y -= amount;
            _radius += amount;
        }

        /// <summary>
        /// Adjusts the edges of this <see cref="Circle"/> by specified horizontal and vertical amounts. 
        /// </summary>
        /// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
        /// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
        public void Inflate(float amount)
        {
            _x -= amount;
            _y -= amount;
            _radius += amount;
        }

        /// <summary>
        /// Gets whether or not the other <see cref="IShape"/> intersects with this <see cref="Circle"/>.
        /// </summary>
        /// <param name="value">The other <see cref="IShape"/> for testing.</param>
        /// <returns><c>true</c> if other <see cref="IShape"/> intersects with this <see cref="Circle"/>; <c>false</c> otherwise.</returns>
        public bool Intersects(IShape value)
        {
            if (value is Circle)
                return Shape.AreIntersecting(this, (Circle)value);
            else if (value is Rectangle)
                return Shape.AreIntersecting((Rectangle)value, this);
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Gets whether or not the other <see cref="IShape"/> intersects with this <see cref="Circle"/>.
        /// </summary>
        /// <param name="value">The other <see cref="IShape"/> for testing.</param>
        /// <param name="result"><c>true</c> if other <see cref="IShape"/> intersects with this <see cref="Circle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Intersects(ref IShape value, out bool result)
        {
            if (value is Circle)
                result = Shape.AreIntersecting(this, (Circle)value);
            else if (value is Rectangle)
                result = Shape.AreIntersecting((Rectangle)value, this);
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="Circle"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="Circle"/>.</param>
        public void Offset(int offsetX, int offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="Circle"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="Circle"/>.</param>
        public void Offset(float offsetX, float offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="Circle"/>.</param>
        public void Offset(Point amount)
        {
            _x += amount.X;
            _y += amount.Y;
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="Circle"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="Circle"/>.</param>
        public void Offset(Vector2 amount)
        {
            _x += amount.X;
            _y += amount.Y;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Circle"/> in the format:
        /// {X:[<see cref="_x"/>] Y:[<see cref="_y"/>] Width:[<see cref="Width"/>] Radius:[<see cref="_radius"/>]}
        /// </summary>
        /// <returns><see cref="string"/> representation of this <see cref="Circle"/>.</returns>
        public override string ToString()
        {
            return "{X:" + _x + " Y:" + _y + " Radius:" + _radius + "}";
        }

        /// <summary>
        /// Creates a new <see cref="Circle"/> that completely contains two other circles.
        /// </summary>
        /// <param name="value1">The first <see cref="Circle"/>.</param>
        /// <param name="value2">The second <see cref="Circle"/>.</param>
        /// <returns>The union of the two circles.</returns>
        public static Circle Union(Circle value1, Circle value2)
        {
            float radius = Vector2.Distance(value1.Center, value2.Center) + value1._radius + value2._radius;
            float centerX = Math.Max(value1.CenterX, value2.CenterX) - Math.Min(value1.CenterX, value2.CenterX);
            float centerY = Math.Max(value1.CenterY, value2.CenterY) - Math.Min(value1.CenterY, value2.CenterY);
            return new Circle(centerX - radius, centerY - radius, radius);
        }

        /// <summary>
        /// Creates a new <see cref="Circle"/> that completely contains two other circles.
        /// </summary>
        /// <param name="value1">The first <see cref="Circle"/>.</param>
        /// <param name="value2">The second <see cref="Circle"/>.</param>
        /// <param name="result">The union of the two circles as an output parameter.</param>
        public static void Union(ref Circle value1, ref Circle value2, out Circle result)
        {
            float radius = Vector2.Distance(value1.Center, value2.Center) + value1._radius + value2._radius;
            float centerX = Math.Max(value1.CenterX, value2.CenterX) - Math.Min(value1.CenterX, value2.CenterX);
            float centerY = Math.Max(value1.CenterY, value2.CenterY) - Math.Min(value1.CenterY, value2.CenterY);
            result = new Circle(centerX - radius, centerY - radius, radius);
        }

        #endregion Public Methods
    }
}
