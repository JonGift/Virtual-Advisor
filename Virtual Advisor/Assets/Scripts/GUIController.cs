using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class GUIController : MonoBehaviour
{
    // Grabs all student information after it is entered.
    public VirtualAdvisor advisor;
    string connection;
    string generalCommand;

    IDbConnection dbcon;
    IDbCommand dbcmd;
    IDataReader reader;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.dataPath);
        //connection = "URI=file:" + Application.persistentDataPath + "/" + "Virtual_Advisor_Database.db";  // Default path is: C:\Users\NAME\AppData\LocalLow\ChungusBros\Virtual Advisor
        connection = "URI=file:" + Application.dataPath + "/" + "Virtual_Advisor_Database.db";
        dbcon = new SqliteConnection(connection);
        dbcon.Open();

        dbcmd = dbcon.CreateCommand();

        RunQuery(CreateCompSciDB2("CompSciClasses"));
        RunQuery(CreateTakenTable("TakenClasses"));
        RunQuery(CreateElectiveTable("ElectiveClasses"));
        RunQuery(CreateGeneratedClassTable("GeneratedClasses"));
    }

    void OnApplicationQuit() {
        dbcon.Close();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }

    public void Update() {
        if (generalCommand != "") {
            dbcmd = dbcon.CreateCommand();
            
            generalCommand = "";
        }
    }

    public IDataReader RunQuery(string query) {
        Debug.Log("Received query: " + query);
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();
        Debug.Log("Records affected: " + reader.RecordsAffected);
        return reader;
    }

    string CreateTakenTable(string tableName)
    {
        string takenTable_CreateTable =
            "CREATE TABLE IF NOT EXISTS " + tableName + " (" +
            "Subject" + " TEXT NOT NULL," +
            "Course" + " INTEGER)";

        return takenTable_CreateTable;

    }

    string CreateGeneratedClassTable(string tableName) {
        string GeneratedClassCreateTable =
            "CREATE TABLE IF NOT EXISTS " + tableName + " (" +
            "CRN" + " INTEGER PRIMARY KEY," +
            "Subject" + " TEXT NOT NULL," +
            "Course" + " INTEGER," +
            "Section" + " INTEGER," +
            "Credits" + " INTEGER," +
            "Title" + " TEXT," +
            "PrereqSubject" + " TEXT," +
            "PrereqCourse" + " INTEGER)";

        return GeneratedClassCreateTable;
    }

    // This is Jon's suggestion for the CS table. Each table should be unique to the semester, and should be pulled from the web page.
    string CreateCompSciDB2(string tableName) {
        string compSci_createTable =
            "CREATE TABLE IF NOT EXISTS " + tableName + " (" +
            "CRN" + " INTEGER PRIMARY KEY," +
            "Subject" + " TEXT NOT NULL," +
            "Course" + " INTEGER," +
            "Section" + " INTEGER," +
            "Credits" + " INTEGER," +
            "Title" + " TEXT," +
            "PrereqSubject" + " TEXT," +
            "PrereqCourse" + " INTEGER)";

        return compSci_createTable;
    }

    string CreateCompEngrDB()
    {
        string compEngr_createTable =
            "CREATE TABLE IF NOT EXISTS " + "compSci_table" + " (" +
            "CRN" + "INTERGER PRIMARY KEY," +
            "Semester" + "TEXT NOT NULL, " +
            "Prerequisites" + "TEXT NOT NULL, " +
            "Campus" + "TEXT NOT NULL )";

        return compEngr_createTable;
    }

    string CreateElectiveTable(string tableName)
    {
        string ElectiveTable_CreateTable =
           "CREATE TABLE IF NOT EXISTS " + tableName + " (" +
           "Subject" + " TEXT NOT NULL)";

        return ElectiveTable_CreateTable;

    }


    string InsertIntoTable(string tableName, int crn, string subject, int course, int section, int credits, string title)
    {
        string insertTable =
            "INSERT INTO " + tableName + " VALUES " +
            "(" + crn + ", " +
            subject + ", " +
            course + ", " +
            section + ", " +
            credits + ", " +
            title + ")";

        return insertTable;
    }
    /// <summary>
    /// This function will produce a list of classes that a prospective student might want to take.
    /// </summary>
    string CreateClassListTable() {

        return "todo";
    }
}
