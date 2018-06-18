using System.Linq;
using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Randolph.UI {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(LayoutElement))]
    public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler /*, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler*/ {
        private Image image;
        private Material initialMaterial;

        private Inventory inventory;

        [SerializeField] private Material outOfReachMaterial;
        private int siblingIndex;
        public InventoryItem Item { get; private set; }

        public void OnBeginDrag(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Right) {
                return;
            }

            GetComponent<CanvasGroup>().blocksRaycasts = false;
            GetComponent<LayoutElement>().ignoreLayout = true;
            siblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Right) {
                return;
            }

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
            if (eventData.button == PointerEventData.InputButton.Right) {
                return;
            }

            // TODO improve failed application attempt response
            var target = FindApplicableInventoryItem(eventData.pointerCurrentRaycast) ?? FindApplicableTarget();
            if (target) {
                if (!inventory.ApplyTo(Item, target)) {
                    Constants.Randolph.ShowDescriptionBubble("I can't do that.", 0.5f);
                }
            } else {
                Constants.Randolph.ShowDescriptionBubble("There is nothing.", 0.5f);
            }

            transform.SetSiblingIndex(siblingIndex);
            image.material = initialMaterial;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            GetComponent<LayoutElement>().ignoreLayout = false;
        }

        public void OnPointerDown(PointerEventData eventData) {
            Debug.Log(this);
            OnMouseDownClickable?.Invoke(Item, (Constants.MouseButton) eventData.button);
        }

        public void OnPointerEnter(PointerEventData eventData) => OnMouseEnterClickable?.Invoke(Item);
        public void OnPointerExit(PointerEventData eventData) => OnMouseExitClickable?.Invoke(Item);

        public void OnPointerUp(PointerEventData eventData) => OnMouseUpClickable?.Invoke(Item, (Constants.MouseButton) eventData.button);

        public static event Clickable.MouseClickable OnMouseEnterClickable;
        public static event Clickable.MouseClickable OnMouseExitClickable;
        public static event Clickable.MouseClickableButton OnMouseDownClickable;
        public static event Clickable.MouseClickableButton OnMouseUpClickable;

        public void Init(Inventory initInventory, InventoryItem initItem) {
            inventory = initInventory;
            Item = initItem;

            image = GetComponent<Image>();
            initialMaterial = image.material;
            image.sprite = initItem.icon;
            image.preserveAspect = true;
        }

        private bool IsCursorWithinApplicableDistance(Vector2 mousePosition) => inventory.IsWithinApplicableDistance(mousePosition);

        private bool IsCursoreAboveAnotherItem(PointerEventData eventData) => eventData.pointerCurrentRaycast.gameObject?.GetComponent<InventoryIcon>() ?? false;

        private static GameObject FindApplicableTarget() {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics2D.RaycastAll(ray.origin, ray.direction)
                            .Select(x => x.collider.gameObject)
                            .FirstOrDefault(x => x.GetComponent<Clickable>());
        }

        private static GameObject FindApplicableInventoryItem(RaycastResult raycastResult) {
            var icon = raycastResult.gameObject?.GetComponent<InventoryIcon>();
            return icon ? icon.Item.gameObject : null;
        }
    }
}
