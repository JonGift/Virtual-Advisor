using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectiveCheckBoxes : MonoBehaviour
{
    bool check = false;
    public string subject = "CS";

    GameObject checkObj;
    Text text;

    public void Start()
    {
        checkObj = transform.GetChild(0).gameObject;
        text = transform.GetChild(1).GetComponent<Text>();
        text.text = subject;
    }

    public bool GetCheck()
    {
        return check;
    }

    public string GetSubject()
    {
        return subject;
    }

    public void Clicked()
    {
        check = !check;
        checkObj.SetActive(check);
    }
}
