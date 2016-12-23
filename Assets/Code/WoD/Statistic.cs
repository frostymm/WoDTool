/*
 * A stat class for storing statistic data
 * I made it so that it could input any type of data and any amount it needed
 * */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Statistic {

	private Hashtable m_Table = new Hashtable(); // table for miscellanious values

	private bool m_Dirty = false;
	public bool isDirty
	{
		get
		{
			if(m_Dirty)
			{
				m_Dirty = false;
				return true;
			}

			return false;
		}
		set { m_Dirty = value; }
	}

	public Statistic(params KeyValuePair<System.Object, System.Object>[] hashelements)
	{
		foreach(KeyValuePair<System.Object, System.Object> pair in hashelements)
		{
			SetHashtableValue(pair.Key, pair.Value);
		}
	}

	public void SetHashtableValue(System.Object key, System.Object val)
	{
		if(!m_Table.Contains(key))
			m_Table.Add(key, val);
		else
			m_Table[key] = val;

		isDirty = true;
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

	public int GetStatValue()
	{
		int value = (int)GetHashtableValue("StatValue");

		return value;
	}
}
