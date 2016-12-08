using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterTypeButtonScript : MonoBehaviour {

	public Text buttonText;
	private CharacterTypes m_Type;
	private CharacterTypeDropDownScript m_DropDownScript;

	public void OnClick()
	{
		GameManager.Instance().selectedCharacter.SetHashtableValue(Character.ConsistentVariables.CharacterType.ToString(), m_Type);
		m_DropDownScript.DestroyButtonList();
	}

	public void SetCharacterType(CharacterTypes cType)
	{
		buttonText.text = cType.ToString();
		m_Type = cType;
	}

	public void SetDropDownScript(CharacterTypeDropDownScript dropDown){ m_DropDownScript = dropDown; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
