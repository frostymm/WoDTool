//Roll object
//Consists of all data needed to roll dice

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Roll {
	private List<string[]> m_Statistics = new List<string[]>();

	public void AddStat(string[] key)
	{
		if(!m_Statistics.Contains(key))
			m_Statistics.Add(key);
	}

	public void RemoveStat(string[] key)
	{
		if(m_Statistics.Contains(key))
			m_Statistics.Remove(key);
	}

	private int m_Modifier = 0;
	public int modifier
	{
		get{ return m_Modifier; }
		set{ m_Modifier = value; }
	}

	private int m_AgainModifier = 1; //10 minus this defines the number that the roll has to be higher than to activate *again* roll
	public int againModifier
	{
		get{ return m_AgainModifier; }
		set{ m_AgainModifier = value; }
	}

	public int RollDice()
	{
		bool chance = false;
		return RollDice(out chance);
	}

	public int RollDice(out bool chance) //Calculate number of successes based on parameters above
	{
		int diceToRoll = 0;
		chance = false;

		//add up statistics
		foreach(string[] stat in m_Statistics)
		{
			if(stat[0] == Character.StatCategories.Attributes.ToString())
			{
				diceToRoll += GameManager.Instance().selectedCharacter.GetAttributeValue(stat[2]);
			}
			else if(stat[0] == Character.StatCategories.BasicInfo.ToString())
			{
				diceToRoll += Convert.ToInt32(GameManager.Instance().selectedCharacter.GetBasicInfo(stat[2]));
			}
			else if(stat[0] == Character.StatCategories.Skills.ToString())
			{
				diceToRoll += GameManager.Instance().selectedCharacter.GetSkillValue(stat[1], stat[2]);
			}
		}

		//add modifier
		diceToRoll += m_Modifier;

		//initiate number of rolls
		if(diceToRoll < 1)
		{
			chance = true;
			return UnityEngine.Random.Range(1, 11);
		}

		int numOfSuccess = 0;
		string rolls = "";
		//While #ofRolls > 0
		while(diceToRoll > 0)
		{
			int number = UnityEngine.Random.Range(1, 11);
			rolls += ", " + number;

			//Add agains back into number of rolls
			if(number >= 8)
			{
				numOfSuccess++;
				if(number > 10 - m_AgainModifier)
				{
					diceToRoll++;
				}
			}

			diceToRoll--;
		}

		if(GameManager.Instance().isDebugMode)
			Debug.Log("Rolls: " + rolls);

		return numOfSuccess; //return success'
	}

	public static Roll GetCurrentlySelectedRoll(){ return GameObject.FindObjectOfType<RollPanelBoardScript>().GetRoll(); }
}
