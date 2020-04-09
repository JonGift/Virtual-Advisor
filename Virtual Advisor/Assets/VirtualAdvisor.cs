using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualAdvisor : MonoBehaviour
{
    // UI Elements
    public Dropdown majorDropdown;

    // Virtual Advisor info
    int page = 0;
    string major = "None";


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementPage() {
        page++;
        UpdatePage(page - 1);
    }

    public void DecrementPage() {
        page--;
        UpdatePage(page + 1);
    }

    void UpdatePage(int prev) {
        string prevPage = "Page" + prev;
        string pageName = "Page" + page;

        transform.GetChild(0).FindChild(prevPage).gameObject.SetActive(false);
        transform.GetChild(0).FindChild(pageName).gameObject.SetActive(true);
    }


    public void UpdateMajor() {
        string m = majorDropdown.options[majorDropdown.value].text;
        major = m;
        Debug.Log("Changed major: " + major);
    }
}
