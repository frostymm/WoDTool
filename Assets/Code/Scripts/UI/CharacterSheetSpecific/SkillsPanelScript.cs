using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SkillsPanelScript : MonoBehaviour {

	public GameObject MentalSkillsContent;
	public GameObject PhysicalSkillsContent;
	public GameObject SocialSkillsContent;

	public List<StatObjectScript> mentalSkills = new List<StatObjectScript>();
	public List<StatObjectScript> physicalSkills = new List<StatObjectScript>();
	public List<StatObjectScript> socialSkills = new List<StatObjectScript>();

	public void SaveToCharacterObject()
	{
		GameManager.Instance().selectedCharacter.SetHashtableValue("SkillsMental", FetchSkills ("Mental"));
		GameManager.Instance().selectedCharacter.SetHashtableValue("SkillsPhysical", FetchSkills ("Physical"));
		GameManager.Instance().selectedCharacter.SetHashtableValue("SkillsSocial", FetchSkills ("Social"));
	}

	private Statistic[] FetchSkills(string category)
	{
		List<Statistic> tempStats = new List<Statistic>();
		if(category == "Mental")
		{
			foreach(StatObjectScript sos in mentalSkills)
			{
				sos.PopulateStatisticData();
				tempStats.Add(sos.statistic);
			}

		}
		
		if(category == "Physical")
		{
			foreach(StatObjectScript sos in physicalSkills)
			{
				sos.PopulateStatisticData();
				tempStats.Add(sos.statistic);
			}

		}
		
		if(category == "Social")
		{
			foreach(StatObjectScript sos in socialSkills)
			{
				sos.PopulateStatisticData();
				tempStats.Add(sos.statistic);
			}

		}
		
		return tempStats.ToArray();
	}

	private Statistic[] FetchDefaultSkills(string category)
	{
		List<Statistic> tempStats = new List<Statistic>();
		if(category == "Mental")
		{
			int[] skills =  (int[])Enum.GetValues(typeof(Character.SkillsMental));
			
			foreach(Character.SkillsMental skill in skills)
			{
				tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
					new KeyValuePair<System.Object, System.Object>("StatName", skill.ToString())}));
			}

			/*tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
			    new KeyValuePair<System.Object, System.Object>("StatName", "Academics")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Computer")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Crafts")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Investigation")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Medicine")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Occult")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Politics")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Science")}));*/
		}

		if(category == "Physical")
		{
			int[] skills =  (int[])Enum.GetValues(typeof(Character.SkillsPhysical));
			
			foreach(Character.SkillsPhysical skill in skills)
			{
				tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
					new KeyValuePair<System.Object, System.Object>("StatName", skill.ToString())}));
			}

			/*tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Athletics")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Brawl")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Drive")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Firearms")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Larceny")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Stealth")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Survival")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Weaponry")}));*/
		}

		if(category == "Social")
		{
			int[] skills =  (int[])Enum.GetValues(typeof(Character.SkillsSocial));
			
			foreach(Character.SkillsSocial skill in skills)
			{
				tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
					new KeyValuePair<System.Object, System.Object>("StatName", skill.ToString())}));
			}

			/*tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Animal Ken")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Empathy")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Expression")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Intimidation")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Persuasion")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Socialize")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Streetwise")}));
			tempStats.Add(new Statistic(new KeyValuePair<System.Object, System.Object>[1]{
				new KeyValuePair<System.Object, System.Object>("StatName", "Subterfuge")}));*/
		}

		return tempStats.ToArray();
	}

	private void CreateSkillsObjects(string category, Statistic[] stats)
	{
		GameObject contentBox;
		List<StatObjectScript> skills;

		if(category == "Mental")
		{
			contentBox = MentalSkillsContent;
			skills = mentalSkills;
		}
		else if(category == "Physical")
		{
			contentBox = PhysicalSkillsContent;
			skills = physicalSkills;
		}
		else
		{
			contentBox = SocialSkillsContent;
			skills = socialSkills;
		}

		foreach(Statistic stat in stats)
		{
			GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/StatObject"),
			                                                   Vector3.zero, Quaternion.identity);

			go.transform.SetParent(contentBox.transform, false);
			
			go.transform.localPosition = new Vector2(0, (skills.Count * -35) + -17.5f);
			
			StatObjectScript sos = go.GetComponent<StatObjectScript>();

			sos.statistic = stat;
			sos.statFields[0].key = "StatName";
			sos.statFields[1].key = "Specialty";
			sos.LoadFromStatistic();
			
			skills.Add(sos);
		}

		Utilities.UI.UpdateContentBoxSize(contentBox, skills.Count, 35f);
	}

	private void PopulateSkillsContent()
	{
		bool populated = false;
		Statistic[] stats;

		GameManager.Instance().selectedCharacter.GetHashtableValue("SkillsMental", out populated);
		if(populated)
		{
			stats = (Statistic[])GameManager.Instance().selectedCharacter.GetHashtableValue("SkillsMental");
			CreateSkillsObjects("Mental", stats);
		}
		else
		{
			stats = FetchDefaultSkills("Mental");
			CreateSkillsObjects("Mental", stats);
		}

		GameManager.Instance().selectedCharacter.GetHashtableValue("SkillsPhysical", out populated);
		if(populated)
		{
			stats = (Statistic[])GameManager.Instance().selectedCharacter.GetHashtableValue("SkillsPhysical");
			CreateSkillsObjects("Physical", stats);
		}
		else
		{
			stats = FetchDefaultSkills("Physical");
			CreateSkillsObjects("Physical", stats);
		}

		GameManager.Instance().selectedCharacter.GetHashtableValue("SkillsSocial", out populated);
		if(populated)
		{
			stats = (Statistic[])GameManager.Instance().selectedCharacter.GetHashtableValue("SkillsSocial");
			CreateSkillsObjects("Social", stats);
		}
		else
		{
			stats = FetchDefaultSkills("Social");
			CreateSkillsObjects("Social", stats);
		}
	}

	// Use this for initialization
	void Start () {
		PopulateSkillsContent();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
