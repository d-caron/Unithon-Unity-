using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    CommandeController commandController;
    // Start is called before the first frame update

    // Permet de savoir si le personnage est occupé ou non
    private bool isOccupied = false;

    void Start()
    {
        commandController = GameObject.Find("GameController").GetComponent<CommandeController>();
    }

    public bool GetIsOccupied() {
        return this.isOccupied;
    }

    public void IsOccupied() {
        this.isOccupied = true;
        Debug.Log("Is occupied !");
    }

    public void IsNotOccupied() {
        this.isOccupied = false;
        Debug.Log("Is not occupied !");

        commandController.actionFree(gameObject.name);
    }

    public void SetCommand(Commande cmd) {
        // [UP_ARROW] Go to Up position
        if (cmd.ids[1].Equals("Up")) {
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = GameObject.Find ("Ugo").transform.position;
        }

        // [RIGHT_ARROW] Go to Right position
        if (cmd.ids[1].Equals("Right")) {
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (11, 0, 0);
        }

        // [DOWN_ARROW] Go to Down position
        if (cmd.ids[1].Equals("Down")) {
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (4, 0, -4);
        }

        // [LEFT_ARROW] Go to Left position
        if (cmd.ids[1].Equals("Left")) {
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (0, 0, 0);
        }
    }

    

}
