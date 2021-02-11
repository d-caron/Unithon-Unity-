/*
    */
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class Recevoir : MonoBehaviour {
       // receiving Thread
       Thread receiveThread;
       // udpclient object
       UdpClient client;
       public string IP = "127.0.0.1"; // default local
       public int port = 80; 
       string strReceiveUDP="";
       
       public void Start()
       {
          Application.runInBackground = true;
          init();   
       }
       
       // init
       private void init()
       {
          // define port
          port = 80;
          receiveThread = new Thread( new ThreadStart(ReceiveData));
          receiveThread.IsBackground = true;
          receiveThread.Start();
       }

       // receive thread
       private  void ReceiveData()
       {
          client = new UdpClient(port);
          while (true)
          {
             try
             {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                strReceiveUDP = text;
                Debug.Log(text);
             }
             catch (Exception err)
             {
                print(err.ToString());
             }
          }
       }

       public string UDPGetPacket()
       {
          return strReceiveUDP;
       }

       private void Update (){
          TraiterMessage();
       }

       private void TraiterMessage(){
         if ( strReceiveUDP == "haut") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (4, 0, 4);
         }

         
         if ( strReceiveUDP == "droite") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (11, 0, 0);
         }

         
         if ( strReceiveUDP == "bas") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (4, 0, -4);
         }

         
         if ( strReceiveUDP == "gauche") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = new Vector3 (0, 0, 0);
         }

         if ( strReceiveUDP == "Ugo") {
            Deplacer deplacement = GameObject.Find ("Michel").GetComponent<Deplacer> ();
            deplacement.dest = GameObject.Find ("Ugo").transform.position;
         }
       }

       void OnDisable()
       {
          if ( receiveThread!= null)   receiveThread.Abort();
          client.Close();
       }
    }