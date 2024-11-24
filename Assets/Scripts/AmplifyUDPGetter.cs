using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class AmplifyUDPGetter : MonoBehaviour {

    public GameObject amplifySlider;
    private SystemController systemController;
    public string UDP_LISTEN_IP;
    public int UDP_LISTEN_PORT;
    private float amplify;
    private UdpClient udpClient;
    private Thread receiveAmplifyThread;

	// Start is called before the first frame update
	void Start() {
        systemController = GameObject.Find("System").GetComponent<SystemController>();

        Debug.Log("Starting to Receive UDP Pitch...");
        amplify = 0.0f;
        UDP_LISTEN_IP = "127.0.0.1";
        UDP_LISTEN_PORT = 5006;
        udpClient = new UdpClient(UDP_LISTEN_PORT, AddressFamily.InterNetwork); // 監聽端口
        receiveAmplifyThread = new Thread(new ThreadStart(ReceiveAmplifyData));
        receiveAmplifyThread.IsBackground = true;
        receiveAmplifyThread.Start();

        Debug.LogWarning("Starting to Receive UDP Amplify at port 5006... Done");
    }

	private void FixedUpdate() {
        systemController.amplify = amplify;
	}

	void OnDestroy() {
        if (receiveAmplifyThread != null) {
            receiveAmplifyThread.Abort();
        }
        udpClient.Close();
    }

    private void ReceiveAmplifyData() {
        while (true) {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(UDP_LISTEN_IP), UDP_LISTEN_PORT);
            var receivedResults = udpClient.Receive(ref remoteEndPoint);
            string message = Encoding.UTF8.GetString(receivedResults);
            float value = float.Parse(message);
            Debug.LogWarning("Received pitch UDP message: " + value);
            amplify = value;
        }
    }
}
