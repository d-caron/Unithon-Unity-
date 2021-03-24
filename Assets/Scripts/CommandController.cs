using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandController : MonoBehaviour
{

    // La liste des commandes en attente
    public List<Command> commands;
    private UIController uIController;


    void Start()
    {
        commands = new List<Command>();
        uIController = GameObject.Find("GameController").GetComponent<UIController>();
    }

    // Ajoute une nouvelle commande à la liste des commandes en attente
    public void NewCommand(Command cmd) {
        CharacterControl targetCharacter = GameObject.Find(cmd.args[0]).GetComponent<CharacterControl>();

        Debug.Log(cmd);

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
    }
}