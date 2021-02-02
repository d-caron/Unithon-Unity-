using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Recevoir : MonoBehaviour
{
    private TcpClient socketConnection;
    private readonly string ip = "127.0.0.1";
    public int port = 8000;

    const string ACK = "ack";

    private string serverMessage;
    private Thread clientReceiveThread;

    private void Awake()
    {
    }

    private void Start()
    {
        ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        try
        {
            socketConnection = new TcpClient(ip, port);
            clientReceiveThread = new Thread(new ThreadStart(ListenForData)) { IsBackground = true };
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            throw new Exception("server not running : " + e);
        }
    }

    private void Update()
    {
        if (socketConnection != null && !socketConnection.Connected)
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            throw new Exception("connection disconnected");
        }

        if (serverMessage != null)
        {
            HandleReceive(serverMessage);
        }
    }


    private void ListenForData()
    {
        try
        {
            byte[] bytes = new byte[2048];
            while (true)
            {
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        serverMessage = Encoding.ASCII.GetString(incommingData);
                    }
                }
                while (serverMessage != null);
            }
        }
        catch (System.InvalidOperationException) { }
        catch (ThreadAbortException) { }
        catch (Exception e)
        {
            Debug.Log(e.GetType());
            Debug.Log(e.Message);
        }
    }

    public void ValidateReceive()
    {
        serverMessage = null;
        SendTCPMessage(ACK);
    }

    private void HandleReceive(string messageStr)
    {
        // Message<object> message = JsonUtility.FromJson<Message<object>>(messageStr);
        // Debug.Log(message.type);
        // ValidateReceive();
        // switch (message.type)
        // {
        //     case "Close":
        //         TcpClientExceptionHandler.HandleException("Quit");
        //         break;
        //     case "Broadcast":
        //         Message<BroadcastContent> broadcastMessage = JsonUtility.FromJson<Message<BroadcastContent>>(messageStr);
        //         gameManager.SetBroadcastText(broadcastMessage.content.message);
        //         break;
        //     case "CardsInitialisation":
        //         Message<CardsInitialisation> cardsInitMessage = JsonUtility.FromJson<Message<CardsInitialisation>>(messageStr);
        //         gameManager.InitCards(cardsInitMessage.content.deck, 
        //             cardsInitMessage.content.middleCards, cardsInitMessage.content.players);
        //         break;
        //     case "PlayerAttribution":
        //         Message<PlayerAttributionContent> playerAttribution = JsonUtility.FromJson<Message<PlayerAttributionContent>>(messageStr);
        //         gameManager.InitPlayer(playerAttribution.content.index, playerAttribution.content.name);
        //         break;
        //     case "PlayerTurn":
        //         Message<PlayerTurn> playerTurn = JsonUtility.FromJson<Message<PlayerTurn>>(messageStr);
        //         gameManager.PlayerTurn(playerTurn.content.index, 
        //             playerTurn.content.messageConcerned, playerTurn.content.messageNonConcerned);
        //         break;
        //     case "PlayerMove":
        //         Message<PlayerMove> playerMove = JsonUtility.FromJson<Message<PlayerMove>>(messageStr);
        //         gameManager.PlayerMove(playerMove.content.playerIndex, playerMove.content.playerCardIndex,
        //             playerMove.content.message);
        //         break;
        //     default:
        //         throw new InvalidOperationException("message type is not recognized");
        // }
    }


    public void SendTCPMessage(string clientMessage)
    {
        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }


    private void OnApplicationQuit()
    {
        if (clientReceiveThread != null) clientReceiveThread.Abort();
        if (socketConnection != null && socketConnection.Connected) CloseTcpClient();
    }

    public void CloseTcpClient()
    {
        socketConnection.Close();
        Debug.Log("Connexion closed !");
    }
}