using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wintergreen.Physics
{
    public class Movement : Component
    {
        protected Vector2 _direction = new Vector2();
        /// <summary>
        /// A Unit Vector representing the movement direction of the <see cref="GameObject"/>
        /// </summary>
        public virtual Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        protected Vector2 _previousLocation;
        /// <summary>
        /// The previous location of the <see cref="GameObject"/>.
        /// </summary>
        public virtual Vector2 PreviousLocation
        {
            get { return _previousLocation; }
        }

        protected float _speed = 0;
        /// <summary>
        /// The speed with which the <see cref="GameObject"/> is moving.
        /// </summary>
        public virtual float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        protected float _targetSpeed = 0;
        /// <summary>
        /// The speed to which the <see cref="GameObject"/> is accelerating.
        /// </summary>
        public virtual float TargetSpeed
        {
            get { return _targetSpeed; }
            set { _targetSpeed = value; }
        }

        protected float _accelerationRate = 0;
        /// <summary>
        /// The change in speed to apply with each update
        /// </summary>
        /// <param name="gameObject"></param>
        public virtual float AccelerationRate
        {
            get { return _accelerationRate; }
            set { _accelerationRate = value; }
        }

        public Movement(GameObject gameObject) : base(gameObject)
        {
        }

        public override sealed void Update(GameTime gameTime)
        {
            UpdateSpeed(gameTime);
            UpdateLocation(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Set's the <see cref="GameObject"/>'s new location based on its velocity.
        /// </summary>
        protected virtual void UpdateLocation(GameTime gameTime)
        {
            int elapsedTime = EngineSettings.TimeBasedUpdating ? gameTime.ElapsedGameTime.Milliseconds : 1;

            // Set the GameObject's new location.
            Vector2 newLocation = _previousLocation = Object.Location;
            newLocation.X += Direction.X * Speed * elapsedTime;
            newLocation.Y += Direction.Y * Speed * elapsedTime;
            Object.Location = newLocation;

            float dirX = 0;
            float dirY = 0;
            if (Direction.X > 0)
                dirX = Direction.X - AccelerationRate;
            if (Direction.Y > 0)
                dirY = Direction.Y - AccelerationRate;
            if (Direction.X < 0)
                dirX = Direction.X + AccelerationRate;
            if (Direction.Y < 0)
                dirY = Direction.Y + AccelerationRate;
            Direction = new Vector2(dirX, dirY);
        }

        /// <summary>
        /// Set's the <see cref="GameObject"/>'s speed based on its acceleration and target speed.
        /// </summary>
        protected virtual void UpdateSpeed(GameTime gameTime)
        {
            // Object is already at its target speed.
            if (Speed == TargetSpeed)
                return;

            int elapsedTime = EngineSettings.TimeBasedUpdating ? gameTime.ElapsedGameTime.Milliseconds : 1;

            if (TargetSpeed < Speed) // Negative Acceleration
            {
                // Acceleration Rate is Absolute Valued to prevent a Target Speed and Acceleration rate being in the wrong direction from each other.
                // This may change later
                Speed = Math.Max(TargetSpeed, Speed - Math.Abs(AccelerationRate * elapsedTime));
            }

            else // Positive Acceleration
            {
                Speed = Math.Min(TargetSpeed, Speed + Math.Abs(AccelerationRate * elapsedTime));
            }
        }
    }
}
