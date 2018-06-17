namespace Randolph.Levels {
    public interface IPickable : IRestartable {

        void Pick();
        bool IsSingleUse { get; }

    }
}
