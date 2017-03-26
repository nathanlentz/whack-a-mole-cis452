using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Wintergreen.Physics;

namespace Wintergreen
{
    [Serializable]
    public class GameScene
    {
        protected List<GameObject> _objects;
        /// <summary>
        /// A list of all of the <see cref="GameObject"/>s within this <see cref="GameScene"/>.
        /// </summary>
        public List<GameObject> Objects
        {
            get { return _objects; }
        }

        protected CollisionHandler _collisionHandler;
        /// <summary>
        /// A <see cref="Physics.CollisionHandler"/> to detect and handle collisions within this <see cref="GameScene"/>.
        /// </summary>
        public CollisionHandler CollisionHandler
        {
            get { return _collisionHandler; }
            set { _collisionHandler = value; }
        }

        public GameScene()
        {
            _objects = new List<GameObject>();
        }

        /// <summary>
        /// Creates a <see cref="GameScene"/> with the given <see cref="List{T}"/> of <see cref="GameObject"/>s and <see cref="Physics.CollisionHandler"/>.
        /// </summary>
        /// <param name="gameObjects">A <see cref="List{T}"/> of <see cref="GameObject"/>s within the <see cref="GameScene"/>.</param>
        /// <param name="collisionHandler">A <see cref="Physics.CollisionHandler"/> to handle collisions within the scene. May be null.</param>
        public GameScene(List<GameObject> gameObjects, CollisionHandler collisionHandler = null)
        {
            if (gameObjects == null)
                gameObjects = new List<GameObject>();

            if (collisionHandler == null)
            {
                _objects = gameObjects;
                return;
            }

            _collisionHandler = collisionHandler;
            foreach (GameObject obj in gameObjects)
                AddGameObject(obj);
        }

        /// <summary>
        /// Adds the <see cref="GameObject"/> to this <see cref="GameScene"/> 
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to add.</param>
        public virtual void AddGameObject(GameObject gameObject)
        {
            _objects.Add(gameObject);

            if (_collisionHandler != null)
            {
                if(gameObject.HasComponent<Hitbox>())
                {
                    // These collections are Hashsets, so these objects cannot be added multiple times.
                    foreach (Hitbox hitbox in gameObject.GetComponents<Hitbox>())
                        _collisionHandler.Hitboxes.Add(hitbox);

                    if (gameObject.HasComponent<Movement>())
                        _collisionHandler.MovableObjects.Add(gameObject);
                }
            }
        }

        /// <summary>
        /// Updates every <see cref="GameObject"/> in this <see cref="GameScene"/>.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in _objects)
                gameObject.Update(gameTime);

            if (_collisionHandler != null)
                CollisionHandler.Update();
        }

        public virtual void Draw()
        {
            foreach (GameObject gameObject in _objects)
                gameObject.Draw();
        }
    }
}
