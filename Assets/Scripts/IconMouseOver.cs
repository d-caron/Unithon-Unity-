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
            if (!description.gameObject.activeInHierarchy){
                description.gameObject.SetActive(true);
                description.text = text;
            }
        } else {
            if (description.gameObject.activeInHierarchy){
                description.text = "";       
                description.gameObject.SetActive(false);
            }
        }
     }

    /*
    * @do : Affecte vrai à mouse_over lorsque la souris passe sur l'icon (interface : IPointerEnterHandler)
    */
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
    }
 
    /*
    * @do : Affecte faux à mouse_over lorsque la souris sort de l'icon (interface : IPointerExitHandler)
    */
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
    }

    /*
    * @do : Affecte une nouveau string à text
    * @args : string, la nouvelle valeur décrivant l'icon 
    */
    public void SetText(string text) {
        this.text = text;
    }
}
