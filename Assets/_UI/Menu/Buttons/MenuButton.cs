using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	// Image change
	public Sprite defaultSprite;
	public Sprite hoverSprite;
	public Sprite clickSprite;

	// Text color change
	public Color defaultColor;
	public Color hoverColor;
	public Color clickColor;

	// Mouse
	bool over;
	bool click;

	// Cached
	Button thisButton;
	Image buttonImage;
	Text buttonText;
	float textSizeRatio = 40/300.0f;

	void Start () {
		thisButton = gameObject.GetComponent<Button>();
		buttonImage = gameObject.GetComponent<Image>();
		buttonText = gameObject.GetComponentInChildren<Text>();

		// Text size appropriate to the resolution
		// e.g. width 300 --> 40 pts
		buttonText.fontSize = (int) ((gameObject.transform as RectTransform).rect.width * textSizeRatio);
	    
        /*
		if (gameObject.name == "Continue Button" && (!PlayerPrefs.HasKey(LevelManager.levelKey) || PlayerPrefs.GetInt(LevelManager.levelKey) == 0)) {
			// brand new game, turn off continue
			thisButton.interactable = false;
			buttonText.color /= 1.5f;
		}
        */
	}

	public void OnPointerEnter (PointerEventData eventData) {
		over = true;
		if (thisButton.interactable) {			
			buttonImage.sprite = hoverSprite;
			buttonText.color = hoverColor;
		}
	}

	public void OnPointerExit (PointerEventData eventData) {
		over = false;
		if (thisButton.interactable) {			
			if (!click) {
				// leave the button without clicking it
				buttonImage.sprite = defaultSprite;
				buttonText.color = defaultColor;
			}
		}
	}

	public void OnPointerDown (PointerEventData eventData) {
		click = true;
		if (thisButton.interactable) {					
			buttonImage.sprite = clickSprite;
			buttonText.color = clickColor;
		}
	}

	public void OnPointerUp (PointerEventData eventData) {
		click = false;
		if (thisButton.interactable) {
			if (over) {
				// click; technically not necessary, you immediately pass to the next scene
				buttonImage.sprite = hoverSprite;
				buttonText.color = hoverColor;
			} else {
				// holding the button down and moving outside
				buttonImage.sprite = defaultSprite;
				buttonText.color = defaultColor;
			}
		}
	}

}
