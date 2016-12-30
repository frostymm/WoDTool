/*This is just a simple script for managing the data displayed on the description panel
 * It would have been far more complicated to attempt to organize all the text via code
 * so I made a script that had references to the text objects as well as a function to
 * populate it with information.
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanelScript : MonoBehaviour {

    public Text titleText;
    public Text prereqText;
    public Text descriptionText;
    public void SetInfo(SpreadSheetMerit merit)
    {
        titleText.text = merit.RetrieveData(SpreadSheetMerit.MeritData.name);
        prereqText.text = merit.RetrieveData(SpreadSheetMerit.MeritData.prerequisites);
        descriptionText.text = merit.RetrieveData(SpreadSheetMerit.MeritData.description);
    }
}
