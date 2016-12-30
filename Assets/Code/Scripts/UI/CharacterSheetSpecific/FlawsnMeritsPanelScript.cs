//script for UI panel that contains both the merits and the flaws

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlawsnMeritsPanelScript : MonoBehaviour {

	public GameObject flawsContentBox;
	public GameObject meritsContentBox;
    public GameObject addMeritsPanel;
    public GameObject descriptionPanel;

    List<StatObjectScript> m_Flaws = new List<StatObjectScript>();
	List<StatObjectScript> m_Merits = new List<StatObjectScript>();

    //The data for flaws and merits are contained in one list for each rather than having a value for every
    //individual one, it even allows for duplicates of some things and makes it much easier to delete data that's had its name changed
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

    //populate current list of merits or flaws
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

    //add a single UI object to list of merits or flaws
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

    //Create editable merit in character merits list based on dropdown choice
    public void CreateNewUIObject(SpreadSheetMerit merit, int value)
    {
        List<StatObjectScript> statScripts = m_Merits;
        string prefabToLoad = "Prefabs/UI/StatObjectMerit";
        GameObject contentBox = meritsContentBox;

        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(prefabToLoad),
                                                           Vector3.zero, Quaternion.identity);

        go.transform.SetParent(contentBox.transform, false);
        go.transform.localPosition = new Vector2(0, (statScripts.Count * -35) + -17.5f);

        StatObjectScript sos = go.GetComponent<StatObjectScript>();
        sos.statistic = new Statistic();
        sos.statistic.SetHashtableValue("StatValue", value);
        sos.statistic.SetHashtableValue("StatName", merit.RetrieveData(SpreadSheetMerit.MeritData.name));
        sos.LoadFromStatistic();
        statScripts.Add(sos);

        Utilities.UI.UpdateContentBoxSize(contentBox, statScripts.Count, 35f);
    }
	
    //Currently only support to remove latest flaw or merit because previous ones can just be changed
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

    //This is called via UI button and therefore has no references otherwise
    public void ActivateAddMeritPanel()
    {
        addMeritsPanel.SetActive(true);
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
