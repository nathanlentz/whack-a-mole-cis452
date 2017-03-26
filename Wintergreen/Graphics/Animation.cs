using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wintergreen.Graphics
{
    public class Animation
    {
        public Animation()
        {
            _frames = new List<Frame>();
        }
        protected Texture2D _spriteSheet;
        /// <summary>
        /// The Sprite Sheet used by the Animation.
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return _spriteSheet; }
            set { _spriteSheet = value; }
        }

        protected bool _looping;
        /// <summary>
        /// Whether the Animation loops upon finishing.
        /// </summary>
        public bool Looping
        {
            get { return _looping; }
            set { _looping = value; }
        }

        protected List<Frame> _frames;
        /// <summary>
        /// The Frames of the Animation.
        /// </summary>
        public List<Frame> Frames
        {
            get { return _frames; }
            set { _frames = value; }
        }

        protected int _currentFrame = 0;
        /// <summary>
        /// The index of the current frame.
        /// </summary>
        public int CurrentFrame
        {
            get { return _currentFrame; }
            set { _currentFrame = value; }
        }

        protected float _currentFrameTimePassed = 0;
        /// <summary>
        /// How long the current frame has been drawn.
        /// </summary>
        public float CurrentFrameTimePassed
        {
            get { return _currentFrameTimePassed; }
            set { _currentFrameTimePassed = value; }
        }

        // TODO: Animation Update method.
        public void Update(GameTime gameTime)
        {
            // increase the currentFrameTimePassed
            _currentFrameTimePassed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            // if it's now bigger than the current frame's duration, increment the currentFrame
            if (_currentFrameTimePassed > _frames.ElementAt(_currentFrame).Duration)
            {
                _currentFrame++;
                _currentFrameTimePassed = 0;
            }
            // If Looping is true, restart the animation when it ends.
            if (_looping && _currentFrame == _frames.Count)
            {
                _currentFrame = 0;
            }
            else if (_currentFrame == _frames.Count)
            {
                _currentFrame = -1;
            }
        }
    }
}
