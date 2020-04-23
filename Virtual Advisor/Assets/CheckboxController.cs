using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxController : MonoBehaviour
{
    bool check = false;
    public string subject = "CS";
    public int course = 120;

    GameObject checkObj;
    Text text;

    public void Start() {
        checkObj = transform.GetChild(0).gameObject;
        text = transform.GetChild(1).GetComponent<Text>();
        text.text = subject + " " + course;
    }

    public bool GetCheck() {
        return check;
    }

    public string GetSubject() {
        return subject;
    }

    public int GetCourse() {
        return course;
    }

    public void Clicked() {
        check = !check;
        checkObj.SetActive(check);
    }
}
