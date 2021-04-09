using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandController : MonoBehaviour
{

    // La liste des commandes en attente
    public List<Command> commands;

    // Le composant contrôlant l'UI
    private UIController uIController;

    // Le composant CharacterControl du personnage qu'on regarde, null si on est dans la vue global
    private CharacterControl currentCharacter;


    void Start()
    {
        commands = new List<Command>();
        uIController = GameObject.Find("GameController").GetComponent<UIController>();
    }

    /*
    * @do : Ajoute une nouvelle commande à la liste des commandes en attente ou si le personnage est libre, directement au personnage
    * @args : Command, la commande à ajouter
    */
    public void NewCommand(Command cmd) {
        CharacterControl targetCharacter = GameObject.Find(cmd.args[0]).GetComponent<CharacterControl>();

        // On check si le targetCharacter a bien été trouvé
        if (targetCharacter != null) {
            if (!targetCharacter.GetIsOccupied()) {
                targetCharacter.HandleCommand(cmd, false);
            } else if (targetCharacter.GetCurrentCommand().passive) {
                targetCharacter.GetCurrentCommand().state = State.FINISH;
                targetCharacter.HandleCommand(cmd, false);
            }
            // Sinon elle passe en file d'attente
            else {
                cmd.state = State.WAIT;
                commands.Add(cmd);
            }
            uIController.SetNewLineLog(cmd);   
        }
        if (currentCharacter == null) {
            UpdateCommandsUI();
        } else if (targetCharacter.name.Equals(currentCharacter.name)) {
            UpdateCommandsUI();
        }
    }

    /*
    * @do : Supprime la Command en paramètre de la liste de commands
    * @args : Command, la commande à supprimer
    */
    void DeleteCommand(Command command) {
        commands.Remove(command);
    }

    /*
    * @do : Appelé par le CharacterControl pour signaler que le personnage vient de finir son action pour en demmander une nouvelle
    * @args : string, l'id du personnage (le nom) qui appel la fonction
    */
    public void ActionFree(string id) {
        // On regarde si il y a une commande en attente pour le personnage dont le nom est "id" (nom de l'objet)
        
        Command cmd = commands.Find(commands => commands.args[0].Equals(id));
        // Si on trouve une commande, on affecte alors au personnage cette commande (la première qui trouve dans la liste)
        if (cmd != null) {
            GameObject.Find(id).GetComponent<CharacterControl>().HandleCommand(cmd, false);
            DeleteCommand(cmd);
        }
        UpdateCommandsUI();
    }

    /*
    * @do : Attribut une nouvelle valeur pour character, pour permettre de savoir sur quelle IA est posée la caméra
    * @args : CharacterControl, la nouvelle IA focus
    */
    public void SetNewIAFocus (CharacterControl character) {
        this.currentCharacter = character;
        UpdateCommandsUI();
    }

    /*
    * @do : Renvoie la liste des commandes liées au personnage actuel (commande en attente + commande en cours), ou une liste null si on est sur la vue global
    * @return : List<Command>, la liste des commandes liées à un personnage (commande en attente + commande en cours)
    */
    public List<Command> GetCommandSpecificIA() {
        List<Command> commandsIA = new List<Command>();
        if(currentCharacter != null) {
            if (currentCharacter.GetCurrentCommand() != null) {
                commandsIA.Add(currentCharacter.GetCurrentCommand());
            }
            commandsIA.AddRange(commands.FindAll(commands => commands.args[0].Equals(currentCharacter.name)));
        }


        return commandsIA;
    }

    /*
    * @do : Affecte au composant UIController la nouvelle liste de commande d'un IA spécifique via la fonction GetCommandSpecificIA
    */
    public void UpdateCommandsUI() {
        uIController.SetCommandsUI(GetCommandSpecificIA());
    }
}