using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    // Variable de log
    private TextMeshProUGUI log;

    // La liste qui contient maximum 5 log 
    private List<Command> logCommands;

    private List<Command> commandsUI;

    public List<Sprite> icons;

    public Image[] images;

    void Start()
    {
        // Récupération de l'objet log
        log = GameObject.Find("Log").GetComponent<TextMeshProUGUI>();
        //camera = GameObject.Find("GameController").GetComponent<SwapCamera>();
        logCommands = new List<Command>();
        commandsUI = new List<Command>();
        ResetCommandsUI();
    }



    // Méthode public permettant de rajouter une ligne dans le log
    public void SetNewLineLog (Command cmd) {
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
        foreach(Command c in logCommands) {
            text += c.GetLog() + "\n";
        }
        log.text = text;
    }

    // Méthode public permettant de remettre à zéro le log
    public void ResetLog() {
        logCommands = new List<Command>();
        UpdateLog();
    }

    public void SetCommandsUI(List<Command> commands) {
        commandsUI = commands;
        if (commands != null) {
            UpdateCommandsUi();
        }
    }

    public void UpdateCommandsUi(){
        ResetCommandsUI();
        for (int i = 0; i < commandsUI.Count; i++) {
            if (i < images.Length) {
                IconMouseOver iconMouseOver = images[i].GetComponent<IconMouseOver>();
                iconMouseOver.gameObject.SetActive(true);
                images[i].sprite = icons[0];
                images[i].color = new Color(1f,1f,1f,1f);
                iconMouseOver.SetText(commandsUI[i].action + " : " + commandsUI[i].args[1]);
            }
            
        }
    }

    public void ResetCommandsUI() {
        for (int i = 0; i < images.Length; i++) {
            images[i].sprite = null;
            images[i].color = new Color(0f,0f,0f,0f);
            IconMouseOver iconMouseOver = images[i].GetComponent<IconMouseOver>();
            iconMouseOver.gameObject.SetActive(false);

        }
    }

}
