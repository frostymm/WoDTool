using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	public Text versionText;
	public GameObject mainMenuPanel;
	public GameObject titlePanel;
	public GameObject onlineMenuPanel;

	public void DisableMainPanels()
	{
		if(mainMenuPanel.activeSelf)
			mainMenuPanel.SetActive(false);
		if(titlePanel.activeSelf)
			titlePanel.SetActive(false);
		if(onlineMenuPanel.activeSelf)
			onlineMenuPanel.SetActive(false);
	}
	
	public void EnablePanel(string panel)
	{
		DisableMainPanels();

		if(panel == "main")
		{
			if(!mainMenuPanel.activeSelf)
				mainMenuPanel.SetActive(true);
			if(!titlePanel.activeSelf)
				titlePanel.SetActive(true);
		}
		else if(panel == "online")
		{
			if(!onlineMenuPanel.activeSelf)
				onlineMenuPanel.SetActive(true);
		}
	}

	public void LoadMain()
	{
		EnablePanel("main");
		NetworkManager.Instance().Disconnect();
		PhotonNetwork.offlineMode = false;
	}

	private bool m_LoadingOnline = false;
	public void LoadOnlineMode()
	{
		NetworkManager.Instance().Connect();

		m_LoadingOnline = true;
	}

	public void LoadOfflineMode()
	{
		GameManager.Instance().LoadOfflineMode();
	}

	public void Quit()
	{
		GameManager.Instance().QuitGame();
	}

	// Use this for initialization
	void Start () {
		versionText.text = "WoDder Version: " + GameManager.Instance().GetVersion();

		EnablePanel("main");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(m_LoadingOnline && !PhotonNetwork.connecting)
		{
			m_LoadingOnline = false;

			if(PhotonNetwork.connected)
				EnablePanel("online");
		}
	}
}
