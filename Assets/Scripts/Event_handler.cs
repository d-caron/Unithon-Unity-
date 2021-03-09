using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_handler : MonoBehaviour
{

    CommandeController commandeControl;

    // Start is called before the first frame update
    void Start()
    {
        commandeControl = GameObject.Find("GameController").GetComponent<CommandeController>();
    }
    // Update is called once per frame
    void Update()
    {
        // [UP_ARROW] Go to Up position
        if (Input.GetKeyDown (KeyCode.UpArrow)) {
            commandeControl.newCommand(new Commande("Deplacer", new string[] {"Michel", "Up"}));
        }

        // [RIGHT_ARROW] Go to Right position
        if (Input.GetKeyDown (KeyCode.RightArrow)) {
            commandeControl.newCommand(new Commande("Deplacer", new string[] {"Michel", "Right"}));
        }

        // [DOWN_ARROW] Go to Down position
        if (Input.GetKeyDown (KeyCode.DownArrow)) {
            commandeControl.newCommand(new Commande("Deplacer", new string[] {"Michel", "Down"}));
        }

        // [LEFT_ARROW] Go to Left position
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            commandeControl.newCommand(new Commande("Deplacer", new string[] {"Michel", "Left"}));
        }

        // [SPACE] Send a message to Python
        if (Input.GetKeyDown (KeyCode.Space)) {
            Comm comm = GameObject.Find ("Comm").GetComponent<Comm> ();
            comm.SendTCPMessage ();
        }
    }
}
