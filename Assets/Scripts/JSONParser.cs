using System.Collections;
using UnityEngine;


namespace JSONParser 
{
    public class JSON
    {
        public static void ParseJSON(string msg){

            string[] ids = {"Michel","Ugo"};
           
            
            Debug.Log("hhhhh");
            Deplacer deplacement = GameObject.Find("Michel").GetComponent<Deplacer> ();
            deplacement.dest = GameObject.Find("Ugo").transform.position;
        }
    }  
}

