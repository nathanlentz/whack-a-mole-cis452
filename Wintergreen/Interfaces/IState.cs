using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wintergreen.Interfaces
{
    /// <summary>
    /// Interface for all possible game states
    /// Game State classes should be game spefic and stay within the Game Project
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Update current state and return either current instance of state or a new state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        IState UpdateState(GameTime gameTime);

        /// <summary>
        /// Queue a change of state from some event during update
        /// </summary>
        /// <param name="state"></param>
        void ChangeState(IState state);

        /// <summary>
        /// Returns an instance of the previous state. Will return current state if previous is null
        /// </summary>
        /// <returns></returns>
        IState GetPreviousState();

        /// <summary>
        /// Typically, this will be used to draw any content appearing from previous states
        /// Call DrawUI from the previous state to accomplish this
        /// </summary>
        /// <param name="spriteBatch"></param>
        void DrawContent(SpriteBatch spriteBatch);

        /// <summary>
        /// Call draw on any <see cref="GameObject"/>s directly associated with this state
        /// </summary>
        /// <param name="spriteBatch"></param>
        void DrawUI(SpriteBatch spriteBatch);
    }
}
