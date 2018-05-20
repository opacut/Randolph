namespace Randolph.Levels {
    public interface IPickable : IRestartable {

        void OnPick();
        bool IsSingleUse { get; }

    }
}
