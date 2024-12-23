using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ServerManagement : MonoBehaviourPunCallbacks
{
    
    // Start is called before the first frame update
    void Start()
    {
        //1. connect to server
        //2. connect to lobby
        //3. connect to room 
        PhotonNetwork.ConnectUsingSettings(); // connect to server 
        /*
        PhotonNetwork.JoinRoom("room name"); // connect to room 
        PhotonNetwork.CreateRoom("room name", room_settings); // create room
        PhotonNetwork.JoinOrCreateRoom("room name", room_settings, TypedLobby.Default);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
         */
    }



    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the server");
        PhotonNetwork.JoinLobby(); // connect to lobby
        //It checks/controls the connection of the server

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to the lobby");
        PhotonNetwork.JoinOrCreateRoom("AkdenizCSRoom", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        //connect to random room or create 
        //it checks the connection of the Lobby
    }

    public override void OnJoinedRoom()
    {
        Quaternion treeRotation = Quaternion.Euler(50, -30, 0);
        Vector3 forest1position = new Vector3(1.6f, 2.8f, -0.4f);
        Vector3 forest2position = new Vector3(10f, 2.8f, 42.5f);
        Vector3 rocks1Position = new Vector3(213, -107.6f, -11.6f);
        Vector3 rocks2Position = new Vector3(118.4f, -37.8f, 149.9f);
        Debug.Log("Connected to the Room");
        Vector3 woodmanPosition = new Vector3(63, 0, 40);
        Vector3 nexusPosition = new Vector3(46.57f, 0, 40.56f);
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            woodmanPosition = new Vector3(106, 0, 74);
            nexusPosition = new Vector3(115.76f, 0, 84.7f);
            PhotonNetwork.Instantiate("Forest2", forest2position, treeRotation);
            PhotonNetwork.Instantiate("Rocks2", rocks2Position, Quaternion.identity);
        }
        PhotonNetwork.Instantiate("Nexus", nexusPosition, Quaternion.identity);
        PhotonNetwork.Instantiate("Woodman", woodmanPosition, Quaternion.identity);
        PhotonNetwork.Instantiate("Forest", forest1position, treeRotation);
        PhotonNetwork.Instantiate("Rocks", rocks1Position, Quaternion.identity);
        
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the Room");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left the Lobby");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not join any room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could not join any random room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not create room");
    }


}
