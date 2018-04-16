using Randolph.Interactable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Randolph.UI {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(LayoutElement))]
    public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        [SerializeField] Material outOfReachMaterial;
        Material initialMaterial;
        Image image;

        Inventory inventory;
        Vector2 position;
        public InventoryItem item { get; private set; }

        public void Init(Inventory inventory, InventoryItem item) {
            this.inventory = inventory;
            this.item = item;

            this.image = GetComponent<Image>();
            initialMaterial = image.material;            
            image.sprite = item.icon;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            position = transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            GetComponent<LayoutElement>().ignoreLayout = true;
        }

        public void OnDrag(PointerEventData eventData) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (outOfReachMaterial && !IsCursorWithinApplicableDistance(mousePosition)) {
                // Too far to apply the item -> set an appropriate material
                image.material = outOfReachMaterial;
            } else if (image.material != initialMaterial) {
                // Can apply the item
                image.material = initialMaterial;
            }
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }

        public void OnEndDrag(PointerEventData eventData) {
            GameObject target = FindApplicableTarget();
            if (target) {
                inventory.ApplyTo(item, target);
            }

            transform.position = position;
            image.material = initialMaterial;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            GetComponent<LayoutElement>().ignoreLayout = false;
        }

        bool IsCursorWithinApplicableDistance(Vector2 mousePosition) {
            return inventory.IsWithinApplicableDistance(mousePosition);
        }

        GameObject FindApplicableTarget() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            foreach (RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction)) {
                if (inventory.IsApplicableTo(item, hit.collider.gameObject)) return hit.collider.gameObject;
            }

            return null;
        }

    }
}
