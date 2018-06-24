using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank {

	// Use this for initialization
	//void Start () {
	//	rb = gameObject.GetComponent<Rigidbody2D>()	
	//}
	
	// Update is called once per frame

	
	void Update () {
		dx = Input.GetAxisRaw("Horizontal");
        dy = dx == 0.0f ? Input.GetAxisRaw("Vertical") : 0.0f;
        

        this.FacingDirection();

        rb.velocity = new Vector2(dx, dy) * speed;

        if (Input.GetButtonDown("Fire1"))
            Shoot();
	}
}
