using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public Node TopNode;
    public Node BottomNode;
    public Node LeftNode;
    public Node RightNode;
    public Vector3 position;

    // Use this for initialization
    void Start()
    {
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject getGameobj()
    {
        return gameObject;
    }
}
