namespace Randolph.Levels {
    /// <summary>Objects which should be reverted to their initial state upon returning to a checkpoint.</summary>
    public interface IRestartable {
        /// <summary>Restore the initial settings and position of an object.</summary>
        void Restart();
    }
}