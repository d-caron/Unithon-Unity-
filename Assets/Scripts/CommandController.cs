using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandController : MonoBehaviour
{

    // La liste des commandes en attente
    public List<Command> commands;
    private UIController uIController;

    private CharacterControl currentCharacter;


    void Start()
    {
        commands = new List<Command>();
        uIController = GameObject.Find("GameController").GetComponent<UIController>();
    }

    // Ajoute une nouvelle commande à la liste des commandes en attente
    public void NewCommand(Command cmd) {
        CharacterControl targetCharacter = GameObject.Find(cmd.args[0]).GetComponent<CharacterControl>();

        // On check si le targetCharacter a bien été trouvé
        if (targetCharacter != null) {
            if (!targetCharacter.GetIsOccupied()) {
                targetCharacter.HandleCommand(cmd);
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

    // Supprime une commande de la liste à partir de l'id d'une commande (unique générée aléatoirement avec GUID)
    void DeleteCommand(Command command) {
        commands.Remove(command);
    }

    // Appelé lorsqu'un personnage vient de terminer une action
    public void ActionFree(string id) {
        // On regarde si il y a une commande en attente pour le personnage dont le nom est "id" (nom de l'objet)
        
        Command cmd = commands.Find(commands => commands.args[0].Equals(id));
        // Si on trouve une commande, on affecte alors au personnage cette commande (la première qui trouve dans la liste)
        if (cmd != null) {
            GameObject.Find(id).GetComponent<CharacterControl>().HandleCommand(cmd);
            DeleteCommand(cmd);
        }
        UpdateCommandsUI();
    }

    /*
    * @do : Attribut une nouvelle valeur pour character, pour permettre de savoir sur quelle IA est posée la caméra
    * @args : CharacterControl, la nouvelle IA focus
    */
    public void SetNewIAFocus (CharacterControl character) {
        Debug.Log(character);
        this.currentCharacter = character;
        UpdateCommandsUI();
    }

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


    public void UpdateCommandsUI() {
        uIController.SetCommandsUI(GetCommandSpecificIA());
    }
}