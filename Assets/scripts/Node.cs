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
    public int obstacle =0;
    public int weight; // Trong So
    public int[] g = new int [2]; // Ham G voi g[0] la Player va G[1] la Base
    public int f; // ham F
    public int[] h = new int [4]; // Ham H Tuong DUong Voi Tuong Loai 

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

