using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInfo : MonoBehaviour
{
    [SerializeField] GameObject PopUp;


    // Start is called before the first frame update
    void Start()
    {
        PopUp.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StopAllCoroutines();
            PopUp.GetComponent<TextMeshPro>().color = new Color(PopUp.GetComponent<TextMeshPro>().color.r, PopUp.GetComponent<TextMeshPro>().color.g, PopUp.GetComponent<TextMeshPro>().color.b, 1);
            PopUp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //PopUp.SetActive(false);
            StartCoroutine(FadePopup(1f, PopUp.GetComponent<TextMeshPro>()));
        }
    }

    public IEnumerator FadePopup(float time, TextMeshPro text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while(text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / time));
            yield return null;
        }
    }
}
