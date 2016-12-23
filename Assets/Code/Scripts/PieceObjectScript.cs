/*
 * Both Tokens and Blocks have this script
 * It is used to tell when a player attempts to place one piece on top of another, returns position relative to piece rather than ground collider
 * */

using UnityEngine;
using System.Collections;

public class PieceObjectScript : MonoBehaviour {

	private Vector3 m_SavedNormal = Vector3.zero;
	private bool m_IsSelected = false;

	private bool m_BeingDragged = false;
	public bool beingDragged { get{ return m_BeingDragged; } set{ m_BeingDragged = value; } }

	public void NormalChangeUpdate()
	{
		if(m_SavedNormal != LevelManager.Instance().GetMouseHit().normal)
			OnPointerEnter();
	}

	public void OnPointerEnter()
	{
		m_IsSelected = true;

		if(!beingDragged)
		{
			Vector3 selectedPosition = transform.position;

			if((GameManager.Instance().inBuildMode && BuildingManager.Instance().buildingMode == BuildingManager.BuildingMode.placeMode)
			   || (!GameManager.Instance().inBuildMode && LevelManager.Instance().isDragging))
			{
				m_SavedNormal = LevelManager.Instance().GetMouseHit().normal;

				if(m_SavedNormal.y == 0)
				{
					selectedPosition = selectedPosition + 
						(m_SavedNormal * GameManager.blockWidth);
				}
				else
				{
					selectedPosition = selectedPosition + 
						(m_SavedNormal * GameManager.blockHeight);
				}
			}

			LevelManager.Instance().SetSelectedPosition(selectedPosition);
		}
	}

	public void OnPointerExit()
	{
		m_IsSelected = false;
	}

	public void FixedUpdate()
	{
		if(m_IsSelected)
			NormalChangeUpdate();
	}
}
