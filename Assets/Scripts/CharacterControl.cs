using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Command;

public class CharacterControl : MonoBehaviour
{
    CommandController commandController;
    // Start is called before the first frame update

    // Permet de savoir si le personnage est occupé ou non
    private bool isOccupied = false;
    private UIController uIController;

    // Commande actuelle du personnage
    private Command command;

    void Start()
    {
        // On récupère le commandController qui permet de faire une files d'attentes
        commandController = GameObject.Find("GameController").GetComponent<CommandController>();

        // On récupère l'uiController qui permet d'envoyer les notifications aux log
        uIController = GameObject.Find("GameController").GetComponent<UIController>();
    }

    /*
    * @do : Renvoie la commande actuellement exécuté par l'IA
    * @return Command
    */
    public Command GetCurrentCommand() {
        return command;
    }

    /*
    * @do : Renvoie le boolean indiquant si oui ou non le personnage est occupé
    * @return bool
    */
    public bool GetIsOccupied() {
        return this.isOccupied;
    }

    /*
    * @do : Affecte vrai (true) à la variable isOccupied
    */
    public void IsOccupied() {
        this.isOccupied = true;
    }

    /*
    * @do : Affecte faux (false) à la variable isOccupied et demmande une nouvelle commande au CommandController
    */
    public void IsNotOccupied() {
        this.isOccupied = false;

        // Lorsqu'on a fini une commande on ajoute une ligne dans le log en vert, enlevé pour réduire le nombre de ligne dans les logs
        this.command.state = State.FINISH;
        uIController.UpdateLog();

        // On affecte null à la commande car elle vient d'être terminée sauf discuter car c'est une action "passive"
        if (!this.command.action.Equals("discuter")) {
            this.command = null;
        }
        

        // Dit au commande controller qu'il est en attente d'une nouvelle commande
        commandController.ActionFree(gameObject.name);
    }

    
    /*
    * @do : Permet au CommandController d'affecter une nouvelle commande au CharacterControl et de lui affecter l'action correspondante
    * @args : Command, la nouvelle commande
    */ 
    public void HandleCommand(Command cmd) {
        // Lorsqu'on reçoit une nouvelle commande, on devient occupé
        this.isOccupied = true;
        
        // On affecte la nouvelle commande
        this.command = cmd;

        // Ajoute une ligne dans le log
        cmd.state = State.START;
        uIController.UpdateLog();

        

        switch (cmd.action) {
            case "deplacer" :
                Deplacer deplacer = GameObject.Find (cmd.args[0]).GetComponent<Deplacer> ();
                deplacer.dest = GameObject.Find (cmd.args[1]).transform.position;
                break;

            case "discuter" :
                GameObject char1 = GameObject.Find (cmd.args[0]);
                GameObject char2 = GameObject.Find (cmd.args[1]);
                char1.GetComponent<Talk> ().partner = char2;
                char2.GetComponent<Talk> ().partner = char1;
                break;
        }
        
    }

    

}
