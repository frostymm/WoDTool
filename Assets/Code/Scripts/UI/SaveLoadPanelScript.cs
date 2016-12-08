using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class SaveLoadPanelScript : MonoBehaviour {

	public GameObject SavePanel;
	public GameObject LoadPanel;

	public GameObject LoadPanelContentBox;
	
	public void SetStageName(Text name)
	{
		if(name.text.Length > 0)
			LevelManager.Instance().stageName = name.text;
		else
			LevelManager.Instance().stageName = "default";
	}

	public void OpenSavePanel()
	{
		BuildingHUD.GetBuildingHUD().DisableMainPanels();

		if(!SavePanel.activeSelf)
			SavePanel.SetActive(true);
	}

	public void CloseSavePanel()
	{
		if(SavePanel.activeSelf)
			SavePanel.SetActive(false);

		BuildingHUD.GetBuildingHUD().EnableMainPanels();
	}

	public void OpenLoadPanel()
	{
		BuildingHUD.GetBuildingHUD().DisableMainPanels();
		
		if(!LoadPanel.activeSelf)
			LoadPanel.SetActive(true);

		PopulateFileList();
	}
	
	public void CloseLoadPanel()
	{
		if(LoadPanel.activeSelf)
			LoadPanel.SetActive(false);
		
		BuildingHUD.GetBuildingHUD().EnableMainPanels();
	}

	List<GameObject> stageFiles = new List<GameObject>();
	public void DeleteOldFileList()
	{
		foreach(GameObject go in stageFiles)
		{
			Destroy(go);
		}

		stageFiles.Clear();
		SelectedStageFileIndex = -1;
	}

	private int SelectedStageFileIndex = -1;
	public int GetSelectedStageFileIndex(){ return SelectedStageFileIndex; }
	public void SetSelectedStageFile(GameObject go)
	{
		if(stageFiles.Contains(go))
		{
			//Set old selected file to default
			Button b = stageFiles[SelectedStageFileIndex].GetComponent<Button>();
			ColorBlock cb = b.colors;
			cb.normalColor = Color.black; 
			b.colors = cb;

			SelectedStageFileIndex = stageFiles.IndexOf(go);

			//Set new selected file to highlighted
			b = stageFiles[SelectedStageFileIndex].GetComponent<Button>();
			cb = b.colors;
			cb.normalColor = Color.white;
			b.colors = cb;
		}
	}

	public void PopulateFileList()
	{
		DeleteOldFileList();

		string[] files =  Directory.GetFiles(Application.persistentDataPath + "/StageData/");

		foreach(string stage in files)
		{
			GameObject go = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/StageToLoadObject"),
			                       Vector3.zero, Quaternion.identity);

			go.transform.SetParent(LoadPanelContentBox.transform, false);

			go.transform.localPosition = new Vector2(0, stageFiles.Count * -100);

			go.GetComponent<FileToLoadObjectScript>().fileName.text = Path.GetFileNameWithoutExtension(stage);

			stageFiles.Add(go);
		}

		LoadPanelContentBox.GetComponent<RectTransform>().sizeDelta = new Vector2(650, stageFiles.Count * 100);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
	}
}
