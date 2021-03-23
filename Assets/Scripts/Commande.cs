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
public class Commande
{
    public string command = "";
    public string[] ids = {};

    public State state;
    

    public Commande (string c, string[] ids) {
        this.command = c;
        this.ids = ids;
    }
    
    // Réécriture du toString, obsolète
    public override string ToString() {
        if(ids.Length == 2) {
            return "Nouvelle commande : " + command + " sur " + ids[0] + " vers " + ids[1];
        }
        else {
            return "Nouvelle commande"+ " : " + command;        
        }
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
        return text + "Commande " + command + " : " + ids[0] + " -> " + ids[1] + "</color>";
    }

    // PLUS UTILISÉ
    // // Renvoie un string pour le log, lorsque la commande est mise en file d'attente
    // public string GetLogQueue() {
    //     string text = "<color=#ff751a> Commande " + command + " mise en la file d'attente de " + ids[0] + state.ToString();
    //     text += GetParam() + "</color>";
    //     return text;
    // }

    // // Renvoie un string dans le log, lorsque la commande commence à être exécuté
    // public string GetLogExecute() {
    //     string text = "<color=#33adff> Commande " + command + " commencée par " + ids[0] + state.ToString();
    //     text += GetParam() + "</color>";
    //     return text;
    // }

    // // Renvoie un string dans le log, lorsque la commande a été terminée
    // public string GetLogFinish() {
    //     string text = "<color=#00c90e> Commande " + command + " terminée par " + ids[0] + state.ToString();
    //     text += GetParam() + "</color>";
    //     return text;
    // }

    // // Renvoie un string contenant les différents paramètres d'une commande
    // public string GetParam() {
    //     string text = "";
    //     if (command.Equals("Deplacer")) {
    //         text += " va vers " + ids[1];
    //     } else if (command.Equals("Discuter")) {
    //         text += " va discuter avec " + ids[1];
    //     }
    //     return text;
    // }
}
