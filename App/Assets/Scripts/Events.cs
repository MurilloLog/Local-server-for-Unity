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
    //public ConectionStatus conectionStatus;
    
    public string id = "";  // _id assigned by the server
    public bool readingFromServer = false;
    public bool writingToServer = false;
    //public bool searchingRoom = false;
    //public bool searchingPlayer = false;
    //public bool paired = false;
    public bool error = false;
    //public bool updatingPlayerPose = false;
    //public bool updatingObjectPose = false;
    //public bool targetIsFound = false;

    //public SearchRoom searchRoom = new SearchRoom();
    //public PlayerPose playerPose = new PlayerPose();
    //public ObjectPose objectPose = new ObjectPose();
    //public ObjectPose objectPose;
    //public GameObject playerFrame;
    //public GameObject objectPrefab;

    public string JSONPackage = "";
    public JsonData JSONPackageReceived = new JsonData();

    void Awake()
    {
        networkBehaviour = FindObjectOfType<Networking>();
        //conectionStatus = FindObjectOfType<ConectionStatus>();
        networkBehaviour.IP = "127.0.0.1"; // Server IP address
        networkBehaviour.PORT = 8080; // Server port
        //conectionStatus.playerIsAlone = false;
        //conectionStatus.playerIsWaiting = true;
    }

    //public void changeTargetStatus() { targetIsFound = !targetIsFound; }

    // Receive a command from server and do ...
    public void readAction(string JsonFromServer)
    {
        Debug.Log("The message from server is: " + JsonFromServer);
        if(JsonFromServer.StartsWith("id:"))
        {
            id = JsonFromServer.Replace("id: ", "");
            Debug.Log("Player ID from server received");
            Debug.Log("My player ID is: " + id);
            //searchingRoom = true;
            //searchRoom.setCommand("SEARCH_ROOM");
            //searchRoom.setPlayerID(id);
            //JSONPackage = JsonUtility.ToJson(searchRoom, true);
            //Debug.Log("The message to send is: " + JSONPackage);
            //sendRoomAction(JSONPackage);
        }
        else
        {
            // Command deserialization
            JSONPackageReceived = JsonUtility.FromJson<JsonData>(JsonFromServer);
            switch (JSONPackageReceived.getCommand())
            {
                case "MESSAGE_FROM_SERVER_1":
                    //searchingPlayer = true;
                    //paired = false;
                    //conectionStatus.playerIsWaiting = true;
                    Debug.Log("I've received the message 1...");
                break;

                case "MESSAGE_FROM_SERVER_2":
                    //paired = true;
                    //searchingPlayer = false;
                    //searchingRoom = false;
                    //conectionStatus.playerIsWaiting = false;
                    Debug.Log("I've received the message 2...");
                break;

                case "MESSAGE_FROM_SERVER_3":
                    //Debug.Log("Posicion: " + JSONPackageReceived.getPosition());
                    //Debug.Log("Rotacion: " + JSONPackageReceived.getRotation());
                    //updatingPlayerPose = true;
                    Debug.Log("I've received the message 3...");
                break;

                case "MESSAGE_FROM_SERVER_N":
                    //updatingObjectPose = true;
                    //UpdateEnvironment();
                    Debug.Log("I've received the message N...");
                    //updatingObjectPose = false;
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
            if(paired && !error)
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
            /*else
            {
                if (paired && targetIsFound)
                {
                    UpdatePlayerPose();
                    //UpdateEnvironment();
                }
            }*/
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

    private void UpdatePlayerPose() 
    {
        playerPose.poseUpdate();
        if(playerPose.getPreviousMovement() != playerPose.getCurrentMovement())
        {
            playerPose.setPreviousMovement();
            playerPose.setCommand("UPDATE_PLAYER_POSE");
            playerPose.setPlayerID(id);
            JSONPackage = JsonUtility.ToJson(playerPose, true);
            sendAction(JSONPackage);
            if(playerPose.isFirstPose())
            {
                playerFrame.name = "playerFrame";
                playerFrame = (GameObject) Instantiate(playerFrame);
                playerPose.setFirstPose();
            }
        }
    }

    private void UpdateEnvironment()
    {
        // Searching for a GameObject
        //GameObject currentObject = new GameObject();
        //float smooth = 10.0f;
        //currentObject = GameObject.Find(JSONPackageReceived.getID());

        if( Lean.Common.LeanSpawn.objects.ContainsKey(JSONPackageReceived.getID()) )
        {
            /*GameObject obj = Lean.Common.LeanSpawn.objects[JSONPackageReceived.getID()];
            obj.transform.position = Vector3.Slerp(obj.transform.position, JSONPackageReceived.getPosition(), Time.deltaTime * smooth);
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, JSONPackageReceived.getRotation(),  Time.deltaTime * smooth);
            */Debug.Log("There is a GameObject with this ID");
        }
        else
        {
            Debug.Log("There is not a GameObject with this ID");
            //var clone = Instantiate(objectPrefab, JSONPackageReceived.getPosition(), JSONPackageReceived.getRotation());
            //objectPrefab = (GameObject) Instantiate(objectPrefab);//, JSONPackageReceived.getPosition(), JSONPackageReceived.getRotation());
            //clone.gameObject.name = JSONPackageReceived.getID();
            //objectPrefab.gameObject.SetActive(true);
            objectPrefab = (GameObject) Instantiate(objectPrefab);//, JSONPackageReceived.getPosition(), JSONPackageReceived.getRotation())
            Lean.Common.LeanSpawn.objects.Add(JSONPackageReceived.getID(), objectPrefab.gameObject);
            Debug.Log("The new object was created");
            //obj.AddComponent<UnityEngine.MeshRenderer>();
            
            //Lean.Common.LeanSpawn.objects.Add(JSONPackageReceived.getID(), obj.gameObject);*/
            
        }

        /*if(currentObject == null)
        {
            currentObject.name = JSONPackageReceived.getID();
            currentObject.AddComponent<UnityEngine.MeshRenderer>();
            currentObject = (GameObject) Instantiate(currentObject);
            
            currentObject.transform.position = Vector3.Slerp(currentObject.transform.position, JSONPackageReceived.getPosition(), Time.deltaTime * smooth);
            currentObject.transform.rotation = Quaternion.Slerp(currentObject.transform.rotation, JSONPackageReceived.getRotation(),  Time.deltaTime * smooth);
            Debug.Log("Se creo el objeto del otro jugador.");
        }
        else
        {
            currentObject.transform.position = Vector3.Slerp(currentObject.transform.position, JSONPackageReceived.getPosition(), Time.deltaTime * smooth);
            currentObject.transform.rotation = Quaternion.Slerp(currentObject.transform.rotation, JSONPackageReceived.getRotation(),  Time.deltaTime * smooth);
            Debug.Log("Se actualizo el objeto del otro jugador.");
        }*/
        //JSONPackage = JsonUtility.ToJson(objectPose, true);
        //sendAction(JSONPackage);
    }
}