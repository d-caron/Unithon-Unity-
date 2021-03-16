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
    
    public override string ToString() {
        if(ids.Length == 2) {
            return "Nouvelle commande : " + command + " sur " + ids[0] + " vers " + ids[1];
        }
        else {
            return "Nouvelle commande"+ " : " + command;        
        }
    }

    public string GetLogQueue() {
        string text = "Commande " + command + " mise en la file d'attente de " + ids[0];
        text += GetParam();
        return text;
    }

    public string GetLogExecute() {
        string text = "Commande " + command + " exécutée par " + ids[0];
        text += GetParam();
        return text;
    }

    public string GetLogFinish() {
        string text = "Commande " + command + " terminée par " + ids[0];
        text += GetParam();
        return text;
    }

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
