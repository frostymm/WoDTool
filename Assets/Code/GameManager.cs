/*
 * Michael Shane McMurdy
 * Enigmatic Games 2015
 * 
 * 
 * This is the game manager singleton script that's attached to a donotdestroyonload object.
 * It includes all main game variables as well as how data is saved/loaded and handled
 * */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class GameManager : MonoBehaviour {

	private static GameManager m_Instance = null;
	public static GameManager Instance()
	{
		if (m_Instance == null) 
		{
			m_Instance = GameObject.FindObjectOfType<GameManager>();
		}
		
		return m_Instance;
	}

	private float m_VersionNumber = 0.2f;
	public float GetVersion(){ return m_VersionNumber; }
	
	bool m_DebugMode = true;
	public bool isDebugMode
	{
		get{ return m_DebugMode; }
		set{ m_DebugMode = value; }
	}

    //Is this client the dungeon master? Handles whether to display hidden blocks and various character information
	private bool m_isDM = false;
	public bool isDM
	{
		get{ return m_isDM; }
		set{ m_isDM = value; }
	}

	private bool m_inBuildMode = true;
	public bool inBuildMode
	{
		get{ return m_inBuildMode; }
		set
        {
            m_inBuildMode = value;
            if (m_inBuildMode)
                LevelManager.Instance().EnterBuildMode();
            else
                LevelManager.Instance().ExitBuildMode();
        }
	}

	public static Vector3 GetBlockScale(){ return new Vector3(GameManager.blockWidth, GameManager.blockHeight, GameManager.blockWidth); }
	
	private static float m_BlockWidth = 4;          //uniform width of all terrain block objects
	public static float blockWidth
	{
		get{ return m_BlockWidth; }
	}

	private static float m_BlockHeight = 1.0f;      // uniform height for all terrain block objects
	public static float blockHeight
	{
		get{ return m_BlockHeight; }
	}
	
	public EventSystem GetEventSystem()             //Event system associated with UI and BuildingHUD
	{
		return GameObject.FindObjectOfType<EventSystem>();
	}

	public void LoadOfflineMode()
	{
		PhotonNetwork.offlineMode = true;
		NetworkManager.Instance().CreateServer("");

        SceneManager.LoadScene("BuildingMode");
	}

    //List of sprites associated with their characters currently loaded into level
	private Dictionary<string, Sprite> m_LoadedSprites = new Dictionary<string, Sprite>();
	public void AddLoadedSprite(byte[] imgData)
	{
		if(selectedCharacter != null)
		{
			Texture2D newTexture = new Texture2D(500, 500);
			
			newTexture.LoadImage(imgData);

			Sprite sp = Sprite.Create(newTexture, 
			                           new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f,0.5f), 100);

			AddLoadedSprite(selectedCharacter.GetName(), sp);
		}
	}

	public void AddLoadedSprite(string name, byte[] imgData)
	{
		Texture2D newTexture = new Texture2D(500, 500);
		
		newTexture.LoadImage(imgData);
		
		Sprite sp = Sprite.Create(newTexture, 
		                          new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f,0.5f), 100);
		
		AddLoadedSprite(name, sp);
	}

	public void AddLoadedSprite(string name, Sprite sp)
	{
		if(m_LoadedSprites.ContainsKey(name))
		{
			m_LoadedSprites[name] = sp;
		}
		else
		{
			m_LoadedSprites.Add(name, sp);
		}
	}

	public Sprite GetLoadedSprite(string name)
	{
		bool contains = m_LoadedSprites.ContainsKey(name);
		if(contains)
		{
			return m_LoadedSprites[name];
		}
		else
			return Resources.Load<Sprite>("Textures/EnigmaticLogo");
	}

	private Character m_SelectedCharacter;      //Currently Selected character, this is who shows up on your board display at the bottom
	public Character selectedCharacter
	{ 
		get{ return m_SelectedCharacter; } 
		set{Debug.Log("Setting Selected Character: " + value.GetKey()); m_SelectedCharacter = value; }
	}

	public void NewSelectedCharacter()
	{
		m_SelectedCharacter = new Character();
	}

	private Dictionary<string, int> m_LoadedStageCharactersLastID = new Dictionary<string, int>();          //last instance id used with each character name, add +1 when making a new instance of a character
	public Dictionary<string, int> GetLoadedStageCharactersLastIDs(){ return m_LoadedStageCharactersLastID; }
	public void SetLoadedStageCharactersLastIDs(Dictionary<string, int> sIDs){ m_LoadedStageCharactersLastID = sIDs; }

	private Dictionary<string, Character> m_LoadedStageCharacters = new Dictionary<string, Character>();    //character instances on stage
	public Dictionary<string, Character> GetLoadedStageCharacters(){ return m_LoadedStageCharacters; }
	public void SetLoadedStageCharacters(Dictionary<string, Character> sChars)
	{ 
		m_LoadedStageCharacters = sChars;
		LevelManager.Instance().stageCharacterListChanged = true;
	}
	public Character GetLoadedStageCharacter(string key){ return m_LoadedStageCharacters[key]; }

	public void AddLoadedStageCharacter(Character c)
	{
		if(c.GetCharacterType() == CharacterTypes.UnimportantNPC)
		{
			if(m_LoadedStageCharacters.ContainsKey(c.GetName() + c.GetInstanceID()))
			{
                GetLoadedStageCharactersLastIDs()[c.GetName()]++;
				c.SetInstanceID(GetLoadedStageCharactersLastIDs()[c.GetName()]);
			}
			else
                GetLoadedStageCharactersLastIDs().Add(c.GetName(), 0);

			m_LoadedStageCharacters.Add(c.GetName() + c.GetInstanceID(), c);
		}
		else
		{
			if(!m_LoadedStageCharacters.ContainsKey(c.GetName() + c.GetInstanceID()))
				m_LoadedStageCharacters.Add(c.GetName() + c.GetInstanceID(), c);
		}
	}

	private List<Character> m_LoadedCharacterFiles = new List<Character>();                             //Currently Loaded characters associated with local files
	public List<Character> GetLoadedCharacterFiles() { return m_LoadedCharacterFiles; }

	public void AddLoadedCharacterFile(Character character)
	{
		if(!m_LoadedCharacterFiles.Contains(character))
			m_LoadedCharacterFiles.Add(character);
	}

	public void ClearLoadedStageCharacters()
	{
		m_LoadedStageCharacters.Clear();
		m_LoadedStageCharactersLastID.Clear();
	}

	public void ClearLoadedCharacterFiles()
	{
		m_LoadedCharacterFiles.Clear();
	}

	public void SaveCharacter() //Save character to persistent data path appdata/locallow/enigmaticgames
	{
		if(!Directory.Exists(Application.persistentDataPath + "/CharacterData/"))
			Directory.CreateDirectory(Application.persistentDataPath + "/CharacterData/");

		Statistic stat = (Statistic)selectedCharacter.GetHashtableValue(Character.ConsistentVariables.MainInfo.ToString());
		string fileName = (string)stat.GetHashtableValue("Name");
		
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/CharacterData/" + fileName + ".dat");
		
		Character data = selectedCharacter;
		
		bf.Serialize(file, data);
		file.Close();

		AddLoadedCharacterFile(data);                   // Add character file to list if new
		
		if(GameManager.Instance().isDebugMode)
			Debug.Log("Saving File to: " + Application.persistentDataPath + "/CharacterData/" + fileName + ".dat");
	}

	private Character m_CharacterToLoad;                // Chosen character from character placement selection
	public Character characterToLoad{ get{ return m_CharacterToLoad; } set{ selectedCharacter = m_CharacterToLoad = value; } }

	public Character LoadCharacter(string fileName)     //Load character from file and add to m_LoadedCharacterFiles list
	{
		if(File.Exists(Application.persistentDataPath + "/CharacterData/" + fileName + ".dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/CharacterData/" + fileName + ".dat", FileMode.Open);
			
			Character data = (Character)bf.Deserialize(file);
			file.Close();
			
			//Set SaveData
			AddLoadedCharacterFile(data);

			Debug.Log("Data Loaded");

			return data;
		}

		return null;
	}

	public void DeleteCharacter()
	{
		if(selectedCharacter != null)
			if(File.Exists(Application.persistentDataPath + "/CharacterData/" + selectedCharacter.GetName() + ".dat"))
			{
				File.Delete(Application.persistentDataPath + "/CharacterData/" + selectedCharacter.GetName() + ".dat");
			}
	}

	public void QuitGame()
	{
		#if UNITY_EDITOR
		if(Application.isEditor)
			UnityEditor.EditorApplication.isPlaying = false;
		#endif

		Application.Quit();
	}

	private float m_CharacterUpdateTimer = 0;
	private float m_CharacterUpdateInterval = 3.0f;
	public void SendCharacterUpdates()               //Send character data changes across network (I.E. damage status updates)
	{
		foreach(Character c in GetLoadedStageCharacters().Values)
		{
			for(int i = c.GetDirtyVariables().Count - 1; i >= 0; i--)
			{
				string s = c.GetDirtyVariables()[i];
				Debug.Log("Sending Dirty Variables: " + s);
				byte[] value = PhotonCustomSerialize.SerializeObject(c.GetHashtableValue(s));
				NetworkManager.Instance().NMPhotonView.RPC("SetCharacterValue", PhotonTargets.Others, 
				                                           c.GetKey(), s, value);

				c.GetDirtyVariables().Remove(s);
			}
		}
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

	// Use this for initialization
	void Start () {
		PhotonCustomSerialize.RegisterCustomTypes();
	}

	void FixedUpdate () {
		if(isDebugMode)
			Debug.Log("Photon: " + PhotonNetwork.connectionState);

        //Update changes across network
        if(!PhotonNetwork.offlineMode)
		    if (GetLoadedStageCharacters().Count > 0 && m_CharacterUpdateTimer < Time.time)
		    {
			    SendCharacterUpdates();
			    m_CharacterUpdateTimer = Time.time + m_CharacterUpdateInterval;
		    }
	}
}
