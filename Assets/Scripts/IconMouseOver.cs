using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class IconMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over = false;
    public string text;

    TextMeshProUGUI description;

    void Start() {
        description = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
     {
        if (mouse_over)
        {
            description.gameObject.SetActive(true);
            description.text = text;
        } else {
            description.text = "";       
            description.gameObject.SetActive(false);
        }
     }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
    }

    public void GetText() {
        Debug.Log(text);
    }

    public void SetText(string text) {
        this.text = text;
    }
}
