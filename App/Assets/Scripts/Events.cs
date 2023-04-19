/* 
    *********************************************************
    In this script is defined all backend Network functions 
        Install-Package System.Json -Version 4.0.20126.16343
    *********************************************************
*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;

public class Events : MonoBehaviour
{
    public Networking networkBehaviour;
    
    public string id = "";  // _id assigned by the server
    public bool readingFromServer = false;
    public bool writingToServer = false;
    public bool error = false;
    
    public string JSONPackage = "";
    public JsonData JSONPackageReceived = new JsonData();

    void Awake()
    {
        networkBehaviour = FindObjectOfType<Networking>();
        networkBehaviour.IP = "127.0.0.1"; // Server IP address
        networkBehaviour.PORT = 8080; // Server port
    }

    // Receive a command from server and do ...
    public void readAction(string JsonFromServer)
    {
        Debug.Log("The message from server is: " + JsonFromServer);
        if(JsonFromServer.StartsWith("id:"))
        {
            id = JsonFromServer.Replace("id: ", "");
            Debug.Log("Player ID from server received");
            Debug.Log("My player ID is: " + id);
        }
        else
        {
            // Command deserialization
            JSONPackageReceived = JsonUtility.FromJson<JsonData>(JsonFromServer);
            switch (JSONPackageReceived.getCommand())
            {
                case "MESSAGE_FROM_SERVER_1":
                    Debug.Log("I've received the message 1...");
                break;

                case "MESSAGE_FROM_SERVER_2":
                    Debug.Log("I've received the message 2...");
                break;

                case "MESSAGE_FROM_SERVER_3":
                    Debug.Log("I've received the message 3...");
                break;

                case "MESSAGE_FROM_SERVER_N":
                    Debug.Log("I've received the message N...");
                break;

                default:
                    Debug.Log("No valid command...");
                break;
            }
        }
    }

    // Send a serialized object to server ...
    public void sendRoomAction(string sendJson)
    {
        writingToServer = true;
        networkBehaviour.stream.BeginWrite(Encoding.UTF8.GetBytes(sendJson), 0, sendJson.Length, new AsyncCallback(endWritingProcess), networkBehaviour.stream);
        networkBehaviour.stream.Flush();
    }

    public void sendAction(string sendJson)
    {
        if(writingToServer)
            return;
        try
        {
            if(!error)
            {
                writingToServer = true;
                networkBehaviour.stream.BeginWrite(Encoding.UTF8.GetBytes(sendJson), 0, sendJson.Length, new AsyncCallback(endWritingProcess), networkBehaviour.stream);
                networkBehaviour.stream.Flush();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Exception Message: " + ex.Message);
            error = true;
        }
    }

    void endWritingProcess(IAsyncResult _IAsyncResult)
    {
        writingToServer = false;
        networkBehaviour.stream.EndWrite(_IAsyncResult);
    }

    private void Update()
    {
        if(networkBehaviour.isRunning)
        {
            if(networkBehaviour.stream.DataAvailable)
            {
                readingFromServer = true;
                networkBehaviour.stream.BeginRead(networkBehaviour.data, 0, networkBehaviour.data.Length, new AsyncCallback(endReadingProcess), networkBehaviour.stream);
            }

            /*
                ...
                    Write your own functions
                ...

                JSONPackageObject = JsonUtility.ToJson(object, true);
                sendRoomAction(JSONPackageObject);
            */
        }
    }

    void endReadingProcess(IAsyncResult _IAsyncResult)
    {
        readingFromServer = false;
        int size = networkBehaviour.stream.EndRead(_IAsyncResult);
        string action = Encoding.UTF8.GetString(networkBehaviour.data, 0, size);
        readAction(action);
    }

    private void OnApplicationQuit()
    {
        networkBehaviour.isRunning = false;
    }
}