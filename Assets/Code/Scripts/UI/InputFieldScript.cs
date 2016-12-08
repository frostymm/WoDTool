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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
