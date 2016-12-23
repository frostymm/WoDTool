/*
 * This class is used to handle excel spreadsheet data files
 * Currently still in progress though
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ExcelLibrary.SpreadSheet;

public class SpreadSheetManager : MonoBehaviour {

   /* private Workbook merits;
    private void LoadSpreadSheet(string name)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Spreadsheets/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Spreadsheets/");

        if (!File.Exists(Application.dataPath + "/Spreadsheets/" + name + ".xls"))
        {
            Debug.Log("File does not exist");

            string fileName = name;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Spreadsheets/" + fileName + ".xls");

            object data = Resources.Load("SpreadSheets/" + name);

            bf.Serialize(file, data);
            file.Close();

            if (GameManager.Instance().isDebugMode)
                Debug.Log("Saving File to: " + Application.persistentDataPath + "/Spreadsheets/" + fileName + ".xls");

        }
       //merits = Workbook.Load()
    }

    private void Start()
    {
        LoadSpreadSheet("Merits");
    }
    */
}
