//Script to manage the roll panel on the board HUD
//Before data loss, this included a roll builder that allowed you to add up several stats and a modifier and combine them into a single roll

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class RollPanelBoardScript : MonoBehaviour {

	private Roll m_Roll = new Roll();
	public Roll GetRoll(){ return m_Roll; }

	public static RollPanelBoardScript GetRollPanelBoardScript(){ return GameObject.FindObjectOfType<RollPanelBoardScript>(); }

	public Text modifier;
	public void UpdateModifier(Text mod)
	{
		int i = 0;
		if(mod.text != "")
			i = Convert.ToInt32(mod.text);

		m_Roll.modifier = i;
	}

	public BubbleFieldScript m_BubbleScript;
	public void UpdateAgainModifier()
	{
		m_Roll.againModifier = m_BubbleScript.GetValue();
	}

	public void SavedRollsOnEnter()
	{
        Debug.Log("fak");
	}

	public void SaveRollOnClick()
	{
	}

	public void RemoveStatObject(string[] key)
	{
		m_Roll.RemoveStat(key);
	}

	public void AddStatObject(string[] key)
	{
		m_Roll.AddStat(key);
	}

	public Text successText;
	public void RollOnClick()
	{
		bool chance = false;
		successText.text = m_Roll.RollDice(out chance).ToString();

		if(chance)
			successText.text = "Chance! " + successText.text;
	}

	public void UpdateRoller()//Update roller on select roll
	{
		modifier.text = m_Roll.modifier.ToString(); // set modifier text
		m_BubbleScript.SetValue(m_Roll.againModifier); // set again bubbles
		// set list of stats
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		if(m_Roll.againModifier != m_BubbleScript.GetValue())
			UpdateAgainModifier();
	}
}
