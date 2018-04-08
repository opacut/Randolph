using Randolph.Levels;

namespace Randolph.Characters {
    /// <summary>Any character/object hostile to the player.</summary>
    public interface IEnemy : IRestartable {
        /// <summary>Kills the enemy.</summary>
        void Kill();
    }
}