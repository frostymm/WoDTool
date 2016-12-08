using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerObjectScript : MonoBehaviour {

	public Text chronicleNameText;

	public void OnClick()
	{
		NetworkManager.Instance().JoinRoom(m_Room);
	}

	private RoomInfo m_Room;
	public void SetRoomReference(RoomInfo Room)
	{
		m_Room = Room;
		chronicleNameText.text = Room.name;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
