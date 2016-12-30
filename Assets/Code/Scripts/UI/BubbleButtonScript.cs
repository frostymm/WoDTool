//Bubbles within a bubblefield

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BubbleButtonScript : MonoBehaviour {
	
	public Button bubbleButton;
	public BubbleFieldScript bubbleField;

	public bool buttonDisabled = false;             //interactable
	public bool buttonActive = false;               //Colored black/Turned on
    public bool buttonFrozenAppearance = false;     //This was added to support the merit dropdown displays which don't actually change appearance once populated
	public void ButtonClick()
	{
		if(!buttonDisabled)
		{
			bubbleField.SetValue(this);
		}
	}

    public void TurnButtonBlack()
    {
        if (buttonFrozenAppearance)
            return;

        ColorBlock cb = bubbleButton.colors;
        cb.normalColor = Color.black;
        bubbleButton.colors = cb;
    }

	public void SetButtonTrue()
	{
        TurnButtonBlack();
		
		buttonActive = true;
	}

    public void TurnButtonGray()
    {
        if (buttonFrozenAppearance)
            return;

        ColorBlock cb = bubbleButton.colors;
        cb.normalColor = Color.gray;
        bubbleButton.colors = cb;
    }

	public void SetButtonFalse()
	{
        TurnButtonGray();
		
		buttonActive = false;
	}
}
