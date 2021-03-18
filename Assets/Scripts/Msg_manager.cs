using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Comm;


namespace Msg_manager 
{
    public class Manager
    {
        public static void Recv_handler (string msg){

            var dao=new DAO();
            Debug.Log (msg);
            dao.Deserialize(msg);
            
            switch (dao.type){
      
                case "cmd":
                    
                    switch (dao.action){
                        
                        case "deplacer":
                            if(dao.world.id == ""){
                                Deplacer deplacement = GameObject.Find(dao.characters[0]).GetComponent<Deplacer> ();
                                deplacement.dest = GameObject.Find(dao.characters[1]).transform.position;
                                Debug.Log ("On se déplace !!");
                            }else{
                                Deplacer deplacement = GameObject.Find(dao.characters[0]).GetComponent<Deplacer> ();
                                deplacement.dest = GameObject.Find(dao.world.regions[0]).transform.position;
                            }
                            break;
                        
                        case "discuter":
                            //ajouter discussion ici
                            break;
                        
                        default:
                            Debug.Log("Commande d'action non reconnue");
                            break;
                    }
                    break;
                
                case "sys":
                    
                    switch (dao.action){
                        
                        case "exit":
                            Application.Quit ();
                            UnityEditor.EditorApplication.isPlaying = false;
                            break;
                        
                        case "reset":
                            Scene scene = SceneManager.GetActiveScene(); 
                            SceneManager.LoadScene(scene.name);
                            break;
                        
                        case "load":
                            SceneManager.LoadScene(dao.world.id);  
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

