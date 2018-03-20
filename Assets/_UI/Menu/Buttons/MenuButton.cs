using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Randolph.UI {
    /// <summary>Handles swapping sprites of the button and text size, not the actual action on click.</summary>
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

        // Image change
        [SerializeField] Sprite defaultSprite;
        [SerializeField] Sprite hoverSprite;
        [SerializeField] Sprite clickSprite;

        // Text color change
        [SerializeField] Color defaultColor = Color.white;
        [SerializeField] Color hoverColor = Color.white;
        [SerializeField] Color clickColor = Color.white;

        // Mouse
        bool over;
        bool click;

        // Cached
        Button thisButton;
        Image buttonImage;
        Text buttonText;
        //const float TextSizeRatio = 35 / 300.0f;

        void Start() {
            thisButton = gameObject.GetComponent<Button>();
            buttonImage = gameObject.GetComponent<Image>();
            buttonText = gameObject.GetComponentInChildren<Text>();

            // Text size appropriate to the resolution
            // e.g. width 300 --> 40 pts
            //buttonText.fontSize = (int) (((RectTransform) transform).rect.width * TextSizeRatio);            
        }

        public void OnPointerEnter(PointerEventData eventData) {
            over = true;
            if (thisButton.interactable) {
                buttonImage.sprite = hoverSprite;
                buttonText.color = hoverColor;
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            over = false;
            if (thisButton.interactable) {
                if (!click) {
                    // Leave the button without clicking it
                    buttonImage.sprite = defaultSprite;
                    buttonText.color = defaultColor;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            click = true;
            if (thisButton.interactable) {
                buttonImage.sprite = clickSprite;
                buttonText.color = clickColor;
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            click = false;
            if (thisButton.interactable) {
                if (over) {
                    // Click; technically not necessary, you immediately pass to the next scene
                    buttonImage.sprite = hoverSprite;
                    buttonText.color = hoverColor;                    
                } else {
                    // Holding the button down and moving outside
                    buttonImage.sprite = defaultSprite;
                    buttonText.color = defaultColor;
                }
            }
        }
    }

}