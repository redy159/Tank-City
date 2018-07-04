using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank {

    // Use this for initialization
    //void Start () {
    //	rb = gameObject.GetComponent<Rigidbody2D>()	
    //}

    // Update is called once per frame

    static public GameObject curNode;

    void Update()
    {   //Input Tu Nguoi Choi
        dx = Input.GetAxisRaw("Horizontal");
        dy = dx == 0.0f ? Input.GetAxisRaw("Vertical") : 0.0f;


        this.turnDirection();

        rb.velocity = new Vector2(dx, dy) * speed;

        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Node")
        {
            curNode = collision.gameObject;
            currentNode = collision.gameObject.GetComponent<Node>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "base")
        {
            Destroy(gameObject);
        }
    }

}
