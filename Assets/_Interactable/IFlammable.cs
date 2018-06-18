namespace Randolph.Interactable {
    internal interface IFlammable {
        InventoryItem GetBurningVersion();
        void Ignite();
    }
}
