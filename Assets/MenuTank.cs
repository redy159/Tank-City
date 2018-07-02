using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTank : MonoBehaviour {

    public Vector2 position;
    Vector2 nextNode;
    protected Node currentNode;
    public Rigidbody2D rb;
    
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if(Input.GetKeyDown("down"))
        {
            rb.velocity = new Vector2(0, -1);
        }
        if(Input.GetKeyDown("up"))
        {
            rb.velocity = new Vector2(0, 1);
        }
    }
}
