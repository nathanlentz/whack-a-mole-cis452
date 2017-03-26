using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen.Physics
{
    public interface IShape
    {
        #region Public Properties

        /// <summary>
        /// The x coordinate of the leftmost point of this <see cref="IShape"/>.
        /// </summary>
        float X { get; set; }

        /// <summary>
        /// The y coordinate of the leftmost point of this <see cref="IShape"/>.
        /// </summary>
        float Y { get; set; }

        /// <summary>
        /// Returns the x coordinate of the left side of this <see cref="IShape"/>.
        /// </summary>
        float Left { get; }

        /// <summary>
        /// Returns the x coordinate of the right side of this <see cref="IShape"/>.
        /// </summary>
        float Right { get; }

        /// <summary>
        /// Returns the y coordinate of the top side of this <see cref="IShape"/>.
        /// </summary>
        float Top { get; }

        /// <summary>
        /// Returns the y coordinate of the bottom side of this <see cref="IShape"/>.
        /// </summary>
        float Bottom { get; }

        /// <summary>
        /// The top-left coordinates of this <see cref="IShape"/>.
        /// </summary>
        Vector2 Location { get; set; }

        /// <summary>
        /// A <see cref="Vector2"/> located in the center of this <see cref="IShape"/>.
        /// </summary>
        Vector2 Center { get; }

        /// <summary>
        /// A <see cref="object"/> representing the size metric of this <see cref="IShape"/>.
        /// </summary>
        object Size { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="IShape"/>; <c>false</c> otherwise.</returns>
        bool Contains(int x, int y);

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="IShape"/>; <c>false</c> otherwise.</returns>
        bool Contains(float x, float y);

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="IShape"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="IShape"/>; <c>false</c> otherwise.</returns>
        bool Contains(Point value);

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="IShape"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="IShape"/>; <c>false</c> otherwise. As an output parameter.</param>
        void Contains(ref Point value, out bool result);

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="IShape"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="IShape"/>; <c>false</c> otherwise.</returns>
        bool Contains(Vector2 value);

        /// <summary>
        /// Gets whether or not the provided <see cref="IShape"/> lies within the bounds of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="IShape"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="IShape"/> lies inside this <see cref="IShape"/>; <c>false</c> otherwise. As an output parameter.</param>
        void Contains(ref Vector2 value, out bool result);

        /// <summary>
        /// Gets whether or not the provided <see cref="IShape"/> lies within the bounds of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="value">The <see cref="IShape"/> to check for inclusion in this <see cref="IShape"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="IShape"/>'s bounds lie entirely inside this <see cref="IShape"/>; <c>false</c> otherwise.</returns>
        bool Contains(IShape value);

        /// <summary>
        /// Gets whether or not the provided <see cref="IShape"/> lies within the bounds of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="value">The <see cref="IShape"/> to check for inclusion in this <see cref="IShape"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="IShape"/>'s bounds lie entirely inside this <see cref="IShape"/>; <c>false</c> otherwise. As an output parameter.</param>
        void Contains(ref IShape value, out bool result);

        /// <summary>
        /// Gets whether or not the other <see cref="IShape"/> intersects with this <see cref="IShape"/> .
        /// </summary>
        /// <param name="value">The other <see cref="IShape"/>  for testing.</param>
        /// <returns><c>true</c> if other <see cref="IShape"/> intersects with this <see cref="IShape"/> ; <c>false</c> otherwise.</returns>
        bool Intersects(IShape value);

        /// <summary>
        /// Gets whether or not the other <see cref="IShape"/> intersects with this <see cref="IShape"/> .
        /// </summary>
        /// <param name="value">The other <see cref="IShape"/>  for testing.</param>
        /// <param name="result"><c>true</c> if other <see cref="IShape"/> intersects with this <see cref="IShape"/> ; <c>false</c> otherwise. As an output parameter.</param>
        void Intersects(ref IShape value, out bool result);

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="IShape"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="IShape"/>.</param>
        void Offset(int offsetX, int offsetY);

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="IShape"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="IShape"/>.</param>
        void Offset(float offsetX, float offsetY);

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="IShape"/>.</param>
        void Offset(Point amount);

        /// <summary>
        /// Changes the <see cref="Location"/> of this <see cref="IShape"/>.
        /// </summary>
        /// <param name="amount">The x and y components to add to this <see cref="IShape"/>.</param>
        void Offset(Vector2 amount);

        #endregion Public Methods
    }
}
