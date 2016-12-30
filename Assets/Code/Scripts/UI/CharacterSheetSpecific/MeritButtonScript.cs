/* This script is for the merit and category buttons
 * They're only technically buttons because I'm using the hover effect on them
 * It handles when the description panel is spawned and populated as well as 
 * what bubble buttons are selectable when choosing a merit
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MeritButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText;
    public bool isCategory = false;

    private List<int> m_bubblesToEnable = new List<int>();
    private SpreadSheetMerit m_ReferenceMerit;
    public void AddMerit(SpreadSheetMerit SSM, int value)
    {
        m_bubblesToEnable.Add(value - 1); //Minus 1 to account for the difference between value and index on buttonfields

        if(m_ReferenceMerit == null)
            m_ReferenceMerit = SSM;
    }

    public void SetBubbleData()
    {
        GetBFS().TurnButtonsBlackAndFreeze(m_bubblesToEnable.ToArray());
        GetBFS().DisableBubbles(new int[5] { 0, 1, 2, 3, 4 });
        GetBFS().EnableBubbles(m_bubblesToEnable.ToArray());
    }

    public void OnPointerEnter(PointerEventData pData)
    {
        if(isCategory)
        {
            GameObject.FindObjectOfType<AddMeritDropDownScript>().SetCategoryPanelActive(buttonText.text);
        }
        else
        {
            DescriptionPanelScript DPS = GameObject.FindObjectOfType<FlawsnMeritsPanelScript>().descriptionPanel.GetComponent<DescriptionPanelScript>();
            DPS.gameObject.SetActive(true);

            //Attach to root panel to avoid drifting off screen
            DPS.transform.position = GameObject.FindObjectOfType<AddMeritDropDownScript>().transform.position;
            DPS.SetInfo(m_ReferenceMerit);
        }
    }

    //Clear category list or description panel
    public void OnPointerExit(PointerEventData pData)
    {
        if(isCategory)
        {
            GameObject.FindObjectOfType<AddMeritDropDownScript>().SetCategoryPanelActive("");
        }
        else
        {
            DescriptionPanelScript DPS = GameObject.FindObjectOfType<DescriptionPanelScript>();
            DPS.gameObject.SetActive(false);
        }
    }

    private BubbleFieldScript m_BFS;
    private BubbleFieldScript GetBFS()
    {
        if(m_BFS == null)
            m_BFS = GetComponentInChildren<BubbleFieldScript>();
        return m_BFS;
    }
    // Use this for initialization
    void Start () {
		if(!isCategory)
        {
            GetBFS().isAttachedToMerit = true;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GetBFS() != null)
        {
            //If the player has clicked on a value in the bubblefield, this saves me a bit of referencing as the bubble field is used in almost everything at this point
            if(GetBFS().GetValue() != 0 && m_ReferenceMerit != null)
            {
                GameObject.FindObjectOfType<FlawsnMeritsPanelScript>().CreateNewUIObject(m_ReferenceMerit, GetBFS().GetValue());
                GameObject.FindObjectOfType<AddMeritDropDownScript>().gameObject.SetActive(false);
                GetBFS().SetValue(0);

                DescriptionPanelScript DPS = GameObject.FindObjectOfType<DescriptionPanelScript>();
                if (DPS != null)
                    DPS.gameObject.SetActive(false);
            }
        }
	}
}
