using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTank : MonoBehaviour {

    private float dy = 0.0f;
    private GameObject[] choices;
    private int index = 0;
    private bool active = false;

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
        dy = Input.GetAxisRaw("Vertical");
        if (!active)
        {
            if (dy != 0) active = true;
            if (dy > 0)
            {


                choices[index].SetActive(false);
                index--;
                if (index < 0)
                    index = 2;
                choices[index].SetActive(true);

            }
            else
            if (dy < 0)
            {
                choices[index].SetActive(false);
                index++;
                if (index > 2)
                    index = 0;
                choices[index].SetActive(true);
            }
            
        }
        else if (dy==0) active = false;
        if (index == 0 && Input.GetKeyDown("space"))
            Application.LoadLevel("Stage1");
        if (index == 1 && Input.GetKeyDown("space"))
            Application.LoadLevel("Help");
        if (index == 2 && Input.GetKeyDown("space"))
            Application.LoadLevel("About");
    }
}
