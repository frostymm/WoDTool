/*
 * The script for the object that fills the stat list
 * in the roll panel on the board HUD
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatListRollObjectScript : MonoBehaviour {

	public Text nameText;

	private string[] m_Key = new string[3];
	private string m_StatName;
	public void SetStat(string[] key)
	{
		m_Key = key;
		m_StatName = m_Key[2];
		nameText.text = m_StatName;
	}

	public void RemoveStat() //Remove stat from list of roll stats
	{
		RollPanelBoardScript.GetRollPanelBoardScript().RemoveStatObject(m_Key);
	}
}
