//Input field populated with character information

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputFieldToCharacterScript : MonoBehaviour {

	public string[] StatCategoryAndName;
	public Text TextField;

	public void SetHashtableValue()
	{
		string key = Utilities.GetKeyFromStringArray(StatCategoryAndName);

		GameManager.Instance().selectedCharacter.SetHashtableValue(key, TextField.text);
	}

	private void LoadData()
	{
		bool populated = false;
		
		GameManager.Instance().selectedCharacter.GetHashtableValue(Utilities.GetKeyFromStringArray(StatCategoryAndName), out populated);
		if(populated)
		{
			TextField.text = (string)GameManager.Instance().selectedCharacter.GetHashtableValue(Utilities.GetKeyFromStringArray(StatCategoryAndName));
		}
	}

	// Use this for initialization
	void Start () {
		LoadData();
	}
}
