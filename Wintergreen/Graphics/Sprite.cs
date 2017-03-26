using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Wintergreen.Interfaces;

namespace Wintergreen.Graphics
{
    public class Sprite : Component
    {
        protected Texture2D _spriteSheet;
        /// <summary>
        /// The Sprite Sheet on which the Sprite is located
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return _spriteSheet; }
            set { _spriteSheet = value; }
        }

        protected Point _frameSize;
        /// <summary>
        /// The size in pixels of the Sprite on the Sprite Sheet
        /// </summary>
        public Point FrameSize
        {
            get { return _frameSize; }
            set { _frameSize = value; }
        }

        protected Point _frame;
        /// <summary>
        /// The Sprite Sheet coordinates of the sprite
        /// </summary>
        public Point Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }

        protected Vector2 _offset;
        /// <summary>
        /// A point relative to the <see cref="GameObject"/>'s location where the sprite is to be drawn.
        /// </summary>
        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        protected Color _tint = Color.White;
        /// <summary>
        /// The color to tint the sprite.
        /// White represents no tint.
        /// </summary>
        public Color Tint
        {
            get { return _tint; }
            set { _tint = value; }
        }

        protected float _scale = 1.0f;
        /// <summary>
        /// The scale at which the sprite is drawn.
        /// 1.0 is normal scale.
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        protected SpriteEffects _effects;
        /// <summary>
        /// A SpriteEffects enum which can flip the sprite vertically or horizontally.
        /// </summary>
        public SpriteEffects Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }

        protected bool _visible = true;
        /// <summary>
        /// Whether the sprite should be drawn
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// The part of the spritesheet we want to draw
        /// </summary>
        public Rectangle drawRec = new Rectangle(0, 0, 0, 0);

        public Sprite(GameObject gameObject) : base(gameObject)
        {
            _gameObject = gameObject;
            this.position = _gameObject.Location;
        }
        public Color drawColor = Color.White;   //used to tint the sprite a specific color, white = no tint
        public float scale = 1.0f;
        public float rotation = 0.0f;
        public SpriteEffects spriteEffect = SpriteEffects.None; //used to flip the sprite horizontally or vertically
        public float alpha = 1.0f;          //controls the opacity of sprite
        public Vector2 origin = new Vector2(0, 0);  //the pivot point/center point of sprite
        public float zDepth = 0.01f;    //the 'layer' this sprite is drawn on, based on Yposition
        public Vector2 position;
        private GameObject _gameObject;

        public virtual void Draw()
        {
            if (Visible)
            {   //set draw rec, draw sprite
                drawRec.Width = FrameSize.X;
                drawRec.Height = FrameSize.Y;
                drawRec.X = (FrameSize.X * Frame.X) + (int)Offset.X;
                drawRec.Y = (FrameSize.Y * Frame.Y) + (int)Offset.Y;
                GraphicsManager.SpriteBatch.Draw(SpriteSheet, position, drawRec, drawColor * alpha);
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.position = _gameObject.Location;
        }
    }
}
