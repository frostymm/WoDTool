//Simple loop script for loading screen when syncing two clients stage data (., .., ..., .....)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextChangeLoopScript : MonoBehaviour {

	public Text textToChange;
	public string[] textArray;
	public float interval = 1;

	private float m_Timer = 0;
	private int m_CurrentIndex = 0;
	private void NextTextFrame()
	{
		textToChange.text = textArray[m_CurrentIndex];

		m_CurrentIndex++;

		if(m_CurrentIndex == textArray.Length)
			m_CurrentIndex = 0;

		m_Timer = Time.time + interval;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Time.time > m_Timer)
			NextTextFrame();
	}
}
