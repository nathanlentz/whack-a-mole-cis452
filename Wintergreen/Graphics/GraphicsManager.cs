using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wintergreen.Graphics
{
    public static class GraphicsManager
    {
        private static GraphicsDeviceManager _graphicsDeviceManager;
        public static GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return _graphicsDeviceManager; }
        }

        public static GraphicsDevice GraphicsDevice
        {
            get { return GraphicsDeviceManager.GraphicsDevice; }
        }

        public static Viewport Viewport
        {
            get { return GraphicsDevice.Viewport; }
            set { GraphicsDevice.Viewport = value; }
        }

        private static SpriteBatch _spriteBatch;
        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        private static float _xscale;
        public static float XScale
        {
            get { return _xscale; }
        }

        private static float _yscale;
        public static float YScale
        {
            get { return _yscale; }
        }

        static GraphicsManager()
        {
            _xscale = 1.0f;
            _yscale = 1.0f;
        }

        internal static void Initialize(Game game)
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(game);
            game.Window.ClientSizeChanged += OnResize;
        }

        internal static void InitializeSpriteBatch()
        {
            _spriteBatch = new SpriteBatch(GraphicsDeviceManager.GraphicsDevice);
        }

        public static void OnResize(object sender, EventArgs e)
        {

        }
    }
}
