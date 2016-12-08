using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileToLoadObjectScript : MonoBehaviour {

	public Text fileName;
	public Image img;

	public void SetStageToLoad()
	{
		if(GameManager.Instance().isDebugMode)
			Debug.Log("Setting Stage To Load: " + fileName.text);

		LevelManager.Instance().SetStageToLoad(fileName.text);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
