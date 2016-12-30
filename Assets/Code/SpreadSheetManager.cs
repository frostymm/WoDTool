/*
 * This class is used to handle excel spreadsheet data files
 * it uses excelLibrary public api and it supports .xls files
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ExcelLibrary.SpreadSheet;

public class SpreadSheetManager{

    //Singleton initialization
    private static SpreadSheetManager m_Instance;
    public static SpreadSheetManager Instance()
    {
        if (m_Instance == null)
            m_Instance = new SpreadSheetManager();
        return m_Instance;
    }

    /*Only one sheet is being loaded currently and only once per scene. Disabled saving reference for now to reduce unnecessary memory usage*/
    //private Dictionary<string, Workbook> m_LoadedSpreadSheets = new Dictionary<string, Workbook>();
    private Workbook LoadSpreadSheet(string fileName)
    {
        //if (m_LoadedSpreadSheets.ContainsKey(fileName))
        //    return m_LoadedSpreadSheets[fileName];

        Workbook loadedSpreadSheet;
        loadedSpreadSheet = Workbook.Load(Application.streamingAssetsPath + "/Spreadsheets/" + fileName + ".xls");

        //m_LoadedSpreadSheets.Add(fileName, loadedSpreadSheet);
        return loadedSpreadSheet;
    }
    
    //Load spreadsheet cell data
    public Dictionary<string, List<SpreadSheetMerit>> LoadMeritList(string fileName)
    {
        Workbook loadedSpreadSheet = LoadSpreadSheet(fileName);
        CellCollection m_cells = loadedSpreadSheet.Worksheets[0].Cells;

        Dictionary<string, List<SpreadSheetMerit>> meritList = new Dictionary<string, List<SpreadSheetMerit>>();
        
        //Ignore first row because it is just labels for each column
        for(int rowindex = m_cells.FirstRowIndex + 1; rowindex <= m_cells.LastRowIndex; rowindex++)
        {
            SpreadSheetMerit merit = new SpreadSheetMerit(); //Merit object to populate with data

            Row m_row = m_cells.GetRow(rowindex);
            for(int colindex = m_row.FirstColIndex; colindex <= m_row.LastColIndex; colindex++)
            {
                Cell cell = m_row.GetCell(colindex);

                merit.PopulateData(colindex, cell.StringValue);
            }

            if (!meritList.ContainsKey(merit.RetrieveData(SpreadSheetMerit.MeritData.category)))
                meritList.Add(merit.RetrieveData(SpreadSheetMerit.MeritData.category), new List<SpreadSheetMerit>());

            meritList[merit.RetrieveData(SpreadSheetMerit.MeritData.category)].Add(merit);
        }

        return meritList;
    }
}

//Representation of a single Merits data from the spread sheet
public class SpreadSheetMerit
{
    private List<string> m_meritData;

    public enum MeritData
    {
        name,
        value,
        prerequisites,
        description,
        category
    }

    public string GetName() { return m_meritData[(int)MeritData.name]; }

    public int GetValueAsInt()
    {
        return m_meritData[(int)MeritData.value].Length;
    }

    public string RetrieveData(int meritData)
    {
        if (m_meritData.Capacity > meritData)
            return m_meritData[meritData];
        else
            return "";
    }

    public string RetrieveData(MeritData meritData) { return RetrieveData((int)meritData); }

    public void PopulateData(int i, string data)
    {
        if (m_meritData == null)
            m_meritData = new List<string>(5) { "", "", "", "", ""};

        m_meritData[i] = data;
    }
}
