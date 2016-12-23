﻿/*
 * This is the network manager singleton script that's attached to a donotdestroyonload object.
 * It is used to handle all Photon network function calls 
 * */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	private static NetworkManager m_Instance = null;
	public static NetworkManager Instance()
	{
		if (m_Instance == null) 
		{
			m_Instance = GameObject.FindObjectOfType<NetworkManager>();
		}
		
		return m_Instance;
	}

	private PhotonView m_PhotonView;
	public PhotonView NMPhotonView
	{
		get
		{
			if(m_PhotonView == null)
				m_PhotonView = gameObject.GetPhotonView();
			return m_PhotonView;
		}
		set{ m_PhotonView = value; }
	}

    //Setup for default network room properties. Nothing important is stored within yet.
	private property[] m_DefaultProperties = new property[2]
	{ new property("Poop", "Poop"), new property("Pooping", true) };

	public void CreateServer(string name)
	{
		LeaveRoom(); //Leave current room check
		
		RoomOptions roomOptions = new RoomOptions(){ 
			CustomRoomProperties = CreateHashtable(m_DefaultProperties) };

		if(name != "")
			PhotonNetwork.CreateRoom(name, roomOptions, TypedLobby.Default);
		else
		{
			if(GetMyPlayer().NickName != "")
				PhotonNetwork.CreateRoom(GetMyPlayer().NickName + "'s room", roomOptions, TypedLobby.Default);
			else
				PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
		}

		SetMyPlayerCustomProperty("isDM", true);
		GameManager.Instance().isDM = true;
	}
	
	public void JoinRoom(RoomInfo room)
	{
		PhotonNetwork.JoinRoom(room.Name);
		//SetStatus("InRoom");
	}
	
	public void JoinRoom(string name)
	{
		PhotonNetwork.JoinRoom(name);
		//SetStatus("InRoom");
	}
	
	public void LeaveRoom()
	{
		if(PhotonNetwork.inRoom)
			PhotonNetwork.LeaveRoom();
	}

	private float m_HostListRefreshInterval = 3.0f;
	public float GetHostListRefreshInterval(){ return m_HostListRefreshInterval; }
	
	public RoomInfo[] GetHostList()
	{
		return PhotonNetwork.GetRoomList();
	}

	public PhotonPlayer[] GetPlayerList()
	{
		return PhotonNetwork.playerList;
	}
	
	public PhotonPlayer GetMyPlayer()
	{
		return PhotonNetwork.player;
	}
	
    //Room Custom Properties
	public void ResetRoomCustomProperties()
	{
		SetMyRoomCustomProperty("RaceFinished", false);
	}

	public void SetMyRoomCustomProperty(string name, object val)
	{
		PhotonNetwork.room.SetCustomProperties(CreateHashtable(name, val));
	}
	
	public object GetMyRoomCustomProperty(string name)
	{
		return PhotonNetwork.room.CustomProperties[name];
	}

    //Player Custom Properties
    public void ResetPlayerCustomProperties()
    {
        SetMyPlayerCustomProperty("isDM", false);
        GameManager.Instance().isDM = false;
    }

    public void SetMyPlayerCustomProperty(string name, object val)
	{
		GetMyPlayer().SetCustomProperties(CreateHashtable(name, val));
	}
	
	public object GetMyPlayerCustomProperty(string name)
	{
		return GetMyPlayer().CustomProperties[name];
	}

	public Room GetCurrentRoom()
	{
		return PhotonNetwork.room;
	}
	
	public void Connect()
	{
		if(!PhotonNetwork.connected)
		{
			PhotonNetwork.ConnectUsingSettings(GameManager.Instance().GetVersion().ToString());
			ResetPlayerCustomProperties();
		}
	}
	
	public void Disconnect()
	{
		if(PhotonNetwork.connected)
			PhotonNetwork.Disconnect();
	}

	public ExitGames.Client.Photon.Hashtable CreateHashtable(string name, object val)
	{
		return new ExitGames.Client.Photon.Hashtable() {{ name, val}};
	}
	
	private ExitGames.Client.Photon.Hashtable CreateHashtable(property[] properties)
	{
		ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
		
		foreach(property p in properties)
		{
			ht.Add(p.m_Name, p.m_Value);
		}
		
		return ht;
	}

    // Singleton initialization
    void Awake()
	{
		if(m_Instance)
			DestroyImmediate(gameObject);
		else
		{
			DontDestroyOnLoad(this);
		}
	}

	private class property
	{
		public string m_Name;
		public object m_Value;
		
		public property(string name, object val)
		{
			m_Name = name;
			m_Value = val;
		}
	}

	//------------------------------------------ RPC Calls ------------------------------
	
	[PunRPC]
	public void StartGame()
	{
		Debug.Log("Game Started Via Network");
		
		SceneManager.LoadScene("BuildingMode");
	}

	[PunRPC]
	public void LoadStage(StageSaveData stageData)
	{
		Debug.Log("Load Stage Via Network");
		LevelManager.Instance().LoadStage(stageData, CharacterConnection.remote);
	}

	[PunRPC]
	public void ReceiveStageData(byte[] data, bool first, bool last)
	{
		LevelManager.Instance().RecieveStageData(data, first, last);
	}

	[PunRPC]
	public void MovePiece(Vector3 oldPos, Vector3 newPos)
	{
		Debug.Log("Move Piece Via Network");
		BuildingManager.Instance().MovePiece(oldPos, newPos);
	}

	[PunRPC]
	public void DeletePiece(Vector3 pos)
	{
		BuildingManager.Instance().DeletePiece(pos);
	}

	[PunRPC]
	public void CreateBlock(Vector3 pos, int blockType)
	{
		BuildingManager.Instance().CreateBlock(pos, (StageBlockType)blockType);
	}

	[PunRPC]
	public void CreateCharacterPiece(Vector3 pos, Character character)
	{
		BuildingManager.Instance().CreateCharacterPiece(pos, character, CharacterConnection.remote);
	}

	[PunRPC]
	public void SetCharacterValue(string characterKey, string statKey, byte[] value)
	{
		object obj = PhotonCustomSerialize.DeserializeObject(value);
		GameManager.Instance().GetLoadedStageCharacter(characterKey).SetHashtableValue(statKey, obj, true);
	}
}

public enum CharacterConnection
{
    local,
    remote
}
