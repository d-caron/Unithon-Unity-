using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterControl : MonoBehaviour
{
    CommandeController commandController;
    // Start is called before the first frame update

    // Permet de savoir si le personnage est occupé ou non
    private bool isOccupied = false;
    private UIController uIController;

    // Commande actuelle du personnage
    private Commande command;

    void Start()
    {
        // On récupère le commandController qui permet de faire une files d'attentes
        commandController = GameObject.Find("GameController").GetComponent<CommandeController>();

        // On récupère l'uiController qui permet d'envoyer les notifications aux log
        uIController = GameObject.Find("GameController").GetComponent<UIController>();
    }

    // Renvoie le boolean indiquant si oui ou non le personnage est occupé
    public bool GetIsOccupied() {
        return this.isOccupied;
    }

    // Affecte true à isOccupied
    public void IsOccupied() {
        this.isOccupied = true;
    }

    // Affecte false à isOccupied
    public void IsNotOccupied() {
        this.isOccupied = false;

        // Lorsqu'on a fini une commande, on ajoute une ligne dans le log en vert
        uIController.SetNewLineLog("<color=#00c90e>" + command.GetLogFinish() + "</color>");

        // On affecte null à la commande car elle vient d'être terminée
        this.command = null;

        // Dit au commande controller qu'il est en attente d'une nouvelle commande
        commandController.ActionFree(gameObject.name);

    }

    // Pas censé être comme ça, version pour effectuer les tests en attente du DAO
    public void SetCommand(Commande cmd) {
        // Lorsqu'on reçoit une nouvelle commande, on devient occupé
        this.isOccupied = true;
        
        // On affecte la nouvelle commande
        this.command = cmd;

        // Ajoute une ligne dans le log, la balise color permet de donner une couleur à la ligne (ici bleu)
        uIController.SetNewLineLog("<color=#3458eb>" + cmd.GetLogExecute() + "</color>");

        // Go to Up position
        if (cmd.ids[1].Equals("Up")) {
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = GameObject.Find ("Ugo").transform.position;
        }

        // Go to Right position
        if (cmd.ids[1].Equals("Right")) {
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (11, 0, 0);
        }

        // Go to Down position
        if (cmd.ids[1].Equals("Down")) {
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (4, 0, -4);
        }

        // Go to Left position
        if (cmd.ids[1].Equals("Left")) {
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (0, 0, 0);
        }
    }

    

}
