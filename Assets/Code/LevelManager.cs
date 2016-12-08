using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class LevelManager : MonoBehaviour {
	
	Dictionary<Vector3,StagePiece> m_Stage = new Dictionary<Vector3,StagePiece>();
	public Dictionary<Vector3,StagePiece> GetStage(){ return m_Stage; }

	Dictionary<Vector3,GameObject> m_StageGameObjects = new Dictionary<Vector3,GameObject>();
	public Dictionary<Vector3,GameObject> GetStageGameObjects(){ return m_StageGameObjects; }

	public List<StageCharacter> GetStageCharacterList()
	{
		List<StageCharacter> sChars;
		List<StagePiece> sPieces;

		if(GameManager.Instance().isDM)
			sPieces = m_Stage.Values.ToList().FindAll(x => x.m_Type == StagePieceType.character);
		else
			sPieces = m_Stage.Values.ToList().FindAll(x => x.m_Type == StagePieceType.character && !x.isHidden);
		
		sChars = sPieces.Cast<StageCharacter>().ToList();

		return sChars;
	}

	private bool m_StageCharacterListChanged = true;
	public bool stageCharacterListChanged{ get{ return m_StageCharacterListChanged; } set{ m_StageCharacterListChanged = value; } }
	public void AddStagePiece(Vector3 pos, StagePiece sp)
	{
		m_Stage.Add(pos, sp);

		if(sp.m_Type == StagePieceType.character)
			stageCharacterListChanged = true;
	}

	public void RemoveStagePiece(Vector3 pos)
	{
		if(m_Stage[pos].m_Type == StagePieceType.character)
			stageCharacterListChanged = true;

		m_Stage.Remove(pos);
	}

	private static LevelManager m_Instance = null;
	public static LevelManager Instance()
	{
		if (m_Instance == null) 
		{
			m_Instance = GameObject.FindObjectOfType<LevelManager>();
		}
		
		return m_Instance;
	}

	private RaycastHit m_MousePosition; //raycast for position of mouse on screen
	public RaycastHit GetMouseHit(){ return m_MousePosition; }
	public Vector3 CalcGridColSelectedPosition()
	{
		float x = Mathf.Floor((m_MousePosition.point.x + GameManager.blockWidth/2) / GameManager.blockWidth) * GameManager.blockWidth;
		float z = Mathf.Floor((m_MousePosition.point.z + GameManager.blockWidth/2) / GameManager.blockWidth) * GameManager.blockWidth;

		Vector3 pos = new Vector3(x, m_MousePosition.collider.transform.position.y, z);

		Debug.DrawLine(Camera.main.transform.position, pos);

		return pos;
	}

	private bool m_IsPositionSelected = false;
	public bool isPositionSelected
	{
		get{ return m_IsPositionSelected; }
		set{ m_IsPositionSelected = value; }
	}

	public GameObject m_HighlighterBox; //box to show highlighter (to select blocks)
	public GameObject GetHighlighterBox(){ return m_HighlighterBox; }

	private Vector3 m_SelectedPosition = new Vector3();
	public Vector3 GetSelectedPosition(){ return m_SelectedPosition; }
	public void SetSelectedPosition(Vector3 pos)
	{
		LevelManager.Instance().isPositionSelected = true;
		m_SelectedPosition = pos;
	}
	
	private void UpdateSelectedPosition(GameObject selectionBox)
	{	
		Vector3 selectedPosition = GetSelectedPosition();
		
		LevelManager.Instance().GetHighlighterBox().transform.position = selectedPosition;
	}

	private bool m_Dragging = false;
	public bool isDragging { get{ return m_Dragging; } set{ m_Dragging = value; } }

	private void OnSelectButtonPress(GameObject selectionBox)
	{
		Vector3 selectedPosition = LevelManager.Instance().GetSelectedPosition();
		
		if((GameManager.Instance().inBuildMode && BuildingManager.Instance().buildingMode == BuildingManager.BuildingMode.select)
		   || (!GameManager.Instance().inBuildMode))
		{
			//Check if clicking current position to remove selector
			if(selectionBox.transform.position != selectedPosition)
			{
				if(!selectionBox.activeSelf)
					selectionBox.SetActive(true);
				
				selectionBox.transform.position = selectedPosition;
			}
			else
			{
				if(!selectionBox.activeSelf)
					selectionBox.SetActive(true);
				else
					selectionBox.SetActive(false);
			}

			if(GetStage().ContainsKey(selectedPosition))
			{
				StagePiece sp = GetStage()[selectedPosition];
				if(sp.m_Type == StagePieceType.character)
				{
					StageCharacter sc = (StageCharacter)sp;
					GameManager.Instance().selectedCharacter = GameManager.Instance().GetLoadedStageCharacter(sc.m_CharacterName + sc.m_CharacterID);
				}
			}
		}
	}

	public GameObject m_SelectionBox; //box to show currently selected block(s)
	private void HandleSelectionBox()
	{
		if(isPositionSelected)
		{
			UpdateSelectedPosition(m_SelectionBox);

			if(Input.GetButtonDown("Select"))
			{
				OnSelectButtonPress(m_SelectionBox);
			}
		}

		if(GameManager.Instance().inBuildMode)
			BuildingManager.Instance().HandleSelectionBox(m_SelectionBox);
		else
		{
			if(isPositionSelected)
			{
				if(Input.GetButtonDown("Select"))
				{
					//OnSelectButtonPress(selectionBox);
				}
				if(Input.GetButtonDown("Deselect"))
				{
					//OnDeselectButtonPress(selectionBox);
				}
			}
		}
	}

	public void EnterBuildMode()
	{
		//GetHighlighterBox().SetActive(true);
	}

	public void ExitBuildMode()
	{
		BuildingManager.Instance().SetBuildingMode(BuildingManager.BuildingMode.select);
	}

	private void ClearCurrentStageVariables()
	{
		foreach(StagePiece sp in GetStage().Values)
		{
			sp.DestroyGameObject();
		}

		GetStage().Clear();
		GameManager.Instance().ClearLoadedCharacterFiles();
	}

	private string m_StageName = "default";
	public string stageName{ get { return m_StageName; } set { m_StageName = value; } }
	public void SaveStage()
	{
		if(!Directory.Exists(Application.persistentDataPath + "/StageData/"))
			Directory.CreateDirectory(Application.persistentDataPath + "/StageData/");

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/StageData/" + stageName + ".dat");
		
		StageSaveData data = new StageSaveData();
		//Set SaveData
		data.SetVariables();
		
		bf.Serialize(file, data);
		file.Close();
		
		if(GameManager.Instance().isDebugMode)
			Debug.Log("Saving File to: " + Application.persistentDataPath + "/StageData/" + stageName + ".dat");
	}
	
	private string m_StageToLoad = "";
	public void SetStageToLoad(string fileName){ m_StageToLoad = fileName; }
	public void LoadStage() // load own stage
	{
		/*if(GetStage().Count > 0)
		{
			DeleteCurrentStage();
		}*/
		StageSaveData stageData = new StageSaveData();
		if(File.Exists(Application.persistentDataPath + "/StageData/" + m_StageToLoad + ".dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/StageData/" + m_StageToLoad + ".dat", FileMode.Open);
			
			stageData = (StageSaveData)bf.Deserialize(file);
			file.Close();
			
			//Set SaveData
			//data.RetrieveVariables();
			
			Debug.Log("Data Loaded");
		}

		//CreateStage();
		LoadStage(stageData, CharacterConnection.local);

		if(!PhotonNetwork.offlineMode)
			BeginSendStageOverNetwork(stageData);
	}

	public void LoadStage(StageSaveData stageData, CharacterConnection conn) //Load stage on clients
	{
		ClearCurrentStageVariables();

		stageData.RetrieveVariables(conn);

		CreateStage();
	}

	public void LoadStage(List<byte[]> parts)
	{
		byte[] bytes = Utilities.CombineFile(parts);
		StageSaveData data = (StageSaveData)PhotonCustomSerialize.DeserializeObject(bytes);

		LoadStage(data, CharacterConnection.remote);
	}

	public void DeleteStage()
	{
		if(File.Exists(Application.persistentDataPath + "/StageData/" + m_StageToLoad + ".dat"))
		{
			File.Delete(Application.persistentDataPath + "/StageData/" + m_StageToLoad + ".dat");
		}
	}

	public void CreateStage()
	{
		foreach(StagePiece sp in GetStage().Values)
		{
			sp.CreateGameObject();
		}
	}

	private bool m_SyncingStageData = false;
	public bool isSyncingStageData
	{
		get{ return m_SyncingStageData; }
		set
		{
			m_SyncingStageData = value;

			HUD.GetHUD().SetSyncingStageData(m_SyncingStageData);
		}
	}

	public void RecieveStageData(byte[] data, bool first, bool last)
	{
		Debug.Log("Receiving Stage Data");

		if(first)
		{
			m_StageDataToLoad = new List<byte[]>();
			isSyncingStageData = true;
		}
		
		m_StageDataToLoad.Add(data);
		
		if(last)
		{
			LoadStage(m_StageDataToLoad);
			isSyncingStageData = false;
		}
	}

	private int m_SendingStageIndex = 0;
	public void SendStageData()
	{
		bool first = false;
		bool last = false;

		if(GameManager.Instance().isDebugMode)
			Debug.Log("Sending Stage Data");

		if(m_SendingStageIndex == 0)
		{
			first = true;
			isSyncingStageData = true;
		}
		if(m_SendingStageIndex == m_StageDataToLoad.Count - 1)
		{
			last = true;
			m_SendingStageData = false;
			isSyncingStageData = false;
		}

		NetworkManager.Instance().NMPhotonView.RPC("ReceiveStageData", PhotonTargets.Others, 
		                                           m_StageDataToLoad[m_SendingStageIndex], first, last);

		m_SendingStageIndex++;
		m_SendingStageDataTimer = Time.time + 1f;
	}

	private float m_SendingStageDataTimer = 0;
	private bool m_SendingStageData = false;
	public List<byte[]> m_StageDataToLoad;
	public void BeginSendStageOverNetwork(StageSaveData stageData)
	{
		m_SendingStageIndex = 0;
		m_SendingStageData = true;
		byte[] bytes = PhotonCustomSerialize.SerializeObject(stageData);
		List<byte[]> data = Utilities.SplitFileUp(bytes, 20000);
		m_StageDataToLoad = data;
		Debug.Log("DataLength: " + data.Count);
	}

	void Awake()
	{
		if(m_Instance)
			DestroyImmediate(gameObject);
		else
		{
			DontDestroyOnLoad(this);
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		HandleSelectionBox();
	}

	void FixedUpdate()
	{
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out m_MousePosition);

		if(m_SendingStageData && Time.time > m_SendingStageDataTimer)
			SendStageData();
	}
}


[Serializable]
public class StageSaveData
{
	public StagePiece[] stagePieces;
	public Dictionary<string, byte[]> m_Images = new Dictionary<string, byte[]>();

	public Dictionary<string, Character> m_Characters = new Dictionary<string, Character>();
	private Dictionary<string, int> m_CharactersLastID = new Dictionary<string, int>();
	
	public StageSaveData()
	{
		
	}

	public StagePiece[] GetStagePieces(){ return stagePieces; }
	
	public void SetVariables() //Set variables on save data
	{
		List<StagePiece> pieces = new List<StagePiece>();

		foreach(StagePiece sp in LevelManager.Instance().GetStage().Values)
		{
			if(sp.m_Type == StagePieceType.character)
			{
				bool contains;
				StageCharacter sc = (StageCharacter)sp;
				Character m_Character = GameManager.Instance().GetLoadedStageCharacter(sc.m_CharacterName + sc.m_CharacterID);
				m_Character.GetHashtableValue(Character.ConsistentVariables.Image.ToString(), out contains);
				
				if(contains)
				{
					byte[] image = (byte[])m_Character.GetHashtableValue(Character.ConsistentVariables.Image.ToString());

					if(!m_Images.ContainsKey(m_Character.GetName()))
					{
						m_Images.Add(m_Character.GetName(), image);
					}

					m_Character.ClearHashtableValue(Character.ConsistentVariables.Image.ToString());
					m_Character.ClearDirtyVariables();
				}
			}

			pieces.Add(sp);
		}

		stagePieces = pieces.ToArray();
		m_Characters = GameManager.Instance().GetLoadedStageCharacters();
		m_CharactersLastID = GameManager.Instance().GetLoadedStageCharactersLastIDs();
	}

	public void RetrieveVariables() //Retrieve from save data
	{
		RetrieveVariables(CharacterConnection.local);
	}

	public void RetrieveVariables(CharacterConnection conn) //Retrieve from save data
	{
		foreach(StagePiece sp in stagePieces)
		{
			if(sp.m_Type == StagePieceType.character)
			{
				StageCharacter sc = (StageCharacter)sp;
				sc.m_Connection = conn;
				Character m_Character = m_Characters[sc.m_CharacterName + sc.m_CharacterID];

				if(m_Images.ContainsKey(m_Character.GetName()))
				{
					m_Character.SetHashtableValue(Character.ConsistentVariables.Image.ToString(), m_Images[m_Character.GetName()]);
				}
			}

			LevelManager.Instance().AddStagePiece(sp.GetPosition(), sp);
		}

		GameManager.Instance().SetLoadedStageCharactersLastIDs(m_CharactersLastID);
		GameManager.Instance().SetLoadedStageCharacters(m_Characters);
	}
}
