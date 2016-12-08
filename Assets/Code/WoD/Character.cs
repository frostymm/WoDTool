using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Character : ICloneable{

	private Hashtable m_Table = new Hashtable(); // table for miscellanious stats

	public object Clone(){ return this.MemberwiseClone(); }//Doesn't work for poop (clones character but uses same hashTable)
	public Character Copy()
	{
		Character c = new Character();

		object[] keys = new object[m_Table.Count];
		object[] values = new object[m_Table.Count];

		m_Table.Keys.CopyTo(keys, 0);
		m_Table.Values.CopyTo(values, 0);

		for(int i = 0; i < m_Table.Count; i++)
			c.SetHashtableValue(keys[i], values[i]);

		c.ClearDirtyVariables();

		return c;
	}

	private List<string> m_DirtyVariables = new List<string>();
	public List<string> GetDirtyVariables(){ return m_DirtyVariables; }
	public void ClearDirtyVariables(){ m_DirtyVariables.Clear(); }

	private void SetDirtyVariable(string variable)
	{
		if(!m_DirtyVariables.Contains(variable))
			m_DirtyVariables.Add(variable);
	}

	public void SetHashtableValue(System.Object key, System.Object val)
	{
		SetHashtableValue(key, val, false);
	}

	public void SetHashtableValue(System.Object key, System.Object val, bool isCalledFromNetwork)
	{
		//Debug.Log("SettingHashTableValue: " + (string)key);

		if(val.GetType() == typeof(Statistic) && isCalledFromNetwork)
		{
			Statistic stat = (Statistic)val;
			stat.isDirty = true;
			val = stat;
		}
		
		if(!m_Table.Contains(key))
			m_Table.Add(key, val);
		else
			m_Table[key] = val;

		if(!isCalledFromNetwork)
			SetDirtyVariable((string)key);
	}

	public System.Object GetHashtableValue(System.Object key)
	{
		if(!m_Table.Contains(key))
			return null;
		else
			return m_Table[key];
	}

	public System.Object GetHashtableValue(System.Object key, out bool contains)
	{
		if(!m_Table.Contains(key))
		{
			contains = false;
			return null;
		}
		else
		{
			contains = true;
			return m_Table[key];
		}
	}

	public void ClearHashtableValue(System.Object key)
	{
		if(m_Table.ContainsKey(key))
			m_Table.Remove(key);
	}

	public object GetBasicInfo(string key)
	{
		bool contains;
		Statistic stat = (Statistic)GetHashtableValue("BasicInfo", out contains);

		if(contains)
		{
			return (string)stat.GetHashtableValue(key);
		}
		else
			return null;
	}

	public int GetAttributeValue(string key)
	{
		bool contains;
		Statistic stat = (Statistic)GetHashtableValue("Attributes" + key, out contains);

		if(contains)
			return stat.GetStatValue();
		else
			return 0;
	}

	public int GetSkillValue(string category, int key)
	{
		Statistic[] stats = (Statistic[])GetHashtableValue("Skills" + category);

		return stats[key].GetStatValue();
	}

	public int GetSkillValue(string category, string key)
	{
		Statistic[] stats = (Statistic[])GetHashtableValue("Skills" + category);

		int iKey = 0;
		int value = 0;
		if(category == SkillsCategories.Mental.ToString())
		{
			iKey = (int)Enum.Parse(typeof(SkillsMental), key);
			value = stats[iKey].GetStatValue();
			if(value == 0)
				value = -3;
		}
		if(category == SkillsCategories.Physical.ToString())
		{
			iKey = (int)Enum.Parse(typeof(SkillsPhysical), key);
			value = stats[iKey].GetStatValue();
			if(value == 0)
				value = -1;
		}
		if(category == SkillsCategories.Social.ToString())
		{
			iKey = (int)Enum.Parse(typeof(SkillsSocial), key);
			value = stats[iKey].GetStatValue();
			if(value == 0)
				value = -1;
		}

		return value;
	}

	public string GetName()
	{
		bool contains;
		GetHashtableValue(ConsistentVariables.MainInfo.ToString(), out contains);

		if(contains)
		{
			Statistic stat = (Statistic)GetHashtableValue(Character.ConsistentVariables.MainInfo.ToString());
			string name = (string)stat.GetHashtableValue("Name");
			return name;
		}

		return "";
	}

	public byte[] GetImage()
	{
		bool contains;
		GetHashtableValue(Character.ConsistentVariables.Image.ToString(), out contains);
		
		if(contains)
		{
			byte[] image = (byte[])GetHashtableValue(Character.ConsistentVariables.Image.ToString());
			return image;
		}
		
		return null;
	}

	public CharacterTypes GetCharacterType()
	{
		bool contains;
		GetHashtableValue(ConsistentVariables.CharacterType.ToString(), out contains);
		
		if(contains)
		{
			return (CharacterTypes)GetHashtableValue(ConsistentVariables.CharacterType.ToString());
		}
		
		return CharacterTypes.PC;
	}

	public void SetInstanceID(int id){ SetHashtableValue(ConsistentVariables.ID.ToString(), id); }
	public int GetInstanceID()
	{
		bool contains;
		GetHashtableValue(ConsistentVariables.ID.ToString(), out contains);

		if(contains)
			return (int)GetHashtableValue(ConsistentVariables.ID.ToString());

		return 0;
	}

	public string GetKey(){ return GetName() + GetInstanceID(); }

	public enum ConsistentVariables
	{
		ID,
		Image,
		BasicInfo,
		MainInfo,
		CharacterType,
		AttributesIntelligence,
		AttributesWits,
		AttributesResolve,
		AttributesStrength,
		AttributesDexterity,
		AttributesStamina,
		AttributesPresence,
		AttributesManipulation,
		AttributesComposure
	}

	public enum StatCategories
	{
		BasicInfo,
		Attributes,
		Skills
	}


	public enum Attributes
	{
		Intelligence,
		Wits,
		Resolve,
		Strength,
		Dexterity,
		Stamina,
		Presence,
		Manipulation,
		Composure
	}

	public enum BasicInfo
	{
		Size,
		Speed,
		Defense,
		Armor,
		Initiative
	}

	public enum SkillsCategories
	{
		Mental,
		Physical,
		Social
	}

	public enum SkillsMental
	{
		Academics,
		Computer,
		Crafts,
		Investigation,
		Medicine,
		Occult,
		Politics,
		Science
	}

	public enum SkillsPhysical
	{
		Athletics,
		Brawl,
		Drive,
		Firearms,
		Larceny,
		Stealth,
		Survival,
		Weaponry
	}

	public enum SkillsSocial
	{
		Animal_Ken,
		Empathy,
		Expression,
		Intimidation,
		Persuasion,
		Socialize,
		Streetwise,
		Subterfuge
	}
}

public enum CharacterTypes
{
	PC, //Player Character (Only one of each type on field)
	ImportantNPC, //Important Non-Player (Share same character object)
	UnimportantNPC //Non-Important Non-Player (Don't share same character object)
}
