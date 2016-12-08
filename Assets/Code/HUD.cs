using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	public GameObject buildingHUD;
	public GameObject boardHUD;

	public static HUD GetHUD(){ return GameObject.FindObjectOfType<HUD>().GetComponent<HUD>(); }

	public GameObject syncingPanel;
	public void SetSyncingStageData(bool syncing)
	{
		syncingPanel.SetActive(syncing);
	}

	public void SwitchHUD(string hudType)
	{
		if(hudType == "build")
		{
			if(boardHUD.activeSelf)
				boardHUD.SetActive(false);
			if(!buildingHUD.activeSelf)
				buildingHUD.SetActive(true);

			GameManager.Instance().inBuildMode = true;
			LevelManager.Instance().EnterBuildMode();
		}
		else if(hudType == "board")
		{
			if(buildingHUD.activeSelf)
				buildingHUD.SetActive(false);
			if(!boardHUD.activeSelf)
				boardHUD.SetActive(true);

			GameManager.Instance().inBuildMode = false;
			LevelManager.Instance().ExitBuildMode();
		}
	}

	public void OnEnter()
	{
		LevelManager.Instance().isPositionSelected = false;
	}

	// Use this for initialization
	void Start () {
		if(boardHUD.activeSelf)
			GameManager.Instance().inBuildMode = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
