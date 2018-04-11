namespace Randolph.Levels {
    public interface IPickable : IRestartable {

        void OnPick();
        bool isSingleUse { get; }

    }
}
