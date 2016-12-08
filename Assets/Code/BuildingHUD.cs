using UnityEngine;
using System.Collections;

public class BuildingHUD : MonoBehaviour {

	public GameObject BlocksPanel;
	public GameObject ElevationPanel;
	public GameObject SaveLoadButtonPanel;
	public GameObject DeletePanel;

	public static BuildingHUD GetBuildingHUD(){ return GameObject.FindObjectOfType<BuildingHUD>().GetComponent<BuildingHUD>(); }

	private bool m_CharacterListNeedsRepopulate = true;
	public bool characterListNeedsPopulate
	{
		get{ return m_CharacterListNeedsRepopulate; }
		set{ m_CharacterListNeedsRepopulate = value; }
	}


	public void DisableMainPanels()
	{
		if(BlocksPanel.activeSelf)
			BlocksPanel.SetActive(false);
		if(ElevationPanel.activeSelf)
			ElevationPanel.SetActive(false);
		if(SaveLoadButtonPanel.activeSelf)
			SaveLoadButtonPanel.SetActive(false);
		if(DeletePanel.activeSelf)
			DeletePanel.SetActive(false);
	}
	
	public void EnableMainPanels()
	{
		if(!BlocksPanel.activeSelf)
			BlocksPanel.SetActive(true);
		if(!ElevationPanel.activeSelf)
			ElevationPanel.SetActive(true);
		if(!SaveLoadButtonPanel.activeSelf)
			SaveLoadButtonPanel.SetActive(true);
		if(!DeletePanel.activeSelf)
			DeletePanel.SetActive(true);
	}

	public void SetBuildingModeDelete()
	{
		BuildingManager.Instance().SetBuildingMode(BuildingManager.BuildingMode.deleteMode);
	}

	public void DeleteStageFile()
	{
		LevelManager.Instance().DeleteStage();
	}

	public void SaveStage()
	{
		LevelManager.Instance().SaveStage();
	}

	public void LoadStage()
	{
		LevelManager.Instance().LoadStage();
	}

	public void ChangeBuildingElevation(int i)
	{
		BuildingManager.Instance().ChangeElevation(i);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
