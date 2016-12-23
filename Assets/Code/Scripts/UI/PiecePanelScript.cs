//This is the script that runs the entire panel with the list of pieces, characters and props you can place.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class PiecePanelScript : MonoBehaviour {

	public GameObject piecePanel;
	public GameObject buildingBlockList;
	public GameObject characterPieceList;

	public GameObject buildingBlockContent;
	public GameObject CharacterPieceContent;

	public static PiecePanelScript GetPiecePanelScript(){ return GameObject.FindObjectOfType<PiecePanelScript>().GetComponent<PiecePanelScript>(); }
	
	public void DisableAllPiecePanels()
	{
		if(buildingBlockList.activeSelf)
			buildingBlockList.SetActive(false);
		if(characterPieceList.activeSelf)
			characterPieceList.SetActive(false);
	}

	public void EnableBlockList()
	{
		if(!buildingBlockList.activeSelf)
			buildingBlockList.SetActive(true);
	}

	public void EnableCharacterList()
	{
		if(!characterPieceList.activeSelf)
			characterPieceList.SetActive(true);
	}

	public void PopulateBuildingBlockList()
	{
		int[] pieceTypes =  (int[])Enum.GetValues(typeof(StageBlockType));

		foreach(StageBlockType piece in pieceTypes)
		{
			if(piece != StageBlockType.empty && piece != StageBlockType.delete)
			{
				GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/PieceButton"),
				                                                   Vector3.zero, Quaternion.identity);
				
				go.transform.SetParent(buildingBlockContent.transform, false);
				
				go.transform.localPosition = new Vector2(0, (((int)piece -2) * -100) - 20);
				
				go.GetComponent<PieceButtonScript>().SetParameters(piece.ToString(), (int)StagePieceType.block, (int)piece);
			}
		}
	}

	public void DeleteCharacter()
	{
		GameManager.Instance().DeleteCharacter();
		BuildingHUD.GetBuildingHUD().characterListNeedsPopulate = true;
	}
	
	List<GameObject> characterFiles = new List<GameObject>();
	public void DeleteOldFileList()
	{
		foreach(GameObject go in characterFiles)
		{
			Destroy(go);
		}
		
		characterFiles.Clear();
		SelectedCharacterFileIndex = -1;

		GameManager.Instance().ClearLoadedCharacterFiles();
	}

	private int SelectedCharacterFileIndex = -1;
	public int GetSelectedCharacterFileIndex(){ return SelectedCharacterFileIndex; }

	public void SetSelectedCharacterFile(GameObject go)
	{
		if(characterFiles.Contains(go))
		{
			if(SelectedCharacterFileIndex == characterFiles.IndexOf(go)) //click on currently selected character
			{
				GameManager.Instance().characterToLoad = go.GetComponent<CharacterToLoadObjectScript>().GetCharacter();
				OpenCharacterCreateScreen();
			}
			else
			{
				Button b;
				ColorBlock cb;

				if(SelectedCharacterFileIndex != -1)
				{
					//Set old selected file to default
					b = characterFiles[SelectedCharacterFileIndex].GetComponent<Button>();
					cb = b.colors;
					cb.normalColor = Color.black; 
					b.colors = cb;
				}
				
				SelectedCharacterFileIndex = characterFiles.IndexOf(go);
				
				//Set new selected file to highlighted
				b = characterFiles[SelectedCharacterFileIndex].GetComponent<Button>();
				cb = b.colors;
				cb.normalColor = Color.white;
				b.colors = cb;

				GameManager.Instance().characterToLoad = characterFiles[SelectedCharacterFileIndex].GetComponent<CharacterToLoadObjectScript>().GetCharacter();
			}
		}
	}



	public void PopulateCharactersList()
	{
		if(BuildingHUD.GetBuildingHUD().characterListNeedsPopulate)
		{
			DeleteOldFileList();
			
			string[] files =  Directory.GetFiles(Application.persistentDataPath + "/CharacterData/");
			
			foreach(string fileName in files)
			{
				Character character = GameManager.Instance().LoadCharacter(Path.GetFileNameWithoutExtension(fileName));

				GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/CharacterToLoadObject"),
				                                                   Vector3.zero, Quaternion.identity);
				
				go.transform.SetParent(CharacterPieceContent.transform, false);
				
				go.transform.localPosition = new Vector2(0, (characterFiles.Count * -150) - 100);
				
				go.GetComponent<CharacterToLoadObjectScript>().SetCharacterReference(character);
				
				characterFiles.Add(go);
			}
			
			CharacterPieceContent.GetComponent<RectTransform>().sizeDelta 
				= new Vector2(CharacterPieceContent.GetComponent<RectTransform>().sizeDelta.x, (characterFiles.Count * 150) + 100);

			BuildingHUD.GetBuildingHUD().characterListNeedsPopulate = false;
		}
	}

	public void ClearCharacterData()
	{
		GameManager.Instance().NewSelectedCharacter();
	}
	
	public void OpenCharacterCreateScreen()
	{
		BuildingHUD.GetBuildingHUD().DisableMainPanels();

		GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/CharacterSheetSpecific/CharacterSheetPanel"),
		                                                   Vector3.zero, Quaternion.identity);
		
		go.transform.SetParent(BuildingHUD.GetBuildingHUD().gameObject.transform, false);
		
		go.transform.localPosition = new Vector2(0, 0);
	}

	// Use this for initialization
	void Start () 
	{
		EnableBlockList();

		PopulateBuildingBlockList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate()
	{
		if(characterFiles.Count < GameManager.Instance().GetLoadedCharacterFiles().Count)
		{
			PopulateCharactersList();
		}
	}
}
