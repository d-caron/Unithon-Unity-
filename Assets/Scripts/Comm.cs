using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Msg_manager.Manager;
using static DAO;
using UnityEngine.SceneManagement;


public class Comm : MonoBehaviour {
    private TcpClient socketConnection;
    private Thread listenThread;
    
    public string host = "localhost";
    public int port;
    private string msg_to_send;
    private string msg_to_recv = "";
    public string action;
    public GameObject[] characters;
    public GameObject[] regions;                     
    public String[] worlds;
    public static Comm comm;

    public DAO dao = new DAO ();
    private void Start (){
        ConnectAndStartThread();
    }

    private void ConnectAndStartThread() {
        ConnectToTCPServer ();
        listenThread = new Thread (new ThreadStart (ListenForData));
        listenThread.Start();
    }

    private void Update (){
        if (msg_to_recv.StartsWith ("{")) {
            Msg_manager.Manager.Recv_handler (msg_to_recv);
            msg_to_recv = "";
        } 
    }

    void Awake() {
       
         
        DontDestroyOnLoad(this);
        GameObject controller = GameObject.Find("GameController");
        DontDestroyOnLoad(controller);
        //DontDestroyOnLoad(this.gameObject);    
    }
    
    private void ConnectToTCPServer (){
        try
        {
            socketConnection = new TcpClient (host, port);

            UpdateInfos();

            Debug.Log("Connexion à la socket réussie !");
        }
        catch (Exception)
        {
            Debug.Log("Connexion impossible à la socket !");
        }
    }

    public void SendTCPMessage (){
        try
        {
            if (IsConnected()) {
               
                NetworkStream stream = socketConnection.GetStream ();
                if (stream.CanWrite)
                {
                    byte[] messageAsByteArray = Encoding.ASCII.GetBytes (msg_to_send);//envoyer cette liste à unity
                    stream.Write (messageAsByteArray, 0, messageAsByteArray.Length);
                }
            } 
            // Si on veut envoyer un message mais que la socket n'est pas connecté alors on essaye de se reconnecter à la socket
            else {
                Debug.Log("La socket est déconnecté, reconnexion en cours...");
                ConnectAndStartThread();
            }
        }
        catch (Exception e)
        {
            Debug.Log ("Exception : " + e);
        }
    }

    // Fonction qui envoie un message à Python pour indiquer que Unity se ferme
    public void SendCloseMessage() {
        DAO dao = new DAO ();
        dao.type = "sys";
        dao.action = "exit";
        msg_to_send = dao.Serialize ();
        SendTCPMessage ();
    }

    private void ListenForData (){
        try
        {
            byte[] bytes = new byte[2048];
            
            using (NetworkStream stream = socketConnection.GetStream ())
            {
                int length;
               
                // Si on est plus connecté à la socket alors on quitte la boucle
                while (((length = stream.Read (bytes, 0, bytes.Length)) != 0) && IsConnected())
                {
                    byte[] receivedData = new byte[length];
                    Array.Copy(bytes, 0, receivedData, 0, length);
                    msg_to_recv = Encoding.ASCII.GetString (receivedData);
                }
                if(!IsConnected()) {
                    Debug.Log("La socket est déconnecté");
                }
            }
        }
        // Jamais tombé dedans
        catch (Exception)
        {
            if(IsConnected()) {
                Debug.Log ("Le serveur python a été perdu !");
                socketConnection.Close ();
            }
            
        }
    }

   // Event appelé quand l'application se ferme
    private void OnApplicationQuit(){
        CloseTCPClient ();
    }


    // Fonction de fermeture de la socket
    public void CloseTCPClient(){
        // Envoie un message à Python pour indiquer que Unity se ferme seulement si la connexion est toujours en cours
        if(IsConnected()){
            SendCloseMessage();
            Debug.Log("Envoie de message de fermeture à Python");
            socketConnection.Close ();
        }
    }

    // Fonction qui permet de savoir si la socket est toujours connecté ou non (trouvé sur : https://stackoverflow.com/questions/6993295/how-to-determine-if-the-tcp-is-connected-or-not)
    public bool IsConnected (){
        try
        {
            if (socketConnection != null && socketConnection.Client != null && socketConnection.Client.Connected)
            {
            /* pear to the documentation on Poll:
                * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                * -or- true if data is available for reading; 
                * -or- true if the connection has been closed, reset, or terminated; 
                * otherwise, returns false
                */

                // Detect if client disconnected
                if (socketConnection.Client.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buff = new byte[1];
                    if (socketConnection.Client.Receive(buff, SocketFlags.Peek) == 0)
                    {
                        // Client disconnected
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                return true;
            }
            else 
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

   public void UpdateInfos(){
       
        dao.type = "info";
        dao.action = "add_characters";
        characters = GameObject.FindGameObjectsWithTag("character");
        dao.characters = new String[characters.Length];
        for (int i = 0; i < characters.Length; i ++) {
            dao.characters[i] = characters[i].name;
        }
        msg_to_send = dao.Serialize ();
        SendTCPMessage ();

        dao.action = "add_regions";
        regions = GameObject.FindGameObjectsWithTag("region");
        dao.world = new S_World ();
        dao.world.regions = new String[regions.Length];
        for (int i = 0; i < regions.Length; i ++) {
            dao.world.regions[i] = regions[i].name;
        }
        msg_to_send = dao.Serialize ();
        SendTCPMessage ();

        dao.action = "add_worlds";
        int sceneCount = SceneManager.sceneCountInBuildSettings;     
        worlds = new string[sceneCount];

        for( int i = 0; i < sceneCount; i++ ){
            worlds[i] = SceneUtility.GetScenePathByBuildIndex(i);
            worlds[i]=worlds[i].Remove(0,14);
             worlds[i]=worlds[i].Remove(worlds[i].Length - 6);
        }
        dao.world = new S_World ();
        dao.world.regions=worlds;
        msg_to_send = dao.Serialize ();
        SendTCPMessage ();
   }
      
}
