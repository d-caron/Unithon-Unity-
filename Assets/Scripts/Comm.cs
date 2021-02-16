using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Comm : MonoBehaviour
{
    private TcpClient socketConnection;
    private Thread listenThread;
    
    public string host = "localhost";
    public int port;
    public string message;

    private void Start ()
    {
        ConnectAndStartThread();
    }

    private void ConnectAndStartThread() {
        ConnectToTCPServer ();
        listenThread = new Thread (new ThreadStart (ListenForData));
        listenThread.Start();
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
            } else {
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
                while (((length = stream.Read (bytes, 0, bytes.Length)) != 0) && IsConnected())
                {
                    byte[] receivedData = new byte[length];
                    Array.Copy(bytes, 0, receivedData, 0, length);
                    string msg = Encoding.ASCII.GetString (receivedData);
                    
                    // Si on reçoit "Close_Python" on ferme la socket
                    if (msg.Equals("Close_Python")) {
                        socketConnection.Close ();
                    } else {
                        Debug.Log (msg);
                    }
                }
                if(!IsConnected()) {
                    Debug.Log("La socket est déconnecté");
                }
            }
        }
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


    // Fonction de fermeture de la socket
    public void CloseTCPClient()
    {
        // Envoie un message à Python pour indiquer que Unity se ferme seulement si la connexion est toujours en cours
        if(IsConnected()){
            SendCloseMessage();
            Debug.Log("Envoie de message de fermeture à Python");
        }
        
        try {
            socketConnection.Close ();
            Debug.Log ("Connexion closed !");
        } catch {
            // Socket déjà fermé
        }
    }

    // Fonction qui permet de savoir si la socket est toujours conencté ou non (trouvé sur : https://stackoverflow.com/questions/6993295/how-to-determine-if-the-tcp-is-connected-or-not)
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
