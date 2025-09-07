using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class IMUReceiverUDP : MonoBehaviour
{
    UdpClient client;
    Thread receiveThread;
    bool running = true;

    float[] imuData = new float[9];

    void Start()
    {
        try
        {
            // Force bind to IPv4 address family
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 4210);
            client = new UdpClient(localEndPoint);

            // Optional: set receive timeout (ms)
            client.Client.ReceiveTimeout = 5000;

            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            //Debug.Log("UDP Client started on port 4210, forced IPv4 bind.");
        }
        catch (System.Exception ex)
        {
            //Debug.LogError("UDP Init error: " + ex.Message);
        }
    }

    void ReceiveData()
    {
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

        while (running)
        {
            try
            {
                byte[] data = client.Receive(ref anyIP);

                //Debug.Log($"Received {data.Length} bytes from {anyIP}");

                string text = Encoding.UTF8.GetString(data);
                //Debug.Log($"Raw packet: {text}");

                string[] parts = text.Split(',');
                if (parts.Length == 9)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        imuData[i] = float.Parse(parts[i]);
                    }

                    //Debug.Log($"Parsed IMU: {imuData[3]}, {imuData[4]}, {imuData[5]} (gyro)");
                }
                else
                {
                    //Debug.LogWarning($"Received packet with unexpected format: {text}");
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    //Debug.LogWarning("UDP Receive timeout...");
                }
                else
                {
                    //Debug.LogError("UDP Receive error: " + ex.Message);
                }
            }
            catch (System.Exception ex)
            {
                //Debug.LogError("UDP General error: " + ex.Message);
            }
        }
    }

    public float[] GetIMUData()
    {
        return imuData;
    }

    void OnApplicationQuit()
    {
        running = false;
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
        if (client != null)
        {
            client.Close();
        }
    }
}
