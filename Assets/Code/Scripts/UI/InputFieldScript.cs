//Simple script to manage data in an input field

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputFieldScript : MonoBehaviour {

	public InputField inputField;
	public string key;

	public string GetText()
	{
		return inputField.text;
	}

	public void SetText(string txt)
	{
		inputField.text = txt;
	}
}
