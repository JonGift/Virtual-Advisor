using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

public class VirtualAdvisor : MonoBehaviour
{
    // UI Elements
    public InputField adminInput;
    public Dropdown majorDropdown;
    public Dropdown semesterDropdown;
    public InputField desiredCreditsInput;
    public GameObject takenClassesObj;
    public GameObject ElectiveClassesObj;

    // Virtual Advisor info
    int page = 0;
    int maxPage = 0;

    string major = "None";
    string semester = "None";
    int desiredCredits = 12;

    GUIController dbcontroller;

    // Start is called before the first frame update
    void Start()
    {
        dbcontroller = GetComponent<GUIController>();
        FindMaxPage();
    }

    /// <summary>
    /// This function will give our GUIController class all of this student's information, how it will be stored is to be decided.
    /// </summary>
    void OutputStudentInfo() {

    }

    void FindMaxPage() {
        string pageFinder = "Page" + maxPage;
        while (transform.GetChild(0).Find(pageFinder) != null) {
            maxPage++;
            pageFinder = "Page" + maxPage;
        }
        maxPage--;
    }

    public void IncrementPage() {
        if (page == maxPage)
            return;
        page++;
        UpdatePage(page - 1);
    }

    public void DecrementPage() {
        if (page == 0)
            return;
        page--;
        UpdatePage(page + 1);
    }

    public void ExecuteQueryFromAdmin() {
        dbcontroller.RunQuery(adminInput.text);
    }

    void UpdatePage(int prev) {
        string prevPage = "Page" + prev;
        string pageName = "Page" + page;

        transform.GetChild(0).Find(prevPage).gameObject.SetActive(false);
        transform.GetChild(0).Find(pageName).gameObject.SetActive(true);

        // Page specific calls below:
        if(prev == 4) {
            UpdateElectives();
        }
        if(prev == 5) {
            UpdateTakenClasses();
        }
    }


    public void UpdateMajor() {
        string m = majorDropdown.options[majorDropdown.value].text;
        major = m;
        Debug.Log("Changed major: " + major);
    }

    public void UpdateSemester()
    {
        string s = semesterDropdown.options[semesterDropdown.value].text;
        semester = s;
        Debug.Log("Updated Semester: " + semester);
    }

    public void UpdateCredits() {
        int credits = int.Parse(desiredCreditsInput.text);
        if (credits > 0 && credits <= 20) {
            desiredCredits = credits;
            Debug.Log("New number of credits: " + desiredCredits);
        }
        else {
            desiredCreditsInput.text = desiredCredits.ToString();
            Debug.Log("Invalid number of credits entered.");
        }
    }

    public void UpdateElectives()
    {
        string query = "DELETE FROM UserChosenElectives";
        dbcontroller.RunQuery(query);
        foreach (CheckboxController checkbox in ElectiveClassesObj.GetComponentsInChildren<CheckboxController>())
        {
            if (checkbox.GetCheck())
            {
                string subject = checkbox.GetSubject();
                int course = checkbox.GetCourse();

                query =
                "INSERT INTO UserChosenElectives VALUES " +
                "('" + subject + "', " +
                course + ")";
                dbcontroller.RunQuery(query);
            }
        }

    }

    public void UpdateTakenClasses() {
        string query = "DELETE FROM TakenClasses";
        dbcontroller.RunQuery(query);
        foreach(CheckboxController checkbox in takenClassesObj.GetComponentsInChildren<CheckboxController>()) {
            if (checkbox.GetCheck()) {
                string subject = checkbox.GetSubject();
                int course = checkbox.GetCourse();

                query =
                "INSERT INTO TakenClasses VALUES " +
                "('" + subject + "', " +
                course + ")";

                dbcontroller.RunQuery(query);
            }
        }
    }

    public void GenerateClasses() {
        string query = "DELETE FROM GeneratedClasses";
        IDataReader reader;
        IDataReader reader2;
        IDataReader reader3;
        IDataReader reader4;
        IDataReader reader5;
        IDataReader reader6;
        IDataReader reader7;
        int takenCredits = 0;

        dbcontroller.RunQuery(query);
        // Query todo: First check if we still have credits > 0 leftover.
        // If so, check the relevant degree classes. (CompSciClasses)
        // Then pull a class from that and check if we have all the preqreqs.
        // Then check if that class would conflict with out current schedule.
        // If we have still passed all of these checks, add it to the GeneratedClasses table.
        // This query is hard, maybe work it out on the whiteboard.

        // Note: Maybe we should just check if it's greater than zero. Say we have 11 credits currently, we want to take 12, but all the available classes are 3 or more credits,
        // We should just add one anyway. We'll end up with 14 credits but that's pretty much unavoidable.
        // The only time we would have less than our desired credits is if we are literally out of classes to take.



        //**************************** Hunter Note: I notice we dont get any classes out when we say we havent taken anything yet. ****************/
        //what do we do if there is not a pre req
        //Maybe if its somebody brand new let them choose electives but then everything else is predetermined?

        string majorTable;
        if (major == "Computer Science")
            majorTable = "CompSciClasses";

        query = "SELECT * FROM CompSciRequiredClasses EXCEPT SELECT * FROM TakenClasses";
        reader = dbcontroller.RunQuery(query);

        while (reader.Read() && (takenCredits < desiredCredits)) {
            string subject = reader.GetValue(0).ToString();
            int course = reader.GetInt32(1);

            query = "SELECT PrereqSubject, PrereqCourse FROM CompSciClasses WHERE CompSciClasses.Subject = '" + subject + "' AND CompSciClasses.Course = " + course;

            reader2 = dbcontroller.RunQuery(query);
            while (reader2.Read()) {
                Debug.Log("CompSci Prereqs exist");
                string prereqSubject = reader2.GetValue(0).ToString();
                int prereqCourse = reader2.GetInt32(1);
                query = "SELECT * FROM TakenClasses WHERE TakenClasses.Subject = '" + prereqSubject + "' AND TakenClasses.Course = " + prereqCourse;
                Debug.Log("About to breakdown loop");
                reader2 = dbcontroller.RunQuery(query);
                while (reader2.Read()) {
                    Debug.Log("We have the prereqs.");
                    query = "INSERT INTO GeneratedClasses SELECT * FROM CompSciClasses WHERE CompSciClasses.Subject = '" + subject + "' AND CompSciClasses.Course = " + course;
                    dbcontroller.RunQuery(query);
                    query = "SELECT Credits FROM CompSciClasses WHERE CompSciClasses.Subject = '" + subject + "' AND CompSciClasses.Course = " + course;
                    reader3 = dbcontroller.RunQuery(query);
                    while (reader3.Read())
                        takenCredits += reader3.GetInt32(0);
                }
            }

            //we are assuming they are starting at Calc 1 which we will say has "no prereq"
            //Also assuming STATS is a part of math classes as DR. LEE says EZ INTEGRAAAAALLLL
            query = "SELECT PrereqSubject, PrereqCourse FROM MathClasses WHERE MathClasses.Subject = '" + subject + "' AND MathClasses.Course = " + course; //this was here before Hunter tried working on Math stuff
            reader4 = dbcontroller.RunQuery(query);
            while (reader4.Read())
            {
                Debug.Log("Math Prereqs exist");
                string prereqSubject = reader4.GetValue(0).ToString();
                int prereqCourse = reader4.GetInt32(1);
                query = "SELECT * FROM TakenClasses WHERE TakenClasses.Subject = '" + prereqSubject + "' AND TakenClasses.Course = " + prereqCourse;
                Debug.Log("About to breakdown loop");
                reader4 = dbcontroller.RunQuery(query);
                while (reader4.Read())
                {
                    Debug.Log("We have the prereqs.");
                    query = "INSERT INTO GeneratedClasses SELECT * FROM MathClasses WHERE MathClasses.Subject = '" + subject + "' AND MathClasses.Course = " + course;
                    dbcontroller.RunQuery(query);
                    query = "SELECT Credits FROM MathClasses WHERE MathClasses.Subject = '" + subject + "' AND MathClasses.Course = " + course;
                    reader5 = dbcontroller.RunQuery(query);
                    while (reader5.Read())
                        takenCredits += reader5.GetInt32(0);
                }
            }

            //This should insert the electives into the schedule i believe
            //This should insert into the generated classes everything about the electives that the user chose
            
            query = "INSERT INTO GeneratedClasses SELECT * FROM ElectiveOptions INNER JOIN ElectiveClasses on ElectiveClasses.Subject = ElectiveOptions.Subject";
            reader6 = dbcontroller.RunQuery(query);
            while(reader6.Read())
            {
                query = "SELECT Credits FROM ElectiveOptions WHERE ElectiveOptions.Subject = '" + subject + "' AND ElectiveOptions.Course = " + course;
                reader7 = dbcontroller.RunQuery(query);
                while (reader7.Read())
                    takenCredits += reader7.GetInt32(0);
            }
            
        }
        // Do stuff
    }
  
}
