using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ESP32GPIOSender : MonoBehaviour
{
    UdpClient udpClient;
    IPEndPoint esp32Endpoint;

    // IP and Port (match your ESP32 setup)
    string esp32IP = "192.168.0.255"; // Use actual IP if not using broadcast
    int esp32Port = 4210;

    public FingertipCollisionManager dataSender;
    void Start()
    {
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;

        esp32Endpoint = new IPEndPoint(IPAddress.Parse(esp32IP), esp32Port);
        Debug.Log("ESP32 UDP Sender ready.");
    }

    void Update()
    {
        SendControlSignal(dataSender.binaryString);
    }

    void SendControlSignal(string signal)
    {
        byte[] data = Encoding.ASCII.GetBytes(signal);
        udpClient.Send(data, data.Length, esp32Endpoint);
        Debug.Log($"Sent control signal: {signal}");
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
    }
}
