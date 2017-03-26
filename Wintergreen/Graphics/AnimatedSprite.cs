using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Wintergreen;

namespace Wintergreen.Graphics
{
    public class AnimatedSprite : Sprite
    {
        private Animation _currentAnimation;
        public Animation CurrentAnimation
        {
            get { return _currentAnimation; }
            set { _currentAnimation = value; }
        }

        public GameObject _gameObject;

        public AnimatedSprite(GameObject gameObject) : base(gameObject)
        {
            _gameObject = gameObject;
        }

        public override void Update(GameTime gameTime)
        {
            if (_currentAnimation != null)
            {
                _currentAnimation.Update(gameTime);

                // If the animation has ended, set it to null
                if (_currentAnimation.CurrentFrame == -1)
                    _currentAnimation = null;
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            if (_currentAnimation != null)
            {
                drawRec.Width = FrameSize.X;
                drawRec.Height = FrameSize.Y;
                drawRec.X = (FrameSize.X * _currentAnimation.Frames.ElementAt(_currentAnimation.CurrentFrame).SpriteSheetCoordinates.X) + (int)Offset.X;
                drawRec.Y = (FrameSize.Y * _currentAnimation.Frames.ElementAt(_currentAnimation.CurrentFrame).SpriteSheetCoordinates.Y) + (int)Offset.Y;
                GraphicsManager.SpriteBatch.Draw(SpriteSheet, position, drawRec, drawColor * alpha);
            }
            else
            {
                // No current animation. Draw the default sprite.
                base.Draw();
            }
        }
    }
}
