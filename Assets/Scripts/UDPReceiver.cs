using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;

public class UDPReceiver : MonoBehaviour
{
    public GameObject receiver;
    public IUdpDataReceiver udpDataRecever;

    // 1. Declare Variables
    Thread receiveThread;  //�b�I�����򱵨�UDP�T��
    UdpClient client;   // parse the pre-defined address for data
    int port;   //port number

    // 2. Initialize variables
    void Start()
    {
        port = 8888;
        udpDataRecever = receiver.GetComponent<IUdpDataReceiver>();
        Debug.Log(udpDataRecever==null);
        InitUDP();
    }
    private void OnDestroy()
    {
        receiveThread.Abort();
    }

    // 3. InitUDP
    private void InitUDP()
    {
        print("UDP Initialized");
        receiveThread = new Thread(new ThreadStart(ReceiveData)); //�}�ӷs���a���Ѽƪ�thread�A�ǤJ��k��Ѽ�
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // 4. Receive Data
    private void ReceiveData()
    {
        client = new UdpClient(port); //���wport
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port); //����ip
                byte[] data = client.Receive(ref anyIP); //���
                udpDataRecever.DecodeData(data);
                /*
                Acc acc =new Acc();
                acc.x = System.BitConverter.ToSingle(data, 0);
                acc.y = System.BitConverter.ToSingle(data, 4);
                acc.z = System.BitConverter.ToSingle(data, 8);
                print(">> " + acc.x +" " +acc.y+" "+acc.z);*/
                //....
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }

    public T FromByteArray<T>(byte[] data)
    {
        //https://stackoverflow.com/questions/33022660/how-to-convert-byte-array-to-any-type
        if (data == null)
            return default(T);
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(data))
        {
            object obj = bf.Deserialize(ms);
            return (T)obj;
        }
    }
}

