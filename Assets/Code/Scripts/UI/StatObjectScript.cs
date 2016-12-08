using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StatObjectScript : MonoBehaviour {

	public string[] StatCategoryAndName;
	public InputFieldScript[] statFields;
	public BubbleFieldScript bubbleField;

	private Statistic m_Statistic;
	public Statistic statistic{ get{ return m_Statistic; } set{ m_Statistic = value; } }

	private Character m_CharacterReference;

	public void LoadFromStatistic()
	{
		//Debug.Log("Load from Statistic");

		if(m_Statistic != null)
		{
			bool contains = false;
			foreach(InputFieldScript statField in statFields)
			{
				string txt = (string)m_Statistic.GetHashtableValue(statField.key, out contains);

				if(contains)
					statField.SetText(txt);
				else
					statField.SetText("");
			}

			if(bubbleField)
			{
				contains = false;
				m_Statistic.GetHashtableValue("StatValue", out contains);

				if(contains)
					bubbleField.SetValue((int)m_Statistic.GetHashtableValue("StatValue"));
				else
					bubbleField.SetValue(0);
			}
		}
	}

	public void PopulateStatisticData()
	{
		foreach(InputFieldScript statField in statFields)
		{
			m_Statistic.SetHashtableValue(statField.key, statField.GetText());
		}

		if(bubbleField)
			m_Statistic.SetHashtableValue("StatValue", bubbleField.GetValue());
	}

	public void SaveToCharacterTable()
	{
		//Debug.Log("Saving Character Data");

		if(StatCategoryAndName.Length > 0)
		{
			PopulateStatisticData();

			if(GameManager.Instance().selectedCharacter != null)
				GameManager.Instance().selectedCharacter.SetHashtableValue(Utilities.GetKeyFromStringArray(StatCategoryAndName),
			                                                                                    statistic);
		}
	}

	public bool HasStatValueChanged()
	{
		if(StatCategoryAndName.Length > 0 && GameManager.Instance().selectedCharacter != null)
		{
			bool contains = false;
			string key = Utilities.GetKeyFromStringArray(StatCategoryAndName);
			
			GameManager.Instance().selectedCharacter.GetHashtableValue(key, out contains);
			
			if(contains)
			{
				Statistic stat = (Statistic)GameManager.Instance().selectedCharacter.GetHashtableValue(key);
				return stat.isDirty;
			}
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		if(Application.isPlaying && GameManager.Instance().selectedCharacter != null)
		{
			m_CharacterReference = GameManager.Instance().selectedCharacter;

			if(StatCategoryAndName.Length > 0)
			{
				bool contains = false;
				string key = Utilities.GetKeyFromStringArray(StatCategoryAndName);

				GameManager.Instance().selectedCharacter.GetHashtableValue(key, out contains);

				if(contains)
					statistic = (Statistic)GameManager.Instance().selectedCharacter.GetHashtableValue(key);
				else
				{
					if(bubbleField)
					{
						statistic = new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
							new KeyValuePair<System.Object, System.Object>("StatValue", bubbleField.defaultValue)});
					}
					else
						statistic = new Statistic();
				}

				LoadFromStatistic();
			}
		}
	}

	void FixedUpdate()
	{
		if(GameManager.Instance().selectedCharacter != m_CharacterReference || HasStatValueChanged())
			Start();
	}


	// Update is called once per frame
	void Update () {

	}
}
