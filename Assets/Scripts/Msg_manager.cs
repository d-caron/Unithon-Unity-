using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Comm;


namespace Msg_manager 
{
    public class Manager
    {
        public static void Recv_handler (string msg){
            Debug.Log (msg);

            var dao = new DAO();
            dao.Deserialize(msg);

            switch (dao.type){
                
                case "cmd":
                    CommandController commandController = GameObject.Find("GameController").GetComponent<CommandController>();
                    if (commandController == null) Debug.Log ("YA RI1 !");
                    Command cmd = new Command ();
                    cmd.action = dao.action;
                    cmd.args[0] = dao.characters[0];

                    switch (dao.action){
                        case "deplacer":
                            cmd.args[1] = dao.world.regions.Length == 0 ? dao.characters[1] : dao.world.regions[0];
                            break;
                        
                        case "discuter":
                            cmd.args[1] = dao.characters[1];
                            break;
                        
                        default:
                            Debug.Log("Commande d'action non reconnue");
                            return;
                    }

                    commandController.NewCommand (cmd);
                    break;
                
                case "sys":
                    
                    switch (dao.action){
                        
                        case "exit":
                            Application.Quit ();
                            //UnityEditor.EditorApplication.isPlaying = false;
                            break;                      

                        default:
                            Debug.Log("Commande système non reconnue");   
                            break;
                    }
                    break;

                default:
                    Debug.Log("Commande non reconnue");
                    break;
            } 
        }
    }  
}

