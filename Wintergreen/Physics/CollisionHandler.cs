using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintergreen;

namespace Wintergreen.Physics
{
    public abstract class CollisionHandler
    {
        protected HashSet<Hitbox> _hitboxes;
        /// <summary>
        /// A <see cref="HashSet{Hitbox}"/> of every <see cref="Hitbox"/> within the <see cref="GameScene"/>.
        /// </summary>
        public HashSet<Hitbox> Hitboxes
        {
            get { return _hitboxes; }
        }

        protected HashSet<GameObject> _movableObjects;
        /// <summary>
        /// A <see cref="HashSet{Hitbox}"/> of every <see cref="Hitbox"/> within the <see cref="GameScene"/> that has a <see cref="Movement"/> <see cref="Component"/>.
        /// </summary>
        public HashSet<GameObject> MovableObjects
        {
            get { return _movableObjects; }
        }


        protected List<GameObject> _movingObjects;
        /// <summary>
        /// A <see cref="List{Hitbox}"/> of every <see cref="GameObject"/> that has moved this update.
        /// </summary>
        public List<GameObject> MovingObjects
        {
            get { return _movingObjects; }
        }

        public CollisionHandler()
        {
            _hitboxes = new HashSet<Hitbox>();
            _movableObjects = new HashSet<GameObject>();
            _movingObjects = new List<GameObject>();
        }

        public void Update()
        {
            IdentifyMovingObjects();
            DetectCollisions();
        }

        private void IdentifyMovingObjects()
        {
            _movingObjects.Clear();
            foreach(GameObject obj in _movableObjects)
            {
                if (obj.GetComponent<Movement>().PreviousLocation != obj.Location)
                    _movingObjects.Add(obj);
            }
        }

        private void DetectCollisions()
        {
            foreach (GameObject obj in _movingObjects)
            {
                foreach (Hitbox objHitbox in obj.GetComponents<Hitbox>())
                {
                    foreach (Hitbox hitbox in _hitboxes)
                    {
                        if (objHitbox.IsIntersecting(hitbox) && objHitbox.Object != hitbox.Object)
                            HandleCollision(objHitbox, hitbox);
                    }
                }
            }
        }

        protected abstract void HandleCollision(Hitbox movingHitbox, Hitbox collidedHitbox);
    }
}
