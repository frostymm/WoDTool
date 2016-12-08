using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CyclingBoxPanelScript : MonoBehaviour {

	public string[] StatCategoryAndName;
	public int[] Values;

	public CyclingButtonScript[] buttons;

	private string m_CharacterReference;

	public void PopulateCyclingButtons()
	{
		bool contains = false;
		
		GameManager.Instance().selectedCharacter.GetHashtableValue(Utilities.GetKeyFromStringArray(StatCategoryAndName), out contains);
		if(contains)
		{
			Values = (int[])GameManager.Instance().selectedCharacter.GetHashtableValue(Utilities.GetKeyFromStringArray(StatCategoryAndName));
			SetButtonValues();
		}

		m_CharacterReference = GameManager.Instance().selectedCharacter.GetKey();
	}

	public void SetButtonValues()
	{
		for(int i = 0; i < buttons.Length; i++)
		{
			buttons[i].SetValue(Values[i]);
		}
	}

	public void SaveToCharacterObject()
	{
		List<int> vals = new List<int>();

		foreach(CyclingButtonScript button in buttons)
		{
			vals.Add(button.GetValue());
		}

		if(GameManager.Instance().selectedCharacter != null)
			GameManager.Instance().selectedCharacter.SetHashtableValue(Utilities.GetKeyFromStringArray(StatCategoryAndName), vals.ToArray());
	}

	public bool HasValueChanged()
	{
		if(StatCategoryAndName.Length > 0 && GameManager.Instance().selectedCharacter != null)
		{
			bool contains = false;
			string key = Utilities.GetKeyFromStringArray(StatCategoryAndName);
			
			GameManager.Instance().selectedCharacter.GetHashtableValue(key, out contains);
			
			if(contains)
			{
				int[] val = (int[])GameManager.Instance().selectedCharacter.GetHashtableValue(key);

				if(val != Values)
					return true;
			}
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		if(GameManager.Instance().selectedCharacter != null)
			PopulateCyclingButtons();
	}

	void FixedUpdate () {
		if(GameManager.Instance().selectedCharacter != null
		   && (GameManager.Instance().selectedCharacter.GetKey() != m_CharacterReference
		   || HasValueChanged()))
			Start();
	}
}
