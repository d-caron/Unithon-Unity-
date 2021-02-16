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
        // [UP_ARROW] Go to Up position
        if (Input.GetKeyDown (KeyCode.UpArrow)) {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = GameObject.Find ("Ugo").transform.position;
        }

        // [RIGHT_ARROW] Go to Right position
        if (Input.GetKeyDown (KeyCode.RightArrow)) {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (11, 0, 0);
        }

        // [DOWN_ARROW] Go to Down position
        if (Input.GetKeyDown (KeyCode.DownArrow)) {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (4, 0, -4);
        }

        // [LEFT_ARROW] Go to Left position
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (0, 0, 0);
        }

        // [SPACE] Send a message to Python
        if (Input.GetKeyDown (KeyCode.Space)) {
            Comm comm = GameObject.Find ("Comm").GetComponent<Comm> ();
            comm.SendTCPMessage ();
        }
    }
}
