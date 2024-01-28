using UnityEngine;
using WebSocketSharp;
using System.Threading;
using System;
using UnityEngine.Rendering;

public class StreamReceiver : MonoBehaviour
{
    [System.NonSerialized]
    public Texture2D leftEyeTexture;
    [System.NonSerialized]
    public Texture2D rightEyeTexture;
    private WebSocket ws;
    private BytesBuffer bytesBuffer = new BytesBuffer();
    private bool stopThread = false;
    private Thread webSocketThread; 
    private long imageSize;

    void Start()
    {   
        // Setup WebSocket connection in separate thread
        webSocketThread = new Thread(() =>
        {
            ws = new WebSocket("ws://localhost:8765");
            ws.OnMessage += (sender, e) =>
            {
                if (e.IsBinary)
                {   
                    bytesBuffer.Enqueue(e.RawData);
                    Debug.Log("ENQUEUED BYTES");
                }
            };
            ws.Connect();
            while (!stopThread)
            {
                if (ws.ReadyState == WebSocketState.Open)
                {
                    ws.Ping();
                }
                Thread.Sleep(100);
            }
        });
        webSocketThread.Start();
    }

    void Update()
    {   
        if (bytesBuffer.Count > 0)
        {
            if (bytesBuffer.TryDequeue(out byte[] bytes))
            {   
                // Extract image size (equal for left and right images)
                long imageSize = BitConverter.ToInt64(bytes, 0);
                // Extract the left and right images
                byte[] leftImageData = new byte[imageSize];
                byte[] rightImageData = new byte[imageSize];
                Array.Copy(bytes, 8, leftImageData, 0, imageSize);
                Array.Copy(bytes, 8 + imageSize, rightImageData, 0, imageSize);
                Debug.Log("bytes length: " + bytes.Length);
                Debug.Log("leftImageData length: " + leftImageData.Length);
                Debug.Log("rightImageData length: " + rightImageData.Length);
                Debug.Log("imageSize: " + imageSize);
                Texture2D leftTex = new Texture2D(2, 2);
                Texture2D rightTex = new Texture2D(2, 2);
                if (leftTex.LoadImage(leftImageData) && rightTex.LoadImage(rightImageData))
                {
                    Debug.Log("leftTex height: " + leftTex.height);
                    Debug.Log("rightTex height: " + rightTex.height);
                    leftEyeTexture = leftTex;
                    rightEyeTexture = rightTex;
                }
                else
                {
                    Debug.LogError("Left and/or right image bytes cannot be loaded into texture");
                }
            }            
        }
    }
    

    void OnDestroy()
    {
        stopThread = true;
        if (ws != null)
            ws.Close();

        if (webSocketThread != null && webSocketThread.IsAlive)
            webSocketThread.Join();
    }
}
