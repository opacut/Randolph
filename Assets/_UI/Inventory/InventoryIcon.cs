using Randolph.Interactable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Randolph.UI {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(LayoutElement))]
    public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        Vector2 positionToReturnTo;
        Inventory inventory;
        public InventoryItem item { get; private set; }

        public void Init(Inventory inventory, InventoryItem item) {
            this.inventory = inventory;
            this.item = item;

            GetComponent<Image>().sprite = item.icon;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            positionToReturnTo = transform.position;
            GetComponent<Image>().sprite = item.icon;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            GetComponent<LayoutElement>().ignoreLayout = true;
        }

        public void OnDrag(PointerEventData eventData) {
            var mouesPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouesPos.x, mouesPos.y, 100);
        }

        public void OnEndDrag(PointerEventData eventData) {
            GameObject target = FindApplicableTarget();
            if (target) {
                inventory.ApplyTo(item, target);
            } else {
                transform.position = positionToReturnTo;
                GetComponent<Image>().sprite = item.icon;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                GetComponent<LayoutElement>().ignoreLayout = false;
            }
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