using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualAdvisor : MonoBehaviour
{
    // UI Elements
    public InputField adminInput;
    public Dropdown majorDropdown;
    public Dropdown semesterDropdown;
    public InputField desiredCreditsInput;
    public Dropdown electiveDropdown;   //Get the drop down button on page 2
    public GameObject takenClassesObj;
    // Virtual Advisor info
    int page = 0;
    int maxPage = 0;

    string major = "None";
    string semester = "None";
    int desiredCredits = 12;

    GUIController dbcontroller;

    Text myText;   //use to output users elective choices

    public List<string> electives = new List<string>();   //Use to store more than one elective choice  

    // Start is called before the first frame update
    void Start()
    {
        dbcontroller = GetComponent<GUIController>();
        FindMaxPage();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        dbcontroller.runQuery(adminInput.text);
    }

    void UpdatePage(int prev) {
        string prevPage = "Page" + prev;
        string pageName = "Page" + page;

        transform.GetChild(0).Find(prevPage).gameObject.SetActive(false);
        transform.GetChild(0).Find(pageName).gameObject.SetActive(true);

        // Page specific calls below:
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

    //Update the users electives that they have selected
    public void UpdateElectives()
    {

        //We need to set a maximum number of electives the user can choose
        int maxElect = 3;
        string overallElectives = string.Empty;   //string used to store all elective choices to later print out

        myText = GameObject.Find("OutputText").GetComponent<Text>();

        string e1 = electiveDropdown.options[electiveDropdown.value].text;
        electives.Add(e1);           //Add electives to the list  

        overallElectives = string.Join(" ", electives);

        myText.text = overallElectives;

        for ( int x = 0; x < electiveDropdown.options.Count; x++)
        {
            if(electiveDropdown.options[x].text == e1)
            {
                electiveDropdown.options.RemoveAt(x);   //If selected remove from list 
                break;
            }
        }
        foreach (string elective in electives)
        {
            Debug.Log(elective);   //print out to make sure its working 
        }

    }

    public void UpdateTakenClasses() {
        string query = "DELETE FROM BigChungusTaken";
        dbcontroller.runQuery(query);
        foreach(CheckboxController checkbox in takenClassesObj.GetComponentsInChildren<CheckboxController>()) {
            if (checkbox.GetCheck()) {
                string subject = checkbox.GetSubject();
                int course = checkbox.GetCourse();
                // At this point we have the subject and the course. We can now insert into the taken courses table with these two values.

                query =
                "INSERT INTO BigChungusTaken VALUES " +
                "('" + subject + "', " +
                course + ")";

                dbcontroller.runQuery(query);
            }
            //need to reset table at this point
        }
    }
  
}
