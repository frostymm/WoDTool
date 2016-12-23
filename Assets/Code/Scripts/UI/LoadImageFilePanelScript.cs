//Script attached to panel that pops up when you attempt to set an image for a character

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
}
