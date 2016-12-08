using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadImageFilePanelScript : MonoBehaviour {

	public InputField path;

	public void LoadImage()
	{
		string imgPath = path.text;

		CharacterSheetScript.FindCharacterSheet().SetPortrait(imgPath);
	}

	public void Cancel()
	{
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
