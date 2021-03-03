using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandeController : MonoBehaviour
{

    // La liste des commandes en attente
    List<Commande> commands;




    void Start()
    {
        commands = new List<Commande>();

        // Liste de commande en dur pour faire les tests
        newCommand(new Commande("Deplacer", new string[] {"Michel", "Up"}));
        newCommand(new Commande("Deplacer", new string[] {"Michel", "Down"}));
        newCommand(new Commande("Deplacer", new string[] {"Michel", "Right"}));
    }

    // Ajoute une nouvelle commande à la liste des commandes en attente
    void newCommand(Commande cmd) {
        // On récupère l'id du personnage dont est destiné la commande (la première valeur du tableau d'ids de Commande)
        string id = cmd.ids[0];

        // On récupère le characterControl du personnage dont est destiné la commande
        CharacterControl characterControl = GameObject.Find(id).GetComponent<CharacterControl>();

        // On check si le characterControl a bien été trouvé
        if (characterControl != null) {

            // On regarde si le personnage est occupé
            if (!characterControl.GetIsOccupied()) {

                // S'il n'est pas occupé alors on regarde s'i 'il n'y pas déjà une commande en attente pour ce personnage
                Commande cmdFromList = commands.Find(commands => commands.ids[0].Equals(id));

                // Si il y a une commande en attente alors on affecte la commande trouvée au personnage et passe la commande en paramètre dans la file d'attente
                if (cmd != null) {
                    GameObject.Find(id).GetComponent<CharacterControl>().SetCommand(cmd);
                    deleteCommand(cmd);
                    commands.Add(cmd);
                }

                // Si il n'y a pas de commande en attente alors on affecte cette nouvelle commande au personnage
                else {
                    characterControl.SetCommand(cmd);
                }
            }
            else {
                // Sinon elle passe en file d'attente
                commands.Add(cmd);
            }   
        }
    }

    // Supprime une commande de la liste à partir de l'id d'une commande (unique générée aléatoirement avec GUID)
    void deleteCommand(Commande command) {
        commands.RemoveAll(commands => commands.id == command.id);
    }

    // Appelé lorsqu'un personnage vient de terminer une action
    public void actionFree(string id) {
        // On regarde si il y a une commande en attente pour le personnage dont le nom est "id" (nom de l'objet)
        Commande cmd = commands.Find(commands => commands.ids[0].Equals(id));

        // Si on trouve une commande, on affecte alors au personnage cette commande (la première qui trouve dans la liste)
        if (cmd != null) {
            GameObject.Find(id).GetComponent<CharacterControl>().SetCommand(cmd);
            deleteCommand(cmd);
        }
    }
}