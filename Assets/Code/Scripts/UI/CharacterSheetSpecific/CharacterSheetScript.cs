/*
 * This script is for the entire character sheet UI object
 * it handles all character data input and portrait importing
 * */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class CharacterSheetScript : MonoBehaviour {

	public SkillsPanelScript skillsPanelScript;
	public FlawsnMeritsPanelScript flawsNMeritsPanelScript;

	public static CharacterSheetScript FindCharacterSheet()
	{ 
		if(GameObject.FindObjectOfType<HUD>())
			return GameObject.FindObjectOfType<CharacterSheetScript>().GetComponent<CharacterSheetScript>();
		else
			return null;
	}
	
	private PiecePanelScript GetPiecePanelScript(){ return GameObject.FindObjectOfType<PiecePanelScript>().GetComponent<PiecePanelScript>(); }

	public Image Portrait;
	public void SetPortrait(string imgPath)
	{
		if(File.Exists(imgPath) 
		   && (Path.GetExtension(imgPath).Equals(".png", System.StringComparison.InvariantCultureIgnoreCase)
		   || Path.GetExtension(imgPath).Equals(".jpg", System.StringComparison.InvariantCultureIgnoreCase)))
		{
			string type = "";
			if(Path.GetExtension(imgPath).Equals(".png", System.StringComparison.InvariantCultureIgnoreCase))
				type = "png";
			else
				type = "jpg";

			byte[] fileData;
			fileData = File.ReadAllBytes(imgPath);

			//Reduce image size
			Debug.Log("FileSize: " + fileData.Length);
			Texture2D tex = new Texture2D(500, 500);
			tex.LoadImage(fileData);
			tex.Apply();
			tex = Utilities.ScaleTexture(tex, 256, 256);
			if(type == "png")
				fileData = tex.EncodeToPNG();
			else
				fileData = tex.EncodeToJPG();
			Debug.Log("FileSize After: " + fileData.Length);

			GameManager.Instance().AddLoadedSprite(fileData);
			Portrait.sprite = GameManager.Instance().GetLoadedSprite(GameManager.Instance().selectedCharacter.GetName());
			
			GameManager.Instance().selectedCharacter.SetHashtableValue("Image", fileData);
		}
	}

	public void SetPortrait()
	{
		bool contains;
		GameManager.Instance().selectedCharacter.GetHashtableValue("Image", out contains);

		if(contains)
		{
			byte[] fileData;
			fileData = (byte[])GameManager.Instance().selectedCharacter.GetHashtableValue("Image");


			GameManager.Instance().AddLoadedSprite(fileData);
			Portrait.sprite = GameManager.Instance().GetLoadedSprite(GameManager.Instance().selectedCharacter.GetName());
		}
	}

	public void SpawnLoadImagePanel()
	{
		GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/CharacterSheetSpecific/ImgFilePanel"),
		                                                   Vector3.zero, Quaternion.identity);
		
		go.transform.SetParent(gameObject.transform, false);
		
		go.transform.localPosition = new Vector2(0, -210);
	}

	private Character m_TempCharacter = new Character(); //Temporary version of character before changes (Not used)
	public Character GetTemporaryCharacter(){ return m_TempCharacter; }

	public void SaveCharacter()
	{
		skillsPanelScript.SaveToCharacterObject(); // Save array of skills to temp character
		flawsNMeritsPanelScript.SaveToCharacterObject(); // Save array of flaws and array of merits to temp character

		GameManager.Instance().SaveCharacter();

		BuildingHUD.GetBuildingHUD().characterListNeedsPopulate = true;
	}

	public void CloseCharacterSheet()
	{
		Destroy(gameObject);

		BuildingHUD.GetBuildingHUD().EnableMainPanels();

		GetPiecePanelScript().PopulateCharactersList();
	}

	// Use this for initialization
	void Start () {
		m_TempCharacter = GameManager.Instance().selectedCharacter;
		SetPortrait();
	}
}
