//Script for handling lobby and room panels
//Lobby contains list of all rooms as well as room creation
//Room contains player list, ready information and start button

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineMenuScript : MonoBehaviour {

	public GameObject lobbyPanel, roomPanel;
	public void DisableMainPanels()
	{
		if(lobbyPanel.activeSelf)
			lobbyPanel.SetActive(false);
		if(roomPanel.activeSelf)
			roomPanel.SetActive(false);
	}

	public void EnablePanel(string panel)
	{
		DisableMainPanels();
		
		if(panel == "lobby")
		{
			if(!lobbyPanel.activeSelf)
				lobbyPanel.SetActive(true);
		}
		else if(panel == "room")
		{
			if(!roomPanel.activeSelf)
				roomPanel.SetActive(true);
		}
	}

	public void LoadLobby()
	{
		DisableCreateServerPrompt();
		EnablePanel("lobby");
	}

	public GameObject roomStartButton;
	public void LoadRoom()
	{
		if(GameManager.Instance().isDM)
			roomStartButton.SetActive(true);
		else
			roomStartButton.SetActive(false);

		DisableCreateServerPrompt();
		EnablePanel("room");
	}

	public GameObject ServerContentBox;
	private List<ServerObjectScript> m_ServerObjectList = new List<ServerObjectScript>();
	public void PopulateLobby()
	{
		RoomInfo[] m_Hosts = NetworkManager.Instance().GetHostList();
		
		if(m_Hosts.Length > m_ServerObjectList.Count)// if too little, add more
		{
			int numToMake = m_Hosts.Length - m_ServerObjectList.Count;
			for(int i = 0; i < numToMake; i++)
			{
				AddServerObject();
			}
		}
		else if(m_Hosts.Length < m_ServerObjectList.Count)// if too many, remove some
		{
			int numToKill = m_Hosts.Length - m_ServerObjectList.Count;
			for(int i = 0; i < numToKill; i++)
			{
				RemoveServerCharacterObject();
			}
		}
		
		for(int i = 0; i < m_ServerObjectList.Count; i++)// set each object
		{
			if(m_Hosts.Length > i)
				m_ServerObjectList[i].SetRoomReference(m_Hosts[i]);
		}

		Utilities.UI.UpdateContentBoxSize(ServerContentBox, m_ServerObjectList.Count, 110f);
		m_RefreshTimer = Time.time + NetworkManager.Instance().GetHostListRefreshInterval();
	}

	public void AddServerObject()
	{
		GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/ServerObject"),
		                                                   Vector3.zero, Quaternion.identity);
		
		go.transform.SetParent(ServerContentBox.transform, false);
		go.transform.localPosition = new Vector2(0, (m_ServerObjectList.Count * -110) + 0);
		
		m_ServerObjectList.Add(go.GetComponent<ServerObjectScript>());
	}
	
	public void RemoveServerCharacterObject()
	{
		ServerObjectScript sos = m_ServerObjectList[m_ServerObjectList.Count - 1];
		GameObject go = sos.gameObject;
		
		m_ServerObjectList.Remove(sos);
		Destroy(go);
	}

	public GameObject createServerPanel;
	public void EnableCreateServerPrompt()
	{
		if(!createServerPanel.activeSelf)
			createServerPanel.SetActive(true);
	}

	public void DisableCreateServerPrompt()
	{
		if(createServerPanel.activeSelf)
			createServerPanel.SetActive(false);
	}

	public void StartGame()
	{
		NetworkManager.Instance().NMPhotonView.RPC("StartGame", PhotonTargets.All);
	}

	// Use this for initialization
	void Start () {
		LoadLobby();
	}
	
	private float m_RefreshTimer = 0;
	void FixedUpdate () {
		if(lobbyPanel.activeSelf)
		{
			if(m_RefreshTimer < Time.time)
				PopulateLobby();

			if(PhotonNetwork.inRoom)
				LoadRoom();
		}
		else if(roomPanel.activeSelf)
		{
			if(!PhotonNetwork.inRoom)
				LoadLobby();
		}
	}
}
