using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Wintergreen.ExtensionMethods;
using Wintergreen.Graphics;
using Wintergreen.Physics;

namespace Wintergreen
{
    [Serializable]
    public class GameObject
    {
        /// <summary>
        /// The <see cref="GameScene"/> to which this <see cref="GameObject"/> belongs.
        /// </summary>
        protected GameScene _scene;

        /// <summary>
        /// A <see cref="List{T}"/> of all of the <see cref="GameObject"/>'s <see cref="Component"/>s for iterating.
        /// </summary>
        protected List<Component> _components;

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> used to retrieve <see cref="Component"/>s at constant time.
        /// The <see cref="Type"/> keys are Base types of components.
        /// </summary>
        protected Dictionary<Type, List<Component>> _componentDictionary;

        private Vector2 _location;
        /// <summary>
        /// The location of the <see cref="GameObject"/>.
        /// </summary>
        public virtual Vector2 Location
        {
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>
        /// Constructs a <see cref="GameObject"/> with the speciifed location.
        /// </summary>
        /// <param name="location"></param>
        public GameObject(GameScene scene, Vector2 location)
        {
            _scene = scene;
            _location = location;
            _scene.AddGameObject(this);
            _components = new List<Component>();
            _componentDictionary = new Dictionary<Type, List<Component>>();
        }

        /// <summary>
        /// Adds a <see cref="Component"/> to this <see cref="GameObject"/>'s collections. Called only from the Component's base constructor.
        /// </summary>
        /// <param name="component"></param>
        internal void AddComponent(Component component)
        {
            // Add the component to the collections
            _components.Add(component);
            Type baseComponentType = component.GetType().BaseComponentType();
            List<Component> c;
            if (!_componentDictionary.TryGetValue(baseComponentType, out c))
                _componentDictionary[baseComponentType] = new List<Component>().AddAndReturn(component);
            else
                _componentDictionary[baseComponentType] = c.AddAndReturn(component);

            // If the scene has a collision handler, make sure it is aware of hitboxes
            if (_scene.CollisionHandler != null)
            {
                if (baseComponentType == typeof(Hitbox))
                {
                    _scene.CollisionHandler.Hitboxes.Add((Hitbox)component);
                    
                    // This collection is a Hashset, so
                    if (this.HasComponent<Movement>())
                        _scene.CollisionHandler.MovableObjects.Add(this);
                }
                else if (baseComponentType == (typeof(Movement)))
                {
                    if (this.HasComponent<Hitbox>())
                        _scene.CollisionHandler.MovableObjects.Add(this);
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="List{T}"/> of the specified <see cref="Component"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to get.</typeparam>
        /// <returns></returns>
        public virtual List<T> GetComponents<T>() where T : Component
        {
            Type baseComponentType = typeof(T).BaseComponentType();

            List<Component> componentList;
            if (!_componentDictionary.TryGetValue(baseComponentType, out componentList))
                return null;

            List<T> typedList = new List<T>(componentList.Count);
            foreach (Component component in componentList)
                typedList.Add((T)component);

            return typedList;
        }

        /// <summary>
        /// Gets the specified <see cref="Component"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to get.</typeparam>
        /// <returns></returns>
        public virtual T GetComponent<T>() where T : Component
        {
            Type baseComponentType = typeof(T).BaseComponentType();

            List<Component> componentList;
            if (!_componentDictionary.TryGetValue(baseComponentType, out componentList))
                return null;

            return (T)componentList.FirstOrDefault();
        }

        /// <summary>
        /// Checks whether this <see cref="GameObject"/> has a <see cref="Component"/> of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to check./></typeparam>
        /// <returns><c>true</c> if this <see cref="GameObject"/> has a <see cref="Component"/> of the specified type; <c>false</c> otherwise.</returns>
        public virtual bool HasComponent<T>() where T : Component
        {
            Type baseComponentType = typeof(T).BaseComponentType();
            return _componentDictionary.ContainsKey(baseComponentType);
        }

        /// <summary>
        /// Updates all of the <see cref="GameObject"/>'s <see cref="Component"/>s.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            foreach (Component c in _components)
            {
                c.Update(gameTime);
            }
        }

        public virtual void Draw()
        {
            foreach (Sprite sprite in GetComponents<Sprite>())
                sprite.Draw();
        }
    }
}