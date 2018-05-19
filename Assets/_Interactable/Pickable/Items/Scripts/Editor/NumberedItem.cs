namespace Randolph.Interactable {
    /// <summary>A wrapper for <see cref="Item"/> to add an index.</summary>
    [System.Serializable]
    public class NumberedItem {

        public int id;
        public Item item;

        public NumberedItem(int id, Item item) {
            this.id = id;
            this.item = item;
        }

    }
}
