using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTank : MonoBehaviour {

    private GameObject[] choices;
    private int index = 0;
    private void Start()
    {
        choices = new GameObject[3];
        for(int i = 0; i < 3; i++)
        {
            choices[i] = transform.GetChild(i).gameObject;
        }
        foreach (GameObject go in choices)
            go.SetActive(false);
        if (choices[0])
            choices[0].SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            choices[index].SetActive(false);
            index--;
            if (index < 0)
                index = 2;
            choices[index].SetActive(true);
        }

        if (Input.GetKeyDown("down"))
        {
            choices[index].SetActive(false);
            index++;
            if (index > 2)
                index = 0;
            choices[index].SetActive(true);
        }

        if (index == 0 && Input.GetKeyDown("space"))
            Application.LoadLevel("test");
        if (index == 1 && Input.GetKeyDown("space"))
            Application.LoadLevel("Help");
        if (index == 2 && Input.GetKeyDown("space"))
            Application.LoadLevel("About");
    }
}
