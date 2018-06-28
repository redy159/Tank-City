using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {   
        if (other.tag=="Node")
        other.gameObject.GetComponent<Node>().obstacle = 9999;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Node")
            other.gameObject.GetComponent<Node>().obstacle = 0;
    }
}
