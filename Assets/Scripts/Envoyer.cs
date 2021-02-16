using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Envoyer : MonoBehaviour
{
    private TcpClient socketConnection;
    
    public string host = "localhost";
    public int port;
    public string message;

    private void Start ()
    {
        ConnectToTCPServer ();
        SendTCPMessage (message);
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

    private void SendTCPMessage (string message) {
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
        catch (SocketException socketException)
        {
            Debug.Log ("Socket exception : " + socketException);
        }
    }

    private void OnApplicationQuit()
    {
        CloseTCPClient ();
    }

    public void CloseTCPClient()
    {
        try {
            socketConnection.Close ();
            Debug.Log ("Connexion closed !");
        }
        catch(Exception) {

        }
    }
}
