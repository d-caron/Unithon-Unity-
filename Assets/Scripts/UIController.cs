using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    // Variable de log
    private TextMeshProUGUI log;

    // La liste qui contient maximum 5 log 
    private List<string> logTexts;

    void Start()
    {
        // Récupération de l'objet log
        log = FindObjectOfType<TextMeshProUGUI>();

        logTexts = new List<string>();
    }

    // Méthode public permettant de rajouter une ligne dans le log
    public void SetNewLineLog (string line) {
        // Si le nombre de ligne est supérieur à 5 alors on supprime la première valeur
        if(logTexts.Count >= 5) {
            logTexts.RemoveAt(0);
        }
        // On ajoute la nouvelle ligne dans la liste
        logTexts.Add(line);

        // On met à jour l'affichage du log
        UpdateLog();
    }

    // Méthode permettant d'actualiser l'affichage du log
    private void UpdateLog() {
        string text = "";

        // Parcours chaque string de logTexts pour l'ajouter au log avec un \n pour sauter une ligne
        foreach(string l in logTexts) {
            text += l + "\n";
        }
        log.text = text;
    }

    // Méthode public permettant de remettre à zéro le log
    public void ResetLog() {
        logTexts = new List<string>();
        UpdateLog();
    }
}
