using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Etat de la commande
public enum State {
    WAIT,
    START,
    FINISH
}

[System.Serializable]
public class Command
{
    public string action;
    public string[] args = new string[2];
    public State state;
    

    public Command () {
        args = new string[2];
    }


    // Renvoie le log correspond à la commande selon l'état de la commande
    public string GetLog() {
        string text = "";
        if (state == State.WAIT) {
            text += "<color=#ff751a> [ATTENTE] ";
        } else if (state == State.START) {
            text += "<color=#33adff> [EN COURS] ";
        } else {
            text += "<color=#00c90e> [TERMINÉ] ";
        }
        return text + "Commande " + action + " : " + args[0] + " -> " + args[1] + "</color>";
    }
}
