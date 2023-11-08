using System.Collections;
using WebSocketSharp;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.UI;

public class MyData     //JSON �����͸� ���� Ŭ����
{
    public string clientID;
    public string message;
    public int requestType;
}



public class SocketClient : MonoBehaviour
{
    WebSocket webSocket;
    bool IsConnected = false;
    int ConnectionAttempt = 0;
    const int MaxConnectionAttempts = 3;

    public Text text;
    public InputField inputField;

    MyData sendData = new MyData { message = "�޼��� ������" };
    // Start is called before the first frame update
    void Start()
    {
        ConnectWebSock();
    }
    
    void ConnectWebSock()
    {
        webSocket = new WebSocket("ws://localhost:8000");           //8000��Ʈ�� ����
        webSocket.OnOpen += OnWebSocketOpen;                        //�Լ� �̺�Ʈ ���
        webSocket.OnMessage += OnWebSocketMessage;                        //�Լ� �̺�Ʈ ���
        webSocket.OnClose += OnWebSocketClose;                        //�Լ� �̺�Ʈ ���

        webSocket.ConnectAsync();
    }
    void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket connected");
        IsConnected = true;
        ConnectionAttempt = 0;
    }
    void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        string jsonData = Encoding.Default.GetString(e.RawData);
        Debug.Log("Received JSON data : " + jsonData);
        text.text = jsonData;

        //JSON �����͸� ��ü�� ������ȭ
        MyData receiveData = JsonConvert.DeserializeObject<MyData>(jsonData);

        if(receiveData != null && !string.IsNullOrEmpty(receiveData.clientID))
        {
            sendData.clientID = receiveData.clientID;
        }
    }
    void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed");
        IsConnected = false;

        if(ConnectionAttempt < MaxConnectionAttempts)
        {
            ConnectionAttempt++;
            Debug.Log("Attemping to reconnect . Attemp" + ConnectionAttempt);
            ConnectWebSock();
        }
        else
        {
            Debug.Log("Failed to connect ager " + MaxConnectionAttempts + "attempts. ");
        }
    }
    void OnAplicationQuit()             //���ø����̼� ����Ǹ� ��Ĺ ����
    {
        DisconnectWebSocket();
    }
    void DisconnectWebSocket()
    {
        if(webSocket != null && IsConnected)
        {
            webSocket.Close();
            IsConnected = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(webSocket == null || !IsConnected)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //JSON ������ ����
            sendData.requestType = 0;
            //sendData.message
            string jsonData = JsonConvert.SerializeObject(sendData);

            webSocket.Send(jsonData);
        }
    }
    public void SendSocketMessage()
    {
        sendData.requestType = 0;
        sendData.message = inputField.text;
        string jsonData = JsonConvert.SerializeObject(sendData);

        webSocket.Send(jsonData);
    }
}
