using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Randolph.UI {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(LayoutElement))]
    public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        private Image image;
        private Material initialMaterial;

        private Inventory inventory;

        [SerializeField] private Material outOfReachMaterial;
        private Vector2 position;
        public InventoryItem item { get; private set; }

        public void OnBeginDrag(PointerEventData eventData) {
            position = transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            GetComponent<LayoutElement>().ignoreLayout = true;
        }

        public void OnDrag(PointerEventData eventData) {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (outOfReachMaterial && !IsCursorWithinApplicableDistance(mousePosition) && !IsCursoreAboveAnotherItem(eventData)) {
                // Too far to apply the item -> set an appropriate material
                image.material = outOfReachMaterial;
            } else if (image.material != initialMaterial) {
                // Can apply the item
                image.material = initialMaterial;
            }
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }

        public void OnEndDrag(PointerEventData eventData) {
            var target = FindApplicableTarget() ?? eventData?.pointerCurrentRaycast.gameObject?.GetComponent<InventoryIcon>()?.item.gameObject;
            if (target) {
                inventory.ApplyTo(item, target);
            }

            transform.position = position;
            image.material = initialMaterial;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            GetComponent<LayoutElement>().ignoreLayout = false;
        }

        public void Init(Inventory inventory, InventoryItem item) {
            this.inventory = inventory;
            this.item = item;

            image = GetComponent<Image>();
            initialMaterial = image.material;
            image.sprite = item.icon;
            image.preserveAspect = true;
        }

        private bool IsCursorWithinApplicableDistance(Vector2 mousePosition) { return inventory.IsWithinApplicableDistance(mousePosition); }

        private bool IsCursoreAboveAnotherItem(PointerEventData eventData) { return eventData.pointerCurrentRaycast.gameObject?.GetComponent<InventoryIcon>() ?? false; }

        private GameObject FindApplicableTarget() {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            foreach (var hit in Physics2D.RaycastAll(ray.origin, ray.direction)) {
                if (inventory.IsApplicableTo(item, hit.collider.gameObject)) {
                    return hit.collider.gameObject;
                }
            }
            
            // TODO Move to better spot and improve with more describing response
            Constants.Randolph.ShowDescriptionBubble("I can't do that.", 2.5f);
            return null;
        }
    }
}
