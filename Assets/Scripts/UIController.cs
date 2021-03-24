using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    // Variable de log
    private TextMeshProUGUI log;

    // La liste qui contient maximum 5 log 
    private List<Commande> logCommands;

    void Start()
    {
        // Récupération de l'objet log
        log = FindObjectOfType<TextMeshProUGUI>();

        logCommands = new List<Commande>();
    }

    // Méthode public permettant de rajouter une ligne dans le log
    public void SetNewLineLog (Commande cmd) {
        // Si le nombre de ligne est supérieur à 5 alors on supprime la première valeur
        if(logCommands.Count >= 5) {
            logCommands.RemoveAt(0);
        }

        logCommands.Add(cmd);
        
        // On met à jour l'affichage du log
        UpdateLog();
    }

    // Méthode public permettant d'actualiser l'affichage du log
    public void UpdateLog() {
        string text = "";

        // Parcours chaque string de logTexts pour l'ajouter au log avec un \n pour sauter une ligne
        foreach(Commande c in logCommands) {
            text += c.GetLog() + "\n";
        }
        log.text = text;
    }

    // Méthode public permettant de remettre à zéro le log
    public void ResetLog() {
        logCommands = new List<Commande>();
        UpdateLog();
    }
}
