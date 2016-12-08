using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterInfoPanelBoardScript : MonoBehaviour {

	public Image portrait;
	private string m_CharacterReference = "";

	public void SetPortrait()
	{
		portrait.sprite = GameManager.Instance().GetLoadedSprite(GameManager.Instance().selectedCharacter.GetName());
	}

	// Use this for initialization
	void Start () {
		if(GameManager.Instance().selectedCharacter != null)
		{
			SetPortrait();
			m_CharacterReference = GameManager.Instance().selectedCharacter.GetKey();
		}
	}

	void FixedUpdate () {
		if(GameManager.Instance().selectedCharacter != null
		   && GameManager.Instance().selectedCharacter.GetKey() != m_CharacterReference)
			Start();
	}
}
