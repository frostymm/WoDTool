using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterTypeDropDownScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Text characterTypeText;
	public GameObject characterDropDownPanel;

	public void OnPointerEnter(PointerEventData pData)
	{
		CreateButtonList();
	}

	public void OnPointerExit(PointerEventData pData)
	{
		DestroyButtonList();
	}

	private bool m_ListCreated = false;
	private void CreateButtonList()
	{
		if(!m_ListCreated)
		{
			int[] cTypes =  (int[])Enum.GetValues(typeof(CharacterTypes));
			
			foreach(CharacterTypes c in cTypes)
			{
				GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/CharacterSheetSpecific/CharacterTypeButton"),
				                                                   Vector3.zero, Quaternion.identity);
				
				go.transform.SetParent(characterDropDownPanel.transform, false);
				
				go.transform.localPosition = new Vector2(0, (((int)c) * -60) + 30);
				
				go.GetComponent<CharacterTypeButtonScript>().SetCharacterType(c);
				go.GetComponent<CharacterTypeButtonScript>().SetDropDownScript(this);
			}
			Utilities.UI.UpdateContentBoxSize(characterDropDownPanel, cTypes.Length, 60);

			m_ListCreated = true;
		}

		if(!characterDropDownPanel.activeSelf)
			characterDropDownPanel.SetActive(true);
	}

	public void DestroyButtonList()
	{
		if(characterDropDownPanel.activeSelf)
			characterDropDownPanel.SetActive(false);

		characterTypeText.text = GameManager.Instance().selectedCharacter.GetCharacterType().ToString();
	}

	// Use this for initialization
	void Start () {
		characterTypeText.text = GameManager.Instance().selectedCharacter.GetCharacterType().ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
