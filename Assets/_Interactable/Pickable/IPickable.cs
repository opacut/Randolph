namespace Randolph.Levels {
    public interface IPickable : IRestartable {
        bool IsSingleUse { get; }

        void Pick();
    }
}
