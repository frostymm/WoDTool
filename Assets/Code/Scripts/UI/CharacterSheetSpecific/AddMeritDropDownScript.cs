/*This script handles the base functionality of the new Merit drop down option
 * It sorts by category
 * Displays all possible value's for a merit (If a dot is blank, it is not selectable because that value for the merit doesn't exist)
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddMeritDropDownScript : MonoBehaviour, IPointerExitHandler
{
    //Clear dropDownLists on pointer exit
    public void OnPointerExit(PointerEventData pData)
    {
        gameObject.SetActive(false);

        DescriptionPanelScript DPS = GameObject.FindObjectOfType<DescriptionPanelScript>();
        if(DPS != null)
            DPS.gameObject.SetActive(false);
    }

    //Content boxes that contain each categories list of merits
    private Dictionary<string, GameObject> m_CategoryPanels = new Dictionary<string, GameObject>();
    public void SetCategoryPanelActive(string category) //Pass blank string to disable all of them
    {
        foreach(KeyValuePair<string, GameObject> pair in m_CategoryPanels)
        {
            pair.Value.SetActive(pair.Key == category);
        }
    }

    //The total loaded merits are seperated into lists based on category to make things easier
    private Dictionary<string, List<SpreadSheetMerit>> m_LoadedSpreadSheetMerits;
    private bool m_ListCreated = false;
    public void PopulateAddMeritPanel()
    {
        if (m_LoadedSpreadSheetMerits == null)
            m_LoadedSpreadSheetMerits = SpreadSheetManager.Instance().LoadMeritList("Merits");

        //Only create the list once per scene
        if(!m_ListCreated)
        {
            foreach (KeyValuePair<string, List<SpreadSheetMerit>> category in m_LoadedSpreadSheetMerits)
            {
                //Populate category buttons
                GameObject categoryButton = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/CharacterSheetSpecific/MeritCategoryButton"),
                                                                  Vector3.zero, Quaternion.identity);
                categoryButton.transform.SetParent(gameObject.transform, false);
                categoryButton.GetComponent<MeritButtonScript>().buttonText.text = category.Key.ToString();
                categoryButton.GetComponent<MeritButtonScript>().isCategory = true;

                //For each category, create a panel to populate with merits
                GameObject categoryPanel = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/GenericPanelw_BG"),
                                                                  Vector3.zero, Quaternion.identity);
                GameObject categoryPanelContent = categoryPanel.GetComponentInChildren<GridLayoutGroup>().gameObject;
                categoryPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(260f, 100f);
                categoryPanel.transform.SetParent(categoryButton.transform, false);
                categoryPanel.SetActive(false);
                m_CategoryPanels.Add(category.Key.ToString(), categoryPanel);

                //for each panel, fill with merits from that category
                Dictionary<string, MeritButtonScript> meritsWithMultipleValues = new Dictionary<string, MeritButtonScript>();
                foreach (SpreadSheetMerit merit in category.Value)
                {
                    //account for multiple inputs of the same merit (I did this to account for varying possible values
                    //It would have been simpler to have a min value and max value but there are some merits
                    //in which you can only choose between specific values (I.E. 2 or 4, no 3)
                    if(meritsWithMultipleValues.ContainsKey(merit.GetName()))
                    {
                        meritsWithMultipleValues[merit.GetName()].AddMerit(merit, merit.GetValueAsInt());
                    }
                    else
                    {
                        GameObject meritButton = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/CharacterSheetSpecific/AddMeritButton"),
                                                                      Vector3.zero, Quaternion.identity);
                        meritButton.transform.SetParent(categoryPanelContent.transform, false);
                        MeritButtonScript MBS = meritButton.GetComponent<MeritButtonScript>();
                        MBS.buttonText.text = merit.GetName();
                        MBS.isCategory = false;

                        MBS.AddMerit(merit, merit.GetValueAsInt());
                        meritsWithMultipleValues.Add(merit.GetName(), MBS);
                    }
                }

                //Set bubble data once we know the possible values for each merit
                foreach(MeritButtonScript MBS in meritsWithMultipleValues.Values)
                {
                    MBS.SetBubbleData();
                }

                //update length of content boxes to completely encapsulate new contents
                int numberOfItemsToDisplay = 6;
                Utilities.UI.UpdateContentBoxSize(categoryPanel, numberOfItemsToDisplay, categoryPanelContent.GetComponent<GridLayoutGroup>().cellSize.y); //Set maskSize
                Utilities.UI.UpdateContentBoxSize(categoryPanelContent, meritsWithMultipleValues.Count, categoryPanelContent.GetComponent<GridLayoutGroup>().cellSize.y);

            }

            //Update length of category list plus one for custom merit button
            Utilities.UI.UpdateContentBoxSize(gameObject, m_LoadedSpreadSheetMerits.Count + 1, gameObject.GetComponent<GridLayoutGroup>().cellSize.y);

            m_ListCreated = true;
        }

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

    }

    // Use this for initialization
    void Start () {
        PopulateAddMeritPanel();
		
	}
}
