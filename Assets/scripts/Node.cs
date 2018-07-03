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
    public int obstacle = 0;
    /*public int weight; // Trong So
    public int[] g = new int [2]; // Ham G voi g[0] la Player va G[1] la Base
    public int f; // ham F
    public int[] h = new int [4]; // Ham H Tuong DUong Voi Tuong Loai */
    public int Dist_Base = -1; // 1 << 31 - 1 ;// Khoang Cach Den Eagle(Base), Duoc Khoi Tao Voi Gia Tri Vo Cuc


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
    public int getName(){
        return int.Parse(this.name);
    }



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "steel")
            obstacle = 9999;
        if (other.tag == "brick")
            obstacle = 9;
    }
}

