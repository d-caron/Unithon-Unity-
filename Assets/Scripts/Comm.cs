using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using JSONParser;

public class Comm : MonoBehaviour
{
    private TcpClient socketConnection;
    private Thread listenThread;
    
    public string host = "localhost";
    public int port;
    public string message;
    public string action;

    private void Start ()
    {
        ConnectAndStartThread();
    }

    private void ConnectAndStartThread() {
        ConnectToTCPServer ();
        listenThread = new Thread (new ThreadStart (ListenForData));
        listenThread.Start();
    }

    private void Update (){
        TraiterMessage();
    }

    private void ConnectToTCPServer ()
    {
        try
        {
            socketConnection = new TcpClient (host, port);
            Debug.Log("Connexion à la socket réussie !");
        }
        catch (Exception)
        {
            Debug.Log("Connexion impossible à la socket !");
        }
    }

    public void SendTCPMessage () {
        try
        {
            if (IsConnected()) {
                NetworkStream stream = socketConnection.GetStream ();
                if (stream.CanWrite)
                {
                    byte[] messageAsByteArray = Encoding.ASCII.GetBytes (message);
                    stream.Write (messageAsByteArray, 0, messageAsByteArray.Length);
                    Debug.Log ("Ca marche !");
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
        try
        {
            NetworkStream stream = socketConnection.GetStream ();
            if (stream.CanWrite)
            {
                byte[] messageAsByteArray = Encoding.ASCII.GetBytes ("Close_Unity");
                stream.Write (messageAsByteArray, 0, messageAsByteArray.Length);
            }
        }
        catch (Exception e)
        {
            Debug.Log ("Exception : " + e);
        }
    }

    private void ListenForData ()
    {
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
                    string msg = Encoding.ASCII.GetString (receivedData);
                    // Si on reçoit "Close_Python" on ferme la sockets
                    if (msg.Equals("Close_Python")) {
                        Debug.Log("La socket a été déconnecté");
                        socketConnection.Close ();
                    } 
                    // Sinon c'est msg classique et on l'affiche
                    else {
                        
                        Debug.Log(msg);
                        JSON.ParseJSON(msg);
                        
                    }
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
    private void OnApplicationQuit()
    {
        CloseTCPClient ();
    }

    private void TraiterMessage(){
        if ( action == "haut") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (4, 0, 4);
        }

        
        if ( action == "droite") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (11, 0, 0);
        }

        
        if ( action == "bas") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (4, 0, -4);
        }

        if ( action == "gauche") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (0, 0, 0);
        }

        if ( action == "Ugo") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = GameObject.Find ("Ugo").transform.position;
        }

        action = "";
    }


    // Fonction de fermeture de la socket
    public void CloseTCPClient()
    {
        // Envoie un message à Python pour indiquer que Unity se ferme seulement si la connexion est toujours en cours
        if(IsConnected()){
            SendCloseMessage();
            Debug.Log("Envoie de message de fermeture à Python");
            socketConnection.Close ();
        }
    }

    // Fonction qui permet de savoir si la socket est toujours connecté ou non (trouvé sur : https://stackoverflow.com/questions/6993295/how-to-determine-if-the-tcp-is-connected-or-not)
    public bool IsConnected ()
    {
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
}
