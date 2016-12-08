using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BubbleButtonScript : MonoBehaviour {
	
	public Button bubbleButton;
	public BubbleFieldScript bubbleField;

	public bool buttonDisabled = false;
	public bool buttonActive = false;
	public void ButtonClick()
	{
		if(!buttonDisabled)
		{
			bubbleField.SetValue(this);
		}
	}

	public void SetButtonTrue()
	{
		ColorBlock cb = bubbleButton.colors;
		cb.normalColor = Color.black; 
		bubbleButton.colors = cb;
		
		buttonActive = true;
	}

	public void SetButtonFalse()
	{
		ColorBlock cb = bubbleButton.colors;
		cb.normalColor = Color.gray; 
		bubbleButton.colors = cb;
		
		buttonActive = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
