using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_handler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Up position
        if (Input.GetKeyDown (KeyCode.UpArrow)) {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = GameObject.Find ("Ugo").transform.position;
        }

        // Right position
        if (Input.GetKeyDown (KeyCode.RightArrow)) {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (11, 0, 0);
        }

        // Down position
        if (Input.GetKeyDown (KeyCode.DownArrow)) {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (4, 0, -4);
        }

        // Left position
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (0, 0, 0);
        }
    }
}
