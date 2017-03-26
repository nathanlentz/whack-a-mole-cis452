using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen
{
    [Serializable]
    public abstract class Component
    {
        protected GameObject _object;
        /// <summary>
        /// The <see cref="GameObject"/> to which this <see cref="Component"/> belongs.
        /// </summary>
        public GameObject Object
        {
            get { return _object; }
        }

        public Component(GameObject gameObject)
        {
            _object = gameObject;
            _object.AddComponent(this);
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
