using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField]
    Canvas speechBubble;

    [SerializeField]
    Text bubbleText;

    [SerializeField]
    float delay = 0.1f;

    [SerializeField]
    string fullText;

    private string currentText = "";

    private void Start()
    {

        speechBubble.enabled = false;
        bubbleText.text = "";
    }

    private void OnMouseDown()
    {
        Speak();
    }

    private void Speak()
    {
        StopAllCoroutines();
        bubbleText.text = "";
        speechBubble.enabled = true;
        StartCoroutine(ShowText());
    }

    private void StopSpeaking()
    {
        speechBubble.enabled = false;
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(5);
        StopSpeaking();
    }

    public IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            bubbleText.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        StartCoroutine(Timer());
    }
}
