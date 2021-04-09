using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static Command;

public class CharacterControl : MonoBehaviour
{
    CommandController commandController;

    // Permet de savoir si le personnage est occupé ou non
    public bool isOccupied = false;
    private UIController uIController;

    // Commande actuelle du personnage
    public Command command;

    // Distance max pour être considéré comme étant proche de quelque chose
    private static float DIST_NEXT_MAX = 1.0F;

    void Start()
    {
        // On récupère le commandController qui permet de faire une files d'attentes
        commandController = GameObject.Find("GameController").GetComponent<CommandController>();

        // On récupère l'uiController qui permet d'envoyer les notifications aux log
        uIController = GameObject.Find("GameController").GetComponent<UIController>();

        this.command = null;

        // De base on désactive les différentes actions
        DisableComponentAction();
    }

    void Update() {
        // Si on a pas d'action à réaliser alors on active le composant Idle pour faire les différentes animations
        if(GetCurrentCommandAction().Equals("")) {
            GetComponent<Idle>().enabled = true;
        } else {
            GetComponent<Idle>().enabled = false;
        }
    }

    /*
    * @do : Renvoie la commande actuellement exécuté par l'IA
    * @return Command
    */
    public Command GetCurrentCommand() {
        return command;
    }

    /*
    * @do : Renvoie l'action actuellement exécuté par l'IA
    * @return string
    */
    public string GetCurrentCommandAction() {
        if (command != null) {
            if (command.action != null) {
                return command.action;
            }
        }
        return "";
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
        Debug.Log(transform.name + ": " + "IsNotOccupied");


        // On affecte null à la commande car elle vient d'être terminée sauf discuter car c'est une action "passive"
        SetCommandTerminateAndNull();
        DisableComponentAction();

        // Dit au commande controller qu'il est en attente d'une nouvelle commande
        commandController.ActionFree(gameObject.name);
        uIController.UpdateLog();
    }

    public void SetCommandTerminateAndNull() {
        if(!GetCurrentCommandAction().Equals("")) {
            command.state = State.FINISH;
            command = null;
        }
    }
    
    /*
    * @do : Permet au CommandController d'affecter une nouvelle commande au CharacterControl et de lui affecter l'action correspondante
    * @args : cmd, la nouvelle commande
              ignore, si on est la target du discussion on ignore l'execution de la commande
    */ 
    public void HandleCommand(Command cmd, bool ignore) {
        if (GetComponent<Talk>().enabled) {
            GetComponent<Talk>().IsFinish();
        }

        // Lorsqu'on reçoit une commande on désactive toutes les actions
        DisableComponentAction();

        // Lorsqu'on reçoit une nouvelle commande, on devient occupé
        this.isOccupied = true;
        
        
        SetCommandTerminateAndNull();
        command = cmd;

        // Passe le status de la commande à START pour signaler au log qu'elle commence
        cmd.state = State.START;

        switch (cmd.action) {
            case "deplacer" :
                GetComponent<Deplacer>().enabled = true;
                Deplacer deplacer = GameObject.Find(cmd.args[0]).GetComponent<Deplacer> ();
                deplacer.dest = GameObject.Find (cmd.args[1]).transform.position;
                break;

            case "discuter" :
                GameObject char1 = GameObject.Find(cmd.args[0]);
                GameObject char2 = GameObject.Find(cmd.args[1]);
                if (IsNextToMe(char2.transform.position)) {
                    cmd.passive = true;
                    GetComponent<Talk>().enabled = true;
                    if (!ignore) {
                        char1.GetComponent<Talk>().partner = char2;
                        char2.GetComponent<Talk>().partner = char1;
                        char2.GetComponent<CharacterControl>().HandleCommand(new Command("discuter", new string[]{char2.name, cmd.args[0]}), true);
                        commandController.UpdateCommandsUI();
                    }
                } 
                // Si on est pas proche du personnage, on termine la commande sans rien faire
                else {
                    IsNotOccupied();
                }
                break;
        }
        uIController.UpdateLog();
        
    }

    /*
    * @do : Désactive les composants d'actions pour l'instant juste Deplacer et Talk, rajouté ici les différentes actions
    */ 
    public void DisableComponentAction(){
        GetComponent<Idle>().enabled = false;
        GetComponent<Deplacer>().enabled = false;
        GetComponent<Talk>().enabled = false;
    } 

    /*
    * @do : Check si le vector3 to est proche du gameObject à partir de DIST_NEXT_MAX qui est la distance maximale pour être considéré comme étant proche
    * @args : Vector3 to, les coordonnées à tester
    * @return : bool
    */
    public bool IsNextToMe(Vector3 to){
        return !((Math.Abs (transform.position.x - to.x) > DIST_NEXT_MAX) ||
        (Math.Abs (transform.position.z - to.z) > DIST_NEXT_MAX));
    }

    

}
