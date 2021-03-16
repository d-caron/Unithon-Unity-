using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandeController : MonoBehaviour
{

    // La liste des commandes en attente
    public List<Commande> commands;
    private UIController uIController;


    void Start()
    {
        commands = new List<Commande>();
        uIController = GameObject.Find("GameController").GetComponent<UIController>();
    }

    // Ajoute une nouvelle commande à la liste des commandes en attente
    public void NewCommand(Commande cmd) {
        // On récupère l'id du personnage dont est destiné la commande (la première valeur du tableau d'ids de Commande)
        string id = cmd.ids[0];

        // On récupère le characterControl du personnage dont est destiné la commande
        CharacterControl characterControl = GameObject.Find(id).GetComponent<CharacterControl>();

        Debug.Log(cmd.ToString());
        // On check si le characterControl a bien été trouvé
        if (characterControl != null) {

            // On regarde si le personnage est occupé
            if (!characterControl.GetIsOccupied()) {
                
                // S'il n'est pas occupé alors on regarde s'i 'il n'y pas déjà une commande en attente pour ce personnage
                Commande cmdFromList = commands.Find(commands => commands.ids[0].Equals(id));
                
                // Si il y a une commande en attente alors on affecte la commande trouvée au personnage et passe la commande en paramètre dans la file d'attente
                if (cmdFromList != null) {
                    GameObject.Find(id).GetComponent<CharacterControl>().SetCommand(cmd);
                    DeleteCommand(cmd);
                    commands.Add(cmd);

                    // Ajoute une ligne dans le log, la balise color permet de donner une couleur à la ligne
                    uIController.SetNewLineLog("<color=#FF0000>" + cmd.GetLogQueue() + "</color>");
                }

                // Si il n'y a pas de commande en attente alors on affecte cette nouvelle commande au personnage
                else {
                    characterControl.SetCommand(cmd);
                }
            }
            // Sinon elle passe en file d'attente
            else {
                // Ajoute une ligne dans le log, la balise color permet de donner une couleur à la ligne
                uIController.SetNewLineLog("<color=#FF0000>" + cmd.GetLogQueue() + "</color>");
                
                commands.Add(cmd);
            }   
        }

        
    }

    // Supprime une commande de la liste à partir de l'id d'une commande (unique générée aléatoirement avec GUID)
    void DeleteCommand(Commande command) {
        commands.Remove(command);
    }

    // Appelé lorsqu'un personnage vient de terminer une action
    public void ActionFree(string id) {
        // On regarde si il y a une commande en attente pour le personnage dont le nom est "id" (nom de l'objet)
        
        Commande cmd = commands.Find(commands => commands.ids[0].Equals(id));
        // Si on trouve une commande, on affecte alors au personnage cette commande (la première qui trouve dans la liste)
        if (cmd != null) {
            GameObject.Find(id).GetComponent<CharacterControl>().SetCommand(cmd);
            DeleteCommand(cmd);
        }
    }
}