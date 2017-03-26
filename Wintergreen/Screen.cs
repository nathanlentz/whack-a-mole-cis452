using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Wintergreen.Interfaces;

namespace Wintergreen
{
    /// <summary>
    /// Screen will hold a list of UI Elements
    /// </summary>
    public class Screen
    {
        private List<UIElement> _UIElements;
        private IState _state;
        private GameScene _gameScene;
        
        public Screen(IState state, GameScene scene)
        {
            _state = state;
            _gameScene = scene;
            _UIElements = new List<UIElement>();
        }


        /// <summary>
        /// Return a list of all UI Elements in this screen
        /// </summary>
        /// <returns></returns>
        public List<UIElement> GetUIElements()
        {
            if (_UIElements.Count > 0)
            {
                return _UIElements;
            }

            return new List<UIElement>();
        }

        /// <summary>
        /// Add a UI Element to the current scene
        /// </summary>
        /// <param name="element"></param>
        public void AddUIElement(UIElement element)
        {
            if(element != null)
            {
                _UIElements.Add(element);
            }
        }

        /// <summary>
        /// Remove a UI Element from the current scene
        /// </summary>
        /// <param name="element"></param>
        public void RemoveUIElement(UIElement element)
        {
            if(element != null)
            {
                _UIElements.Remove(element);
            }
        }


        /// <summary>
        /// Remove all UIElements in this scene
        /// </summary>
        public void RemoveAllUIElements()
        {
            _UIElements = null;
        }

        /// <summary>
        /// Update UI Elements and update to current scene
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, GameScene scene)
        {
            _gameScene = scene;

            foreach(var element in _UIElements)
            {
                element.Update(gameTime, scene);
            }
        }

        /// <summary>
        /// Draw UI Elements
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var element in _UIElements)
            {
                element.Draw(spriteBatch);
            }
        }

    }
}
