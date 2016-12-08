using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CyclingButtonScript : MonoBehaviour {
	public Text buttonText;

	public string[] buttonTypes;

	public void OnClick()
	{
		SetValue(++currValue);
		gameObject.GetComponentInParent<CyclingBoxPanelScript>().SaveToCharacterObject();
	}

	private int currValue = 0;
	public void SetValue(int i)
	{
		if(i > buttonTypes.Length - 1)
			i = 0;

		currValue = i;
		buttonText.text = buttonTypes[currValue];
	}
	public int GetValue(){ return currValue; }

	void Start()
	{

	}
}
