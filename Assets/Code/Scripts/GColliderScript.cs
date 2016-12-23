/*
 * The ground collider covering the map
 * This is simply used to figure out where the player is pointing
 * */

using UnityEngine;
using System.Collections;

public class GColliderScript : MonoBehaviour {

	bool hoveringOver;

	public void OnPointerEnter()
	{
		LevelManager.Instance().SetSelectedPosition(LevelManager.Instance().CalcGridColSelectedPosition());
		hoveringOver = true;
	}
	
	public void OnPointerExit()
	{
		hoveringOver = false;
	}

	void Update()
	{
		if(hoveringOver)
		{
			LevelManager.Instance().SetSelectedPosition(LevelManager.Instance().CalcGridColSelectedPosition());
		}
	}
}
