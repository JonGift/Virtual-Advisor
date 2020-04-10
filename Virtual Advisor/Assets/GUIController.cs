using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class GUIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";  
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand dbcmd;
        IDataReader reader;

        dbcmd = dbcon.CreateCommand();
        string q_createTable =
          "CREATE TABLE IF NOT EXISTS " + "my_table" + " (" +
          "id" + " INTEGER PRIMARY KEY, " +
          "val" + " INTEGER )";

        dbcmd.CommandText = q_createTable;

        string compSciDB = createCompSciDB();                     //attempting to understand whats going on
        dbcmd.CommandText = compSciDB;

        reader = dbcmd.ExecuteReader();
    }

    //This function im trying to create a table for the Computer science courses
    //I may need to do this inside of start
    string createCompSciDB()
    {
        //IDbCommand dbcmd2;  //Im not sure what this is but its like a variable definition of a database command?

        //dbcmd2 = dbcon.CreateCommand(); //create the command 
        string compSci_createTable =
            "CREATE TABLE IF NOT EXISTS " + "compSci_table" + " (" +
            "CRN" + "INTERGER PRIMARY KEY," +
            "Semester" + "TEXT NOT NULL, " +
            "Prerequisites" + "TEXT NOT NULL, " +
            "Campus" + "TEXT NOT NULL )";

        //dbcmd2.CommandText = compSci_createTable;  //actually create the table??
        //big chungus
        return compSci_createTable;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
