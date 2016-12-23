//Bubble field
//Allows for disabling first bubble for attributes and handles selecting multiple bubbles at once

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BubbleFieldScript : MonoBehaviour {

	public StatObjectScript statObject;
	
	public int defaultValue = 0;
	
	public int numOfBubbles;
	public List<BubbleButtonScript> bubbles = new List<BubbleButtonScript>();
	public bool firstBubbleDisabled = false;
	
	public float bubblesXCoordConstant = 0;

	private int m_Value = 0;
	public int GetValue() { return m_Value; }


	public void SetValue(int val)
	{
		int index = val;

		for(int i = bubbles.Count -1; i >= 0; i--)
		{
			if(i < index)
			{
				bubbles[i].SetButtonTrue();
			}
			if(i >= index)
			{
				if(i != 0 || (i == 0 && !firstBubbleDisabled))
					bubbles[i].SetButtonFalse();
			}
		}
		
		m_Value = val;
	}

	public void SetValue(BubbleButtonScript button)
	{
		bool increasingValue = false;
		if(!button.buttonActive)
		{
			increasingValue = true;
		}

		if(increasingValue)
			SetValue(bubbles.IndexOf(button) + 1);
		else
			SetValue(bubbles.IndexOf(button));

		if(statObject)
			statObject.SaveToCharacterTable();
	}

	private void ResetBubbleButtons()
	{
		for(int i = 0; i < bubbles.Count; i++)
		{
			if(bubbles[i] != null && bubbles[i].gameObject)
				DestroyImmediate(bubbles[i].gameObject);
		}
		
		bubbles.Clear();
		
		for(int i = numOfBubbles; i > 0; i--)
		{
			GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/BubbleButton"),
			                                                   Vector3.zero, Quaternion.identity);
			
			go.transform.SetParent(gameObject.transform, false);

			go.GetComponent<RectTransform>().localPosition 
				= new Vector2(bubblesXCoordConstant + (i * -18), 0);

			BubbleButtonScript buttonScript = go.GetComponent<BubbleButtonScript>();
			buttonScript.bubbleField = this;
			
			bubbles.Add(buttonScript);
		}
	}
	
	private void CheckBubbleButtonsInEditor()
	{
		if(bubbles.Count != numOfBubbles)
		{
			ResetBubbleButtons();
		}
	}
	
	// Use this for initialization
	void Start () {
		if(Application.isPlaying)
		{
			if(firstBubbleDisabled)
			{
				if(!bubbles[0].buttonActive)
					bubbles[0].SetButtonTrue();
				m_Value = defaultValue;
			}
		}
	}
	
	void FixedUpdate()
	{
		if(Application.isEditor)
			CheckBubbleButtonsInEditor();
	}
}
