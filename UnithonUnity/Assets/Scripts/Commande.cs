using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Commande
{
    public string command = "";
    public string ids;

    public Commande (string c, string ids) {
        this.command = c;
        this.ids = ids;
    }
}
