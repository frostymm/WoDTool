//Script for objects within the board character list
//Functions as a button to select said character

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardCharacterObjectScript : MonoBehaviour {

	public Image portrait;
	public Image[] bgImages;
	public Color[] colors;

	private StageCharacter m_StageCharacter;
	public void SetPieceReference(StageCharacter sc)
	{
		m_StageCharacter = sc;
		Character m_Character = GameManager.Instance().GetLoadedStageCharacter(sc.m_CharacterName + sc.m_CharacterID);

		if(m_Character.GetCharacterType() == CharacterTypes.ImportantNPC)
		{
			bgImages[0].color = colors[2];
			bgImages[1].color = colors[3];
		}
		else if(m_Character.GetCharacterType() == CharacterTypes.UnimportantNPC)
		{
			bgImages[0].color = colors[0];
			bgImages[1].color = colors[1];
		}

		portrait.sprite = GameManager.Instance().GetLoadedSprite(m_StageCharacter.m_CharacterName);
	}

	public void OnClick()
	{
		Character m_Character = GameManager.Instance().GetLoadedStageCharacter(m_StageCharacter.m_CharacterName + m_StageCharacter.m_CharacterID);

		GameManager.Instance().selectedCharacter = m_Character;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
