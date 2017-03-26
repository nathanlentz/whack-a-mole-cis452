using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Wintergreen.ExtensionMethods;

namespace Wintergreen.Physics
{
    /// <summary>
    /// Describes a 2D-rectangle with <see cref="float"/> size and location
    /// </summary>
    /// <remarks>Based on MonoGame's Rectangle class using <see cref="float"/> instead of <see cref="int"/> and some other changes.</remarks>
    public struct Rectangle : IEquatable<Rectangle>, IShape
    {
        #region Private Fields

        private static Rectangle emptyRectangle = new Rectangle();

        /// <summary>
        /// The x coordinate of the top-left corner of this <see cref="Rectangle"/>.
        /// </summary>
        private float _x;

        /// <summary>
        /// The y coordinate of the top-left corner of this <see cref="Rectangle"/>.
        /// </summary>
        private float _y;

        /// <summary>
        /// The width of this <see cref="Rectangle"/>.
        /// </summary>
        private float _width;

        /// <summary>
        /// The height of this <see cref="Rectangle"/>.
        /// </summary>
        private float _height;

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a <see cref="Rectangle"/> with X=0, Y=0, Width=0, Height=0.
        /// </summary>
        public static Rectangle Empty
        {
            get { return emptyRectangle; }
        }

        /// <summary>
        /// The x coordinate of the top-left corner of this <see cref="Rectangle"/>.
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// The y coordinate of the top-left corner of this <see cref="Rectangle"/>.
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// The width of this <see cref="Rectangle"/>.
        /// </summary>
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// The height of this <see cref="Rectangle"/>.
        /// </summary>
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Returns the x coordinate of the left edge of this <see cref="Rectangle"/>.
        /// </summary>
        public float Left
        {
            get { return _x; }
        }

        /// <summary>
        /// Returns the x coordinate of the right edge of this <see cref="Rectangle"/>.
        /// </summary>
        public float Right
        {
            get { return _x + _width; }
        }

        /// <summary>
        /// Returns the y coordinate of the top edge of this <see cref="Rectangle"/>.
        /// </summary>
        public float Top
        {
            get { return _y; }
        }

        /// <summary>
        /// Returns the y coordinate of the bottom edge of this <see cref="Rectangle"/>.
        /// </summary>
        public float Bottom
        {
            get { return _y + _height; }
        }


        /// <summary>
        /// Whether or not this <see cref="Rectangle"/> has a <see cref="_width"/> and
        /// <see cref="_height"/> of 0, and a <see cref="Location"/> of (0, 0).
        /// </summary>
        public bool IsEmpty
        {
            get { return _x == 0 && _y == 0 && _width == 0 && _height == 0; }
        }

        /// <summary>
        /// The top-left coordinates of this <see cref="Rectangle"/>.
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
        /// The width-height coordinates of this <see cref="Rectangle"/>.
        /// </summary>
        public object Size
        {
            get
            {
                return new Vector2(_width, _height);
            }
            set
            {
                _width = ((Vector2)value).X;
                _height = ((Vector2)value).Y;
            }
        }

        /// <summary>
        /// A <see cref="Vector2"/> located in the center of this <see cref="Rectangle"/>.
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return new Vector2(_x + (_width / 2), _y + (_height / 2));
            }
        }

        #endregion Public Properties

        #region Constructors
        
        /// <summary>
        /// Creates a new instance of <see cref="Rectangle"/> struct, with the specified
        /// position, width, and height.
        /// </summary>
        /// <param name="x">The x coordinate of the top-left corner of the created <see cref="Rectangle"/>.</param>
        /// <param name="y">The y coordinate of the top-left corner of the created <see cref="Rectangle"/>.</param>
        /// <param name="width">The width of the created <see cref="Rectangle"/>.</param>
        /// <param name="height">The height of the created <see cref="Rectangle"/>.</param>
        public Rectangle(float x, float y, float width, float height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Rectangle"/> struct, with the specified
        /// location and size.
        /// </summary>
        /// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="Rectangle"/>.</param>
        /// <param name="size">The width and height of the created <see cref="Rectangle"/>.</param>
        public Rectangle(Vector2 location, Vector2 size)
        {
            _x = location.X;
            _y = location.Y;
            _width = size.X;
            _height = size.Y;
        }

        #endregion Constructors

        #region Operators

        /// <summary>
        /// Compares whether two <see cref="Rectangle"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="Rectangle"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="Rectangle"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return ((a._x == b._x) && (a._y == b._y) && (a._width == b._width) && (a._height == b._height));
        }

        /// <summary>
        /// Compares whether two <see cref="Rectangle"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="Rectangle"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="Rectangle"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }

        #endregion Operators

        #region Public Methods

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="Rectangle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(int x, int y)
        {
            return ((((this._x <= x) && (x < (this._x + this._width))) && (this._y <= y)) && (y < (this._y + this._height)));
        }

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="Rectangle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(float x, float y)
        {
            return ((((this._x <= x) && (x < (this._x + this._width))) && (this._y <= y)) && (y < (this._y + this._height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Rectangle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="Rectangle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Point value)
        {
            return ((((this._x <= value.X) && (value.X < (this._x + this._width))) && (this._y <= value.Y)) && (value.Y < (this._y + this._height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Rectangle"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="Rectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref Point value, out bool result)
        {
            result = ((((this._x <= value.X) && (value.X < (this._x + this._width))) && (this._y <= value.Y)) && (value.Y < (this._y + this._height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Rectangle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="Rectangle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Vector2 value)
        {
            return ((((this._x <= value.X) && (value.X < (this._x + this._width))) && (this._y <= value.Y)) && (value.Y < (this._y + this._height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Rectangle"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="Rectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref Vector2 value, out bool result)
        {
            result = ((((this._x <= value.X) && (value.X < (this._x + this._width))) && (this._y <= value.Y)) && (value.Y < (this._y + this._height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="IShape"/> lies within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="value">The <see cref="IShape"/> to check for inclusion in this <see cref="Rectangle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="IShape"/>'s bounds lie entirely inside this <see cref="Rectangle"/>; <c>false</c> otherwise.</returns>
        public bool Contains(IShape value)
        {
            if (value is Rectangle)
            {
                return ((((this._x <= ((Rectangle)value).X) && ((((Rectangle)value).X + ((Rectangle)value).Width) <= (this._x + this._width)))
                    && (this._y <= ((Rectangle)value).Y)) && ((((Rectangle)value).Y + ((Rectangle)value).Height) <= (this._y + this._height)));
            }
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Rectangle"/> lies within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="value">The <see cref="Rectangle"/> to check for inclusion in this <see cref="Rectangle"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Rectangle"/>'s bounds lie entirely inside this <see cref="Rectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref IShape value, out bool result)
        {
            if (value is Rectangle)
            {
                result = ((((this._x <= ((Rectangle)value).X) && ((((Rectangle)value).X + ((Rectangle)value).Width) <= (this._x + this._width)))
                    && (this._y <= ((Rectangle)value).Y)) && ((((Rectangle)value).Y + ((Rectangle)value).Height) <= (this._y + this._height)));
            }
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
            return (obj is Rectangle) && this == ((Rectangle)obj);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="other">The <see cref="Rectangle"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Rectangle other)
        {
            return this == other;
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Rectangle"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Rectangle"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + _x.GetHashCode();
                hash = hash * 23 + _y.GetHashCode();
                hash = hash * 23 + _width.GetHashCode();
                hash = hash * 23 + _height.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Adjusts the edges of this <see cref="Rectangle"/> by specified horizontal and vertical amounts. 
        /// </summary>
        /// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
        /// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
        public void Inflate(int horizontalAmount, int verticalAmount)
        {
            _x -= horizontalAmount;
            _y -= verticalAmount;
            _width += horizontalAmount * 2;
            _height += verticalAmount * 2;
        }

        /// <summary>
        /// Adjusts the edges of this <see cref="Rectangle"/> by specified horizontal and vertical amounts. 
        /// </summary>
        /// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
        /// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            _x -= horizontalAmount;
            _y -= verticalAmount;
            _width += horizontalAmount * 2;
            _height += verticalAmount * 2;
        }

        /// <summary>
        /// Gets whether or not the other <see cref="IShape"/> intersects with this rectangle.
        /// </summary>
        /// <param name="value">The other <see cref="IShape"/> for testing.</param>
        /// <returns><c>true</c> if other <see cref="IShape"/> intersects with this rectangle; <c>false</c> otherwise.</returns>
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
        /// Gets whether or not the other <see cref="IShape"/> intersects with this rectangle.
        /// </summary>
        /// <param name="value">The other <see cref="IShape"/> for testing.</param>
        /// <param name="result"><c>true</c> if other <see cref="IShape"/> intersects with this rectangle; <c>false</c> otherwise. As an output parameter.</param>
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
        /// Creates a new <see cref="Rectangle"/> that contains overlapping region of two other rectangles.
        /// </summary>
        /// <param name="value1">The first <see cref="Rectangle"/>.</param>
        /// <param name="value2">The second <see cref="Rectangle"/>.</param>
        /// <returns>Overlapping region of the two rectangles.</returns>
        public static Rectangle Intersect(Rectangle value1, Rectangle value2)
        {
            Rectangle rectangle;
            Intersect(ref value1, ref value2, out rectangle);
            return rectangle;
        }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/> that contains overlapping region of two other rectangles.
        /// </summary>
        /// <param name="value1">The first <see cref="Rectangle"/>.</param>
        /// <param name="value2">The second <see cref="Rectangle"/>.</param>
        /// <param name="result">Overlapping region of the two rectangles as an output parameter.</param>
        public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
        {
            if (value1.Intersects(value2))
            {
                float right_side = Math.Min(value1._x + value1._width, value2._x + value2._width);
                float left_side = Math.Max(value1._x, value2._x);
                float top_side = Math.Max(value1._y, value2._y);
                float bottom_side = Math.Min(value1._y + value1._height, value2._y + value2._height);
                result = new Rectangle(left_side, top_side, right_side - left_side, bottom_side - top_side);
            }
            else
            {
                result = new Rectangle(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="Rectangle"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="Rectangle"/>.</param>
        public void Offset(int offsetX, int offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="Rectangle"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="Rectangle"/>.</param>
        public void Offset(float offsetX, float offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="Rectangle"/>.</param>
        public void Offset(Point amount)
        {
            _x += amount.X;
            _y += amount.Y;
        }

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="Rectangle"/>.</param>
        public void Offset(Vector2 amount)
        {
            _x += amount.X;
            _y += amount.Y;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Rectangle"/> in the format:
        /// {X:[<see cref="_x"/>] Y:[<see cref="_y"/>] Width:[<see cref="_width"/>] Height:[<see cref="_height"/>]}
        /// </summary>
        /// <returns><see cref="string"/> representation of this <see cref="Rectangle"/>.</returns>
        public override string ToString()
        {
            return "{X:" + _x + " Y:" + _y + " Width:" + _width + " Height:" + _height + "}";
        }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/> that completely contains two other rectangles.
        /// </summary>
        /// <param name="value1">The first <see cref="Rectangle"/>.</param>
        /// <param name="value2">The second <see cref="Rectangle"/>.</param>
        /// <returns>The union of the two rectangles.</returns>
        public static Rectangle Union(Rectangle value1, Rectangle value2)
        {
            float x = Math.Min(value1._x, value2._x);
            float y = Math.Min(value1._y, value2._y);
            return new Rectangle(x, y,
                                 Math.Max(value1.Right, value2.Right) - x,
                                     Math.Max(value1.Bottom, value2.Bottom) - y);
        }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/> that completely contains two other rectangles.
        /// </summary>
        /// <param name="value1">The first <see cref="Rectangle"/>.</param>
        /// <param name="value2">The second <see cref="Rectangle"/>.</param>
        /// <param name="result">The union of the two rectangles as an output parameter.</param>
        public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
        {
            result._x = Math.Min(value1._x, value2._x);
            result._y = Math.Min(value1._y, value2._y);
            result._width = Math.Max(value1.Right, value2.Right) - result._x;
            result._height = Math.Max(value1.Bottom, value2.Bottom) - result._y;
        }

        #endregion Public Methods
    }
}
