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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        connection = "URI=file:" + Application.persistentDataPath + "/" + "Virtual_Advisor_Database";  
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand dbcmd;
        IDataReader reader;

        dbcmd = dbcon.CreateCommand();

        string q_createTable = CreateCompSciDB();

        dbcmd.CommandText = q_createTable;

        reader = dbcmd.ExecuteReader();
    }

    //This function im trying to create a table for the Computer science courses
    //I may need to do this inside of start
    string CreateCompSciDB()
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

    /// <summary>
    /// This function will produce a list of classes that a prospective student might want to take.
    /// </summary>
    string CreateClassListTable() {

        return "todo";
    }
}
