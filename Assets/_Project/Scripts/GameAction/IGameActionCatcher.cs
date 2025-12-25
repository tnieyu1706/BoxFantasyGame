using System;

namespace Systems.GameAction
{
    public interface IGameActionCatcher
    {
        Action OnCompleted { get; set; }
        
        /// <summary>
        /// Func to (check detail of command) data is valid
        /// Call after Format data completed
        /// </summary>
        /// <param name="actionCommand"></param>
        /// <returns></returns>
        bool Catch(GameActionCommand actionCommand);
        
        /// <summary>
        /// Func to (format struct of command) data is valid
        /// </summary>
        /// <param name="actionCommand"></param>
        /// <returns></returns>
        bool Format(GameActionCommand actionCommand);
    }
}