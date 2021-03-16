using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCamera : MonoBehaviour
{
    private GameObject mainCam;
    public Vector3 positionGlobal;
    public Vector3 rotationGlobal;
    private GameObject[] tabCamIA;
    public int selectedIA;
    public bool globalActivated;
    // Start is called before the first frame update
    void Start()
    {
        //On commence en position globale
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        mainCam.SetActive(true);
        mainCam.transform.position = positionGlobal;
        mainCam.transform.rotation = Quaternion.Euler(rotationGlobal);
        globalActivated = true;
        
        //on récupère toute les caméra des IA sur la map
        tabCamIA = GameObject.FindGameObjectsWithTag("CharacterCamera");
        foreach(GameObject cam in tabCamIA){
            cam.SetActive(false);
        }
        selectedIA = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(2)){
            globalActivated = !globalActivated;
        }
        

        if(globalActivated){
            tabCamIA[selectedIA].SetActive(false);
            mainCam.SetActive(true);
            transform.position = positionGlobal;
            transform.rotation = Quaternion.Euler(rotationGlobal);
        }
        else{
            tabCamIA[selectedIA].SetActive(true);
            mainCam.SetActive(false);
            if(Input.GetAxis("Mouse ScrollWheel")<0){
                tabCamIA[selectedIA].SetActive(false);
                selectedIA = Mathf.Abs(selectedIA - 1) % tabCamIA.Length;
                tabCamIA[selectedIA].SetActive(true);
            }
            if(Input.GetAxis("Mouse ScrollWheel")>0){
                tabCamIA[selectedIA].SetActive(false);
                selectedIA = Mathf.Abs(selectedIA + 1) % tabCamIA.Length;
                tabCamIA[selectedIA].SetActive(true);
            }
        }
        
    }
}
