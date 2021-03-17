using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Commande
{
    // Ajout d'un ID à commande pour pouvoir bien les distinguer
    public string id;
    public string command = "";
    public string[] ids = {};
    

    public Commande (string c, string[] ids) {
        this.command = c;
        this.ids = ids;

        id = System.Guid.NewGuid().ToString();
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

    // Renvoie un string pour le log, lorsque la commande est mise en file d'attente
    public string GetLogQueue() {
        string text = "<color=#FF0000> Commande " + command + " mise en la file d'attente de " + ids[0] ;
        text += GetParam() + "</color>";
        return text;
    }

    // Renvoie un string dans le log, lorsque la commande commence à être exécuté
    public string GetLogExecute() {
        string text = "<color=#3458eb> Commande " + command + " commencée par " + ids[0];
        text += GetParam() + "</color>";
        return text;
    }

    // Renvoie un string dans le log, lorsque la commande a été terminée
    public string GetLogFinish() {
        string text = "<color=#00c90e> Commande " + command + " terminée par " + ids[0];
        text += GetParam() + "</color>";
        return text;
    }

    // Renvoie un string contenant les différents paramètres d'une commande
    public string GetParam() {
        string text = "";
        if (ids.Length > 1) {
            text += ", param : ";
            for(int i = 1; i < ids.Length; i++) {
                text += ids[i] + " ";
            }
        }
        return text;
    }
}
