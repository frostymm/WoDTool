//Script for the character list panel on the side of the screen during play.
//Before losing data from last time working on this, it also held a roll initiative button that rolled initiative for all characters present

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardCharacterListScript : MonoBehaviour {

	public GameObject contentBox;

	private List<BoardCharacterObjectScript> m_CharacterObjectList = new List<BoardCharacterObjectScript>();

	public void UpdateList()
	{
		List<StageCharacter> m_StageCharacters = LevelManager.Instance().GetStageCharacterList();

		if(m_StageCharacters.Count > m_CharacterObjectList.Count)// if too little, add more
		{
			int numToMake = m_StageCharacters.Count - m_CharacterObjectList.Count;
			for(int i = 0; i < numToMake; i++)
			{
				AddBoardCharacterObject();
			}
		}
		else if(m_StageCharacters.Count < m_CharacterObjectList.Count)// if too many, remove some
		{
			int numToKill = m_CharacterObjectList.Count - m_StageCharacters.Count;
			for(int i = 0; i < numToKill; i++)
			{
				RemoveBoardCharacterObject();
			}
		}

		for(int i = 0; i < m_CharacterObjectList.Count; i++)// set each object
		{
			if(m_StageCharacters.Count > i)
				m_CharacterObjectList[i].SetPieceReference(m_StageCharacters[i]);
		}

		Utilities.UI.UpdateContentBoxSize(contentBox, m_CharacterObjectList.Count, 110f);

		LevelManager.Instance().stageCharacterListChanged = false;
	}

	public void AddBoardCharacterObject()
	{
		GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/BoardCharacterObject"),
			                                                   Vector3.zero, Quaternion.identity);
			
		go.transform.SetParent(contentBox.transform, false);
		go.transform.localPosition = new Vector2(0, (m_CharacterObjectList.Count * -110) + 0);
		
		m_CharacterObjectList.Add(go.GetComponent<BoardCharacterObjectScript>());
	}

	public void RemoveBoardCharacterObject()
	{
		BoardCharacterObjectScript bcos = m_CharacterObjectList[m_CharacterObjectList.Count - 1];
		GameObject go = bcos.gameObject;

		m_CharacterObjectList.Remove(bcos);
		DestroyImmediate(go);
	}

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate () {
		if(LevelManager.Instance().stageCharacterListChanged)
			UpdateList();
	}
}
