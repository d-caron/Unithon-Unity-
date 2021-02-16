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
        ConnectToTCPServer ();
        listenThread = new Thread (new ThreadStart (ListenForData));
        listenThread.Start();
    }

    private void ConnectToTCPServer ()
    {
        try
        {
            socketConnection = new TcpClient (host, port);
        }
        catch (Exception e)
        {
            throw new Exception ("Server not running : " + e);
        }
    }

    public void SendTCPMessage () {
        try
        {
            NetworkStream stream = socketConnection.GetStream ();
            if (stream.CanWrite)
            {
                byte[] messageAsByteArray = Encoding.ASCII.GetBytes (message);
                stream.Write (messageAsByteArray, 0, messageAsByteArray.Length);
                Debug.Log ("Ca marche !");
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
                while ((length = stream.Read (bytes, 0, bytes.Length)) != 0)
                {
                    byte[] receivedData = new byte[length];
                    Array.Copy(bytes, 0, receivedData, 0, length);
                    string msg = Encoding.ASCII.GetString (receivedData);
                    
                    // Si on reçoit "Close_Python"
                    if (msg.Equals("Close_Python")) {
                        socketConnection.Close ();
                    } else {
                        Debug.Log (msg);
                    }
                }
            }
        }
        catch (Exception)
        {
            Debug.Log ("Le serveur python a été perdu !");
            socketConnection.Close ();
        }
    }

   // Event appelé quand l'application se ferme
    private void OnApplicationQuit()
    {
        // Envoie un message à Python pour indiquer que Unity se ferme
        SendCloseMessage();
        CloseTCPClient ();
    }


    // Fonction de fermeture de la socket
    public void CloseTCPClient()
    {
        SendCloseMessage ();
        Debug.Log("Envoie de message de fermeture à Python");
        try {
            socketConnection.Close ();
        } catch {
            // Socket déjà fermé
        }
        Debug.Log ("Connexion closed !");
    }
}
