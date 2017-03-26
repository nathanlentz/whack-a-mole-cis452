using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wintergreen
{
    // Not sure if we'll need this or not
    public enum UIElementType
    {
        Menu,
        StatusBar
    }

    /// <summary>
    /// This is the base class for all UI Elements
    /// UIElements include Menus, Status Bars, Maps, etc
    /// </summary>
    public abstract class UIElement
    {
        protected Vector2 _location;
        protected ContentManager _content;
        protected GameScene _gameScene;


        public UIElementType UIElementType
        {
            get
            {
                return UIElementType;
            }
            set
            {
               UIElementType = value;

            }
        }

        /// <summary>
        /// Constructs a UI Element with the specified location
        /// </summary>
        /// <param name="location"></param>
        public UIElement(Vector2 location, ContentManager content, GameScene scene)
        {
            _location = location;
            _gameScene = scene;
            _content = new ContentManager(content.ServiceProvider, content.RootDirectory);
        }

        /// <summary>
        /// The location of the UIElement
        /// </summary>
        public Vector2 Location
        {
            get { return _location;  }
            set { _location = value; }
        }

        /// <summary>
        /// Update properties in UI Element
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime, GameScene scene);


        /// <summary>
        /// Draw method for a UIElement
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Check if current UIElement is null
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool isNullOrEmpty(UIElement element)
        {
            if (isNullOrEmpty(this))
            {
                return true;
            }

            return false;
        }
        
    }


}
