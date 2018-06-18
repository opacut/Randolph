namespace Randolph.Levels {
    /// <summary>Objects which should be reverted to their initial state upon returning to a checkpoint.</summary>
    public interface IRestartable {
        /// <summary>Saves preferred state to restore.</summary>
        void SaveState();
        /// <summary>Restore the saved state of an object.</summary>
        void Restart();
    }
}