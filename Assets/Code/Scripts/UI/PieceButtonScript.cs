using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PieceButtonScript : MonoBehaviour {

	public Text buttonText;

	private int pieceType;
	private int blockType;
	public void SetParameters(string name, int typeOfPiece, int type)
	{
		buttonText.text = name;
		pieceType = typeOfPiece;
		blockType = type;
	}

	public void OnButtonClick()
	{
		BuildingManager.Instance().currentSelectedType = (StageBlockType)blockType;
		BuildingManager.Instance().SetBuildingMode(BuildingManager.BuildingMode.placeMode);
		BuildingManager.Instance().m_PiecePlacingType = (StagePieceType)pieceType;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
