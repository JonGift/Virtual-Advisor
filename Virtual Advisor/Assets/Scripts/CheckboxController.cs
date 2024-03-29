﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CheckboxController : MonoBehaviour
{
    bool check = false;
    public string visibleSubject = "Computer Science";
    public string subject = "CS";
    public int course = 120;

    GameObject checkObj;
    Text text;

    public void Start() {
        checkObj = transform.GetChild(0).gameObject;
        text = transform.GetChild(1).GetComponent<Text>();
        if (course != 0)
            text.text = visibleSubject + " " + course;
        else
            text.text = visibleSubject;
    }

    public bool GetCheck() {
        return check;
    }

    public string GetVisibleSubject() {
        return visibleSubject;
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
