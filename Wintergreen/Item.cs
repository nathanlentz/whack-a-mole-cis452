using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen
{
    public abstract class Item : GameObject
    {
        public Item(GameScene scene, Vector2 location) : base(scene, location)
        {
        }

        /// <summary>
        /// A method to be called when this <see cref="Item"/> is picked up.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> which picked up this <see cref="Item"/>.</param>
        public abstract void OnItemPickUp(GameObject gameObject);

        /// <summary>
        /// A method to be called when this <see cref="Item"/> is dropped.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> which dropped this <see cref="Item"/>.</param>
        public abstract void OnItemDrop(GameObject gameObject);
    }
}
