using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CoinObjectScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public Image coinFace;
	public GameObject[] coinMats;

	public void SetMats(Material newMat)
	{
		for(int i = coinMats.Length -1; i >= 0; i--)
		{
			coinMats[i].GetComponent<Renderer>().material = newMat;
		}
	}

	private Vector3 m_BeginDragPos;
	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("Begin Drag");
		LevelManager.Instance().isDragging = true;
		gameObject.GetComponent<PieceObjectScript>().beingDragged = true;

		m_BeginDragPos = gameObject.transform.position;
		gameObject.transform.parent = LevelManager.Instance().GetHighlighterBox().transform;
		gameObject.transform.localPosition = Vector3.zero;

		gameObject.GetComponent<BoxCollider>().enabled = false;
	}
	
	public void OnDrag(PointerEventData data)
	{
		Debug.Log("OnDrag");
	}
	
	private void SetDraggedPosition(PointerEventData data) // not happening right now?
	{
		Debug.Log("SetDragged");
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("End Drag");
		gameObject.transform.parent = null;

		//BuildingManager.Instance().MovePiece(m_BeginDragPos, LevelManager.Instance().GetSelectedPosition());
		NetworkManager.Instance().NMPhotonView.RPC("MovePiece", PhotonTargets.All, 
		                                           m_BeginDragPos, LevelManager.Instance().GetSelectedPosition());

		LevelManager.Instance().isDragging = false;
		gameObject.GetComponent<PieceObjectScript>().beingDragged = false;
		gameObject.GetComponent<BoxCollider>().enabled = true;
	}
}
