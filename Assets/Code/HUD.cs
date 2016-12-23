/*
 * This is the script attached to the HUD object when the game starts
 * It's really only used to switch between the board and building game modes
 * */

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
		}
		else if(hudType == "board")
		{
			if(buildingHUD.activeSelf)
				buildingHUD.SetActive(false);
			if(!boardHUD.activeSelf)
				boardHUD.SetActive(true);

			GameManager.Instance().inBuildMode = false;
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
