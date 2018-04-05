using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Randolph.Interactable;

namespace Randolph.UI {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(LayoutElement))]
    public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        Vector2 positionToReturnTo;
        Inventory inventory;
        InventoryItem item;

        public InventoryItem Item {
            get { return item; }
        }

        public void Init(Inventory inventory, InventoryItem item) {
            this.inventory = inventory;
            this.item = item;

            GetComponent<Image>().sprite = item.icon;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            positionToReturnTo = transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            GetComponent<LayoutElement>().ignoreLayout = true;
        }

        public void OnDrag(PointerEventData eventData) {
            transform.position = Input.mousePosition;

            GetComponent<Image>().sprite = (FindApplicableTarget()) ? item.iconOK : item.iconNOK;
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
