using System.Collections;
using UnityEngine;


namespace JSONParser 
{
    public class JSON
    {
        public static void ParseJSON(string msg){

            var dao=new DAO();
            dao.Deserialize(msg);
            Debug.Log(dao.type);
            Debug.Log(dao.action);
            
            switch (dao.type){
      
                case "cmd":
                    
                    switch (dao.action){
                        
                        case "deplacer":
                            if(dao.world==null){
                                Deplacer deplacement = GameObject.Find(dao.characters[0]).GetComponent<Deplacer> ();
                                deplacement.dest = GameObject.Find(dao.characters[1]).transform.position;
                            }else{
                                Deplacer deplacement = GameObject.Find(dao.characters[0]).GetComponent<Deplacer> ();
                                deplacement.dest = GameObject.Find(world.region[0]).transform.position;
                            }
                            break;
                        
                        case "discuter":
                             //ajouter discussion ici
                             break;
                        
                        default:
                            Debug.Log("Commande d'action non reconnue");
                    }
                    break;
                
                case "sys":
                    
                    switch (dao.action){
                        
                        case "shutdown":
                            comm.SendCloseMessage();
                        
                        case "reset":
                            Scene scene = SceneManager.GetActiveScene(); 
                            SceneManager.LoadScene(scene.name);
                            break;
                        
                        case "load":
                            SceneManager.LoadScene(dao.world.id);                            

                        default:
                            Debug.Log("Commande système non reconnue");   
                    }
                    break;

                default:
                     Debug.Log("Commande non reconnue");
                    break;
            } 
             
        }
    }  
}

