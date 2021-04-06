using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    // Variable de log
    private TextMeshProUGUI log;

    // La liste de commande pour l'affichage des commandes dans le log (5 maximum)
    private List<Command> logCommands;

    // La liste de commande pour l'affichage des icons
    private List<Command> commandsUI;

    // La liste de template des icons, icon pour le déplacement, icon pour discuter (icons[0] correspond au template par défaut, un cercle vide)
    public List<Sprite> icons;

    // La liste des images correspondant aux icons affichées à l'écran (5 affichées)
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


    /*
    * @do     : Ajoute une nouvelle ligne à logCommands
    * @args   : Command la nouvelle commande
    */
    public void SetNewLineLog (Command cmd) {
        // Si le nombre de ligne est supérieur à 5 alors on supprime la première valeur
        if(logCommands.Count >= 5) {
            logCommands.RemoveAt(0);
        }

        logCommands.Add(cmd);
        
        // On met à jour l'affichage du log
        UpdateLog();
    }

    /*
    * @do     : Met à jour le text du log à partir du tableau logCommands
    */
    public void UpdateLog() {
        string text = "";

        // Parcours chaque string de logTexts pour l'ajouter au log avec un \n pour sauter une ligne
        foreach(Command c in logCommands) {
            text += c.GetLog() + "\n";
        }
        log.text = text;
    }

    /*
    * @do     : Met à zéro la liste de commande logCommands
    */
    public void ResetLog() {
        logCommands = new List<Command>();
        UpdateLog();
    }

    /*
    * @do     : Affecte une nouvelle liste de Commande à commandsUI
    * @args   : List<Command> la nouvelle liste de commande
    */
    public void SetCommandsUI(List<Command> commands) {
        commandsUI = commands;
        if (commands != null) {
            UpdateCommandsUi();
        }
    }

    /*
    * @do     : Met à jour le tableau d'image selon les commandes dans commandsUI
    */
    public void UpdateCommandsUi(){
        ResetCommandsUI();
        for (int i = 0; i < commandsUI.Count; i++) {
            if (i < images.Length) {
                IconMouseOver iconMouseOver = images[i].GetComponent<IconMouseOver>();
                iconMouseOver.gameObject.SetActive(true);
                images[i].sprite = GetSpriteForCommands(commandsUI[i]);
                images[i].color = new Color(1f,1f,1f,1f);
                iconMouseOver.SetText(commandsUI[i].action + " : " + commandsUI[i].args[1]);
            }
        }
    }

    /*
    * @do     : Reset le tableau de commande avec de"s valeurs null
    */
    public void ResetCommandsUI() {
        for (int i = 0; i < images.Length; i++) {
            images[i].sprite = null;
            images[i].color = new Color(0f,0f,0f,0f);
            IconMouseOver iconMouseOver = images[i].GetComponent<IconMouseOver>();
            iconMouseOver.gameObject.SetActive(false);

        }
    }

    /*
    * @do     : Renvoie le Sprite associé à l'action de la commande en paramètre
    * @return : Sprite
    * @args   : Command
    */
    private Sprite GetSpriteForCommands(Command cmd) {
        for(int i = 0; i < icons.Count; i++) {
            Debug.Log(icons[i].name);
            if (icons[i].name.ToLower().Contains(cmd.action.ToLower())) {
                return icons[i];
            }
        }
        return icons[0];
    }

}
