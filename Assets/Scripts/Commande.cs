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

    public string ToString() {
        if(ids.Length > 0) {
            return "Nouvelle commande : " + command + " sur " + ids[0];
        }
        else {
            return "Nouvelle commande"+ " : " + command;        
        }
    }
}
