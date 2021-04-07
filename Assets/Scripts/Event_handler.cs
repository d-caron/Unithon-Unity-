using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_handler : MonoBehaviour
{
    
    CommandController commandeControl;

    // Start is called before the first frame update
    void Start()
    {
        commandeControl = GameObject.Find("GameController").GetComponent<CommandController>();
    }
    // Update is called once per frame
    void Update()
    {
        

        // [UP_ARROW] Go to Up position
        if (Input.GetKeyDown (KeyCode.UpArrow)) {
            Command cmd = new Command();
            cmd.action = "deplacer";
            cmd.args[0] = "Michel";
            cmd.args[1] = "Ugo";
            commandeControl.NewCommand(cmd);
        }

        // [RIGHT_ARROW] Go to Right position
        if (Input.GetKeyDown (KeyCode.RightArrow)) {
            Command cmd = new Command();
            cmd.action = "deplacer";
            cmd.args[0] = "Michel";
            cmd.args[1] = "Est";
            commandeControl.NewCommand(cmd);
        }

        // [DOWN_ARROW] Go to Down position
        if (Input.GetKeyDown (KeyCode.DownArrow)) {
            Command cmd = new Command();
            cmd.action = "deplacer";
            cmd.args[0] = "Michel";
            cmd.args[1] = "Sud";
            commandeControl.NewCommand(cmd);
        }

        // [LEFT_ARROW] Go to Left position
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            Command cmd = new Command();
            cmd.action = "deplacer";
            cmd.args[0] = "Michel";
            cmd.args[1] = "Ouest";
            commandeControl.NewCommand(cmd);
        }

        // [SPACE] Discuss with UGo
        if (Input.GetKeyDown (KeyCode.Space)) {
            Command cmd = new Command();
            cmd.action = "discuter";
            cmd.args[0] = "Michel";
            cmd.args[1] = "Ugo";
            commandeControl.NewCommand(cmd);
        }

        // [UP_ARROW] Go to Up position
        if (Input.GetKeyDown (KeyCode.Z)) {
            Command cmd = new Command();
            cmd.action = "deplacer";
            cmd.args[0] = "Ugo";
            cmd.args[1] = "Michel";
            commandeControl.NewCommand(cmd);
        }

        // [RIGHT_ARROW] Go to Right position
        if (Input.GetKeyDown (KeyCode.D)) {
            Command cmd = new Command();
            cmd.action = "deplacer";
            cmd.args[0] = "Ugo";
            cmd.args[1] = "Est";
            commandeControl.NewCommand(cmd);
        }

        // [DOWN_ARROW] Go to Down position
        if (Input.GetKeyDown (KeyCode.S)) {
            Command cmd = new Command();
            cmd.action = "deplacer";
            cmd.args[0] = "Ugo";
            cmd.args[1] = "Sud";
            commandeControl.NewCommand(cmd);
        }

        // [LEFT_ARROW] Go to Left position
        if (Input.GetKeyDown (KeyCode.Q)) {
            Command cmd = new Command();
            cmd.action = "deplacer";
            cmd.args[0] = "Ugo";
            cmd.args[1] = "Ouest";
            commandeControl.NewCommand(cmd);
        }
    }
}