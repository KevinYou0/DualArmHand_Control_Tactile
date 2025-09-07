using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class PiezoUDPScaler : MonoBehaviour
{
    private UdpClient udpClient;
    private Thread udpThread;
    private volatile float latestValue = 0f;
    private float currrentScale = 0.1f;

    public int listenPort = 4211;
    void Start()
    {
        udpClient = new UdpClient(listenPort);
        udpThread = new Thread(ReceiveUDP);
        udpThread.IsBackground = true;
        udpThread.Start();
        Debug.Log("UDP listener started on port" + listenPort);
    }

    void ReceiveUDP()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, listenPort);

        while (true)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string message = Encoding.ASCII.GetString(data);

                if (float.TryParse(message, out float value))
                {
                    latestValue = value;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("UDP receive error: " + e.Message);
            }
        }
    }

    void Update()
    {
        float normalized = Mathf.Clamp01(latestValue / 3.3f);
        float targetScale = Mathf.Lerp(0.1f, 1.5f, normalized);
        float smoothingFactor = 0.05f;

        currrentScale = Mathf.Lerp(currrentScale, targetScale, smoothingFactor);
        transform.localScale = new Vector3(currrentScale, currrentScale, currrentScale);

        // Map voltage (0–3.3V) to a scale factor (e.g., 0.1–1.5)
        //float scale = Mathf.Lerp(0.1f, 1.5f, Mathf.Clamp01(latestValue / 3.3f));
        //transform.localScale = new Vector3(scale, scale, scale);
    }

    void OnApplicationQuit()
    {
        udpThread?.Abort();
        udpClient?.Close();
    }
}
