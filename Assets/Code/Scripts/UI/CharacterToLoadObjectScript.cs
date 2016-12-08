using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterToLoadObjectScript : MonoBehaviour {

	public Text fileName;
	public Image img;
	
	private Character m_Character;
	public Character GetCharacter() { return m_Character; }
	public void SetCharacterReference(Character character)
	{
		m_Character = character;

		fileName.text = m_Character.GetName();

		bool contains;
		m_Character.GetHashtableValue(Character.ConsistentVariables.Image.ToString(), out contains);

		if(contains)
		{
			byte[] imgData = (byte[])m_Character.GetHashtableValue(Character.ConsistentVariables.Image.ToString());

			GameManager.Instance().AddLoadedSprite(character.GetName(), imgData);
			img.sprite = GameManager.Instance().GetLoadedSprite(character.GetName());
		}
	}

	public void SetCharacterToLoad()
	{
		if(GameManager.Instance().isDebugMode)
			Debug.Log("Setting Character To Load: " + fileName.text);
		
		PiecePanelScript.GetPiecePanelScript().SetSelectedCharacterFile(gameObject);
		BuildingManager.Instance().m_PiecePlacingType = StagePieceType.character;
		BuildingManager.Instance().SetBuildingMode(BuildingManager.BuildingMode.placeMode);
	}
	
	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate () {
	}
}
