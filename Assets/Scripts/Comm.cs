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
    public string action;

    private void Start ()
    {
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
        }
        catch (Exception)
        {
            throw new Exception ("Server not running");
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
                    action = Encoding.ASCII.GetString (receivedData);
                    Debug.Log (action);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log ("Exception : " + e);
        }
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

    private void OnApplicationQuit()
    {
        CloseTCPClient ();
    }

    public void CloseTCPClient()
    {
        socketConnection.Close ();
        Debug.Log ("Connexion closed !");
    }
}
