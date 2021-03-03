using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandeController : MonoBehaviour
{

    // La liste des commandes en attente
    List<Commande> commands;




    void Start()
    {
        Debug.Log("wtf");
        commands = new List<Commande>();
        newCommand(new Commande("Deplacer", new string[] {"Michel", "Up"}));
        newCommand(new Commande("Deplacer", new string[] {"Michel", "Down"}));
        newCommand(new Commande("Deplacer", new string[] {"Michel", "Right"}));
    }

    // Ajoute une nouvelle commande à la liste des commandes en attente (testé)
    void newCommand(Commande command) {
        // TODO Contrôle si le personnage peut bien exécuter la commande

        commands.Add(command);
    }

    // Supprime une commande de la liste à partir de la commande (testé)
    void deleteCommand(Commande command) {
        commands.RemoveAll(commands => commands.id == command.id);
    }

    public void actionFree(string id) {
        string name = id;
        Debug.Log(name);
        
        Commande cmd = commands.Find(commands => commands.ids[0].Equals(name));
        if (cmd != null) {
            GameObject.Find(id).GetComponent<CharacterControl>().SetCommand(cmd);
            deleteCommand(cmd);
        }
    }
}