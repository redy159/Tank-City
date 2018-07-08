using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	} 
     
    public void Startgame(string stage)
    {
        if (stage == "Chose_Level" || stage == "Menu_Demo")
        {

        }
        else
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
            foreach (GameObject obj in objs)
            {
                GameObject.Destroy(obj);
            }
        }
        Application.LoadLevel(stage);
    }

}
