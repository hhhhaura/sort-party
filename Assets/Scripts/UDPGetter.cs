using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPGetter : MonoBehaviour {

    public GameObject amplitudeSlider;
    private SystemController systemController;
    public string UDP_LISTEN_IP;
    public int UDP_LISTEN_PORT_AMPLITUDE;
    public int UDP_LISTEN_PORT_PITCH;
    private float amplitude;
    private float pitch;
    private UdpClient amplitudeUdpClient;
    private UdpClient pitchUdpClient;
    private Thread receiveAmplitudeThread;
    private Thread receivePitchThread;

	// Start is called before the first frame update
	void Start() {
        systemController = GameObject.Find("System").GetComponent<SystemController>();

        Debug.Log("Starting to Receive UDP Pitch...");
        UDP_LISTEN_IP = "127.0.0.1";
        
        amplitude = 0.0f;
        UDP_LISTEN_PORT_AMPLITUDE = 5006;
        amplitudeUdpClient = new UdpClient(UDP_LISTEN_PORT_AMPLITUDE, AddressFamily.InterNetwork); // 監聽端口
        receiveAmplitudeThread = new Thread(new ThreadStart(ReceiveAmplitudeData));
        receiveAmplitudeThread.IsBackground = true;
        receiveAmplitudeThread.Start();
        Debug.LogWarning("Starting to Receive UDP Amplitude at port " + UDP_LISTEN_PORT_AMPLITUDE + "... Done");
        
        pitch = 0.0f;
        UDP_LISTEN_PORT_PITCH = 5005;
        pitchUdpClient = new UdpClient(UDP_LISTEN_PORT_PITCH, AddressFamily.InterNetwork); // 監聽端口
        receivePitchThread = new Thread(new ThreadStart(ReceivePitchData));
        receivePitchThread.IsBackground = true;
        receivePitchThread.Start();
        Debug.LogWarning("Starting to Receive UDP Amplitude at port " + UDP_LISTEN_PORT_PITCH + "... Done");

    }

	private void FixedUpdate() {
        systemController.amplitude = amplitude;
        systemController.pitch = pitch;
	}

	void OnDestroy() {
        if (receiveAmplitudeThread != null) {
            receiveAmplitudeThread.Abort();
        }
        if (receivePitchThread != null) {
            receivePitchThread.Abort();
        }
        amplitudeUdpClient.Close();
        pitchUdpClient.Close();
    }

    private void ReceiveAmplitudeData() {
        while (true) {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(UDP_LISTEN_IP), UDP_LISTEN_PORT_AMPLITUDE);
            var receivedResults = amplitudeUdpClient.Receive(ref remoteEndPoint);
            string message = Encoding.UTF8.GetString(receivedResults);
            float value = float.Parse(message);
            Debug.LogWarning("Received amplitude UDP message: " + value);
            amplitude = value;
        }
    }

    private void ReceivePitchData() {
        while (true) {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(UDP_LISTEN_IP), UDP_LISTEN_PORT_PITCH);
            var receivedResults = pitchUdpClient.Receive(ref remoteEndPoint);
            string message = Encoding.UTF8.GetString(receivedResults);
            float value = float.Parse(message);
            Debug.LogWarning("Received pitch UDP message: " + value);
            pitch = value;
        }
    }
}
