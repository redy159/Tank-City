using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseControl : MonoBehaviour {

    public Image winImg;
    public Image loseImg;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setWin()
    {
        winImg.enabled = true;
        StartCoroutine("Wait");
    }

    public void setLose()
    {
        loseImg.enabled = true;
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);

        Application.LoadLevel("Menu_Demo");
    }
}
