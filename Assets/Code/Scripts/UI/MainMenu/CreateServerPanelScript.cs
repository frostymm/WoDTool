//Room creation panel script

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateServerPanelScript : MonoBehaviour {

	public Text chronicleText;

	public void CreateServer()
	{
		NetworkManager.Instance().CreateServer(chronicleText.text);
	}

	public void Cancel()
	{
		gameObject.SetActive(false);
	}
}
