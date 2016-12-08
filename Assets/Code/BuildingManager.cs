using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class BuildingManager : MonoBehaviour {

	private static BuildingManager m_Instance = null;
	public static BuildingManager Instance()
	{
		if (m_Instance == null) 
		{
			m_Instance = GameObject.FindObjectOfType<BuildingManager>();
		}
		
		return m_Instance;
	}

	private StageBlockType m_CurrentlySelectedType = 0;
	public StageBlockType currentSelectedType
	{
		get{ return m_CurrentlySelectedType; }
		set{ m_CurrentlySelectedType = value; }
	}

	public static Material GetStageBlockMaterial(StageBlockType stagePieceType)
	{
		return Resources.Load<Material>("Materials/Terrain/" + stagePieceType.ToString ());
	}

	public void SetBuildingMode(BuildingMode bMode)
	{
		if(bMode == BuildingMode.placeMode)
		{
			if(m_PiecePlacingType == StagePieceType.block)
			{
				// set temporary selector to have appropriate texture
				if(LevelManager.Instance().GetHighlighterBox().GetComponent<Renderer>().material 
				   != BuildingManager.GetStageBlockMaterial(currentSelectedType))
				{
					LevelManager.Instance().GetHighlighterBox().GetComponent<Renderer>().material = GetStageBlockMaterial(currentSelectedType);
				}

				DisableTransparentCoin();
			}
			else if(m_PiecePlacingType == StagePieceType.character)
			{
				if(LevelManager.Instance().GetHighlighterBox().GetComponent<Renderer>().material 
				   != BuildingManager.GetStageBlockMaterial(StageBlockType.empty))
				{
					LevelManager.Instance().GetHighlighterBox().GetComponent<Renderer>().material = GetStageBlockMaterial(StageBlockType.empty);
				}

				EnableTransparentCoin();
			}
		}
		else if(bMode == BuildingMode.select)
		{
			if(LevelManager.Instance().GetHighlighterBox().GetComponent<Renderer>().material 
			   != BuildingManager.GetStageBlockMaterial(StageBlockType.empty))
			{
				LevelManager.Instance().GetHighlighterBox().GetComponent<Renderer>().material = GetStageBlockMaterial(StageBlockType.empty);
			}

			DisableTransparentCoin();
		}
		else if(bMode == BuildingMode.deleteMode)
		{
			if(LevelManager.Instance().GetHighlighterBox().GetComponent<Renderer>().material 
			   != BuildingManager.GetStageBlockMaterial(StageBlockType.delete))
			{
				LevelManager.Instance().GetHighlighterBox().GetComponent<Renderer>().material = GetStageBlockMaterial(StageBlockType.delete);
			}

			DisableTransparentCoin();
		}

		buildingMode = bMode;
	}

	public BuildingMode buildingMode = 0;
	public enum BuildingMode
	{
		select,
		placeMode,
		deleteMode
	}

	public void ChangeElevation(int i)
	{
		Vector3 pos = m_GridCollider.transform.position;
		pos = new Vector3(pos.x, pos.y + (i * GameManager.blockHeight), pos.z);

		m_GridCollider.transform.position = pos;
	}

	public void SetElevation(int i)
	{ 
		m_GridCollider.transform.position =
			new Vector3(m_GridCollider.transform.position.x, i * GameManager.blockHeight, m_GridCollider.transform.position.z);
	}

	public void DeletePiece(Vector3 pos)
	{
		if(LevelManager.Instance().GetStage().ContainsKey(pos))
		{
			LevelManager.Instance().GetStage()[pos].DestroyGameObject();

			LevelManager.Instance().RemoveStagePiece(pos);
		}
	}

	public void CreateBlock(Vector3 pos, StageBlockType selectedType)
	{
		StageBlock sp = new StageBlock(selectedType, pos);
		LevelManager.Instance().AddStagePiece(pos, sp);
		sp.CreateGameObject();
	}

	public void CreateCharacterPiece(Vector3 pos, CharacterConnection conn)
	{
		Vector3 lowerLevel = pos - new Vector3(0, GameManager.blockHeight, 0);
		if(!LevelManager.Instance().GetStage().ContainsKey(lowerLevel) 
		   || LevelManager.Instance().GetStage()[lowerLevel].m_Type != StagePieceType.character)
		{
			Character character = GameManager.Instance().selectedCharacter;

			StageCharacter sp = new StageCharacter(character,
		                        	pos, conn);

			LevelManager.Instance().AddStagePiece(pos, sp);
			sp.CreateGameObject();

			NetworkManager.Instance().NMPhotonView.RPC("CreateCharacterPiece", PhotonTargets.Others, pos, GameManager.Instance().selectedCharacter);
		}
	}

	public void CreateCharacterPiece(Vector3 pos, Character characterRef, CharacterConnection conn)
	{
		Character character = characterRef;

		StageCharacter sp = new StageCharacter(character,
		                        pos, conn);
		LevelManager.Instance().AddStagePiece(pos, sp);
		sp.CreateGameObject();
	}

	public void CreatePiece(Vector3 pos)
	{
		if(m_PiecePlacingType == StagePieceType.block)// If placing a block
		{
			if(LevelManager.Instance().GetStage().ContainsKey(pos))
			{
				StagePiece sp = LevelManager.Instance().GetStage()[pos];
				if(sp.m_Type == StagePieceType.block)
				{
					StageBlock sb = sp as StageBlock;
					sb.SetBlockType(currentSelectedType); // not supported in multiplayer yet
				}
				else
				{
					sp.DestroyGameObject();

					NetworkManager.Instance().NMPhotonView.RPC("CreateBlock", PhotonTargets.All, pos, (int)currentSelectedType);
				}
			}
			else
			{
				NetworkManager.Instance().NMPhotonView.RPC("CreateBlock", PhotonTargets.All, pos, (int)currentSelectedType);
			}
		}
		else if(m_PiecePlacingType == StagePieceType.character)// If placing a character
		{
			if(LevelManager.Instance().GetStage().ContainsKey(pos))
			{
				StagePiece sp = LevelManager.Instance().GetStage()[pos];
				if(sp.m_Type == StagePieceType.character)
				{
					//StageCharacter sb = sp as StageCharacter;

				}
				else
				{
					sp.DestroyGameObject();

					CreateCharacterPiece(pos, CharacterConnection.local);
				}
			}
			else
			{
				CreateCharacterPiece(pos, CharacterConnection.local);
			}
		}
	}

	public void MovePiece(Vector3 oldPos, Vector3 newPos)
	{
		StagePiece sp = LevelManager.Instance().GetStage()[oldPos];
		GameObject go = LevelManager.Instance().GetStageGameObjects()[oldPos];

		sp.SetPosition(newPos);
		go.transform.position = newPos;

		LevelManager.Instance().GetStage().Remove(oldPos);
		LevelManager.Instance().GetStageGameObjects().Remove(oldPos);

		LevelManager.Instance().GetStage().Add(newPos, sp);
		LevelManager.Instance().GetStageGameObjects().Add(newPos, go);
	}

	private GameObject m_TransparentCoin;
	public void EnableTransparentCoin()
	{
		if(m_TransparentCoin == null)
		{
			m_TransparentCoin = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/SelectionCoin"));
			m_TransparentCoin.transform.parent = LevelManager.Instance().GetHighlighterBox().transform;
			m_TransparentCoin.transform.localPosition = Vector3.zero;

			MeshRenderer[] meshs = m_TransparentCoin.GetComponentsInChildren<MeshRenderer>();
			foreach(MeshRenderer mesh in meshs)
			{
				mesh.material = (Material)Resources.Load("Materials/TransParentCoin");
			}
		}
		else
		{
			if(!m_TransparentCoin.activeSelf)
				m_TransparentCoin.SetActive(true);
		}
	}

	public void DisableTransparentCoin()
	{
		if(m_TransparentCoin && m_TransparentCoin.activeSelf)
			m_TransparentCoin.SetActive(false);
	}

	private void OnSelectButtonPress(GameObject selectionBox)
	{
		Vector3 selectedPosition = LevelManager.Instance().GetSelectedPosition();

		if(buildingMode == BuildingMode.placeMode)
		{
			CreatePiece(selectedPosition);
		}
		else if(buildingMode == BuildingMode.deleteMode)
		{
			NetworkManager.Instance().NMPhotonView.RPC("DeletePiece", PhotonTargets.All, selectedPosition);
		}
	}

	private void OnDeselectButtonPress(GameObject selectionBox)
	{
		if(buildingMode == BuildingMode.placeMode || buildingMode == BuildingMode.deleteMode)
			BuildingManager.Instance().SetBuildingMode(BuildingMode.select);
		if(buildingMode == BuildingMode.select)
		{
			if(selectionBox.activeSelf)
				selectionBox.SetActive(false);
		}
	}
	
	public StagePieceType m_PiecePlacingType = StagePieceType.block;
	public Collider m_GridCollider; //plane in which we can view and click on currently
	public void HandleSelectionBox(GameObject selectionBox)
	{
		if(LevelManager.Instance().isPositionSelected)
		{
			if(Input.GetButtonDown("Select"))
			{
				OnSelectButtonPress(selectionBox);
			}
			if(Input.GetButtonDown("Deselect"))
			{
				OnDeselectButtonPress(selectionBox);
			}
		}
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
}

[Serializable]
public class StageCharacter : StagePiece
{
	public string m_CharacterName;
	public int m_CharacterID;
	public CharacterConnection m_Connection;

	public StageCharacter(Character character, Vector3 pos, CharacterConnection connection) : base(pos, StagePieceType.character)
	{
		Character clone;
		GameManager.Instance().AddLoadedStageCharacter(clone = (Character)character.Copy());

		m_CharacterID = clone.GetInstanceID();
		m_CharacterName = clone.GetName();
		m_Connection = connection;
	}

	public void SetImage()
	{
		Character m_Character = GameManager.Instance().GetLoadedStageCharacter(m_CharacterName + m_CharacterID);

		bool contains;
		m_Character.GetHashtableValue(
			Character.ConsistentVariables.Image.ToString(), out contains);

		if(contains)
		{
			byte[] imageData = (byte[])m_Character.GetHashtableValue(Character.ConsistentVariables.Image.ToString());

			GameObject go = LevelManager.Instance().GetStageGameObjects()[GetPosition()];
			
			GameManager.Instance().AddLoadedSprite(m_CharacterName, imageData);
			go.GetComponent<CoinObjectScript>().coinFace.sprite = GameManager.Instance().GetLoadedSprite(m_CharacterName);
		}
	}

	public void SetImage(byte[] imageData)
	{
		GameObject go = LevelManager.Instance().GetStageGameObjects()[GetPosition()];

		GameManager.Instance().AddLoadedSprite(m_CharacterName, imageData);
		go.GetComponent<CoinObjectScript>().coinFace.sprite = GameManager.Instance().GetLoadedSprite(m_CharacterName);
	}

	public override void CreateGameObject()
	{
		Character m_Character = GameManager.Instance().GetLoadedStageCharacter(m_CharacterName + m_CharacterID);

		GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Pieces/CharacterCoin"),
		                                                   GetPosition(), Quaternion.identity);
		
		LevelManager.Instance().GetStageGameObjects().Add(GetPosition(), go);

		SetImage();

		if(m_Character.GetCharacterType() == CharacterTypes.ImportantNPC) //if friendly npc, set coin to silver
			go.GetComponent<CoinObjectScript>().SetMats(Resources.Load<Material>("Materials/SilverCoin"));

		if(m_Character.GetCharacterType() == CharacterTypes.UnimportantNPC) //if enemy npc, set coin to bronze
			go.GetComponent<CoinObjectScript>().SetMats(Resources.Load<Material>("Materials/BronzeCoin"));

		base.CreateGameObject();
	}
	
	public override void DestroyGameObject()
	{
		GameObject go = LevelManager.Instance().GetStageGameObjects()[GetPosition()];
		LevelManager.Instance().GetStageGameObjects().Remove(GetPosition());
		
		MonoBehaviour.Destroy(go);

		base.DestroyGameObject();
	}
}

[Serializable]
public class StageBlock : StagePiece
{
	public StageBlockType m_BlockType;

	public StageBlock(StageBlockType type, Vector3 pos) : base(pos, StagePieceType.block)
	{
		m_BlockType = type;
	}

	public void SetBlockType(StageBlockType type)
	{
		GameObject go = LevelManager.Instance().GetStageGameObjects()[GetPosition()];
		
		if(m_BlockType != type)
		{
			m_BlockType = type;
			
			if(go)
			{
				go.GetComponent<Renderer>().material = BuildingManager.GetStageBlockMaterial(m_BlockType);
			}
		}
	}

	public override void CreateGameObject()
	{
		GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/TerrainBlocks/TerrainBlock"),
		                                                   GetPosition(), Quaternion.identity);
		
		LevelManager.Instance().GetStageGameObjects().Add(GetPosition(), go);
		
		go.GetComponent<Renderer>().material = BuildingManager.GetStageBlockMaterial(m_BlockType);

		base.CreateGameObject();
	}
	
	public override void DestroyGameObject()
	{
		GameObject go = LevelManager.Instance().GetStageGameObjects()[GetPosition()];
		LevelManager.Instance().GetStageGameObjects().Remove(GetPosition());
		
		MonoBehaviour.Destroy(go);

		base.DestroyGameObject();
	}
}

[Serializable]
public class StagePiece
{
	public StagePieceType m_Type;

	public float m_PosX;
	public float m_PosY;
	public float m_PosZ;

	private bool m_Hidden = false;
	public bool isHidden
	{
		get{ return m_Hidden; }
		set
		{ 
			m_Hidden = value;

			if(LevelManager.Instance().GetStageGameObjects().ContainsKey(GetPosition()))
			{
				Utilities.SetRenderersForGameObject(LevelManager.Instance().GetStageGameObjects()[GetPosition()], isHidden);
			}
		} 
	}

	public StagePiece(Vector3 pos, StagePieceType type)
	{
		m_PosX = pos.x;
		m_PosY = pos.y;
		m_PosZ = pos.z;
		m_Type = type;
	}

	public void SetPosition(Vector3 pos)
	{
		m_PosX = pos.x;
		m_PosY = pos.y;
		m_PosZ = pos.z;
	}

	public Vector3 GetPosition() { return new Vector3(m_PosX, m_PosY, m_PosZ); }

	public virtual void CreateGameObject()
	{
		if(isHidden)
			Utilities.SetRenderersForGameObject(LevelManager.Instance().GetStageGameObjects()[GetPosition()], isHidden);
	}

	public virtual void DestroyGameObject()
	{

	}
}

public enum StagePieceType
{
	block,
	character,
	prop
}

public enum StageBlockType
{
	empty,
	delete,
	dirt,
	water,
	lava
}