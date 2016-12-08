using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlawsnMeritsPanelScript : MonoBehaviour {

	public GameObject flawsContentBox;
	public GameObject meritsContentBox;
	
	List<StatObjectScript> m_Flaws = new List<StatObjectScript>();
	List<StatObjectScript> m_Merits = new List<StatObjectScript>();

	public void SaveToCharacterObject()
	{
		GameManager.Instance().selectedCharacter.SetHashtableValue("Flaws", FetchFlaws());
		GameManager.Instance().selectedCharacter.SetHashtableValue("Merits", FetchMerits());
	}

	private Statistic[] FetchFlaws()
	{
		List<Statistic> tempStats = new List<Statistic>();
		
		foreach(StatObjectScript sos in m_Flaws)
		{
			sos.PopulateStatisticData();
			tempStats.Add(sos.statistic);
		}
		
		
		return tempStats.ToArray();
	}

	private Statistic[] FetchMerits()
	{
		List<Statistic> tempStats = new List<Statistic>();
		
		foreach(StatObjectScript sos in m_Merits)
		{
			sos.PopulateStatisticData();
			tempStats.Add(sos.statistic);
		}
		
		
		return tempStats.ToArray();
	}

	private void CreateUIObjects(Statistic[] stats, string category)
	{
		List<StatObjectScript> statScripts;
		string prefabToLoad;
		GameObject contentBox;

		if(category == "Flaws")
		{
			statScripts = m_Flaws;
			prefabToLoad = "Prefabs/UI/StatObjectFlaw";
			contentBox = flawsContentBox;
		}
		else
		{
			statScripts = m_Merits;
			prefabToLoad = "Prefabs/UI/StatObjectMerit";
			contentBox = meritsContentBox;
		}

		foreach(Statistic stat in stats)
		{
			GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(prefabToLoad),
			                                                   Vector3.zero, Quaternion.identity);
			
			go.transform.SetParent(contentBox.transform, false);
			go.transform.localPosition = new Vector2(0, (statScripts.Count * -35) + -17.5f);
			
			StatObjectScript sos = go.GetComponent<StatObjectScript>();
			sos.statistic = stat;
			sos.LoadFromStatistic();
			statScripts.Add(sos);
		}
		
		Utilities.UI.UpdateContentBoxSize(contentBox, statScripts.Count, 35f);
	}

	public void CreateNewUIObject(string category)
	{
		List<StatObjectScript> statScripts;
		string prefabToLoad;
		GameObject contentBox;
		
		if(category == "Flaws")
		{
			statScripts = m_Flaws;
			prefabToLoad = "Prefabs/UI/StatObjectFlaw";
			contentBox = flawsContentBox;
		}
		else
		{
			statScripts = m_Merits;
			prefabToLoad = "Prefabs/UI/StatObjectMerit";
			contentBox = meritsContentBox;
		}

		GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(prefabToLoad),
		                                                   Vector3.zero, Quaternion.identity);
		
		go.transform.SetParent(contentBox.transform, false);
		go.transform.localPosition = new Vector2(0, (statScripts.Count * -35) + -17.5f);

		StatObjectScript sos = go.GetComponent<StatObjectScript>();
		sos.statistic = new Statistic();
		sos.LoadFromStatistic();
		statScripts.Add(sos);
		
		Utilities.UI.UpdateContentBoxSize(contentBox, statScripts.Count, 35f);
	}
	
	public void DestroyLatestUIObject(string category)
	{
		List<StatObjectScript> statScripts;

		if(category == "Flaws")
			statScripts = m_Flaws;
		else
			statScripts = m_Merits;

		if(statScripts.Count > 0)
		{
			StatObjectScript sos = statScripts[statScripts.Count-1];
			
			Destroy(sos.gameObject);
			statScripts.Remove(sos);
		}
	}

	private void PopulateContent()
	{
		bool populated = false;
		Statistic[] stats;
		
		GameManager.Instance().selectedCharacter.GetHashtableValue("Flaws", out populated);
		if(populated)
		{
			stats = (Statistic[])GameManager.Instance().selectedCharacter.GetHashtableValue("Flaws");
			CreateUIObjects(stats, "Flaws");
		}

		GameManager.Instance().selectedCharacter.GetHashtableValue("Merits", out populated);
		if(populated)
		{
			stats = (Statistic[])GameManager.Instance().selectedCharacter.GetHashtableValue("Merits");
			CreateUIObjects(stats, "Merits");
		}
	}
	
	// Use this for initialization
	void Start () {
		PopulateContent();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
