using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Fire : Tank {

	// Use this for initialization
	 public Rigidbody2D rb;

	 void FreeFire(){

		 
	 }
	void TriggerFire(){
		//Tinh Chinh Huong Quay Mat Cua Tank
		dx = (firePosition.position.x - transform.position.x);
		dy = (firePosition.position.y - transform.position.y);
		dx = (dx > dy) ? 1 : 0;
		if (dx > 0) 
			dx *= 1;
		else dx *= -1;
		
		dy = (dx < dy) ? 1 : 0;
		if (dy > 0) 
			dy *= 1;
		else dy *= -1;

		//Ray Cast Theo Huong Quay
		RaycastHit2D hit = Physics2D.Raycast(firePosition.position, new Vector2(dx,dy) );
		float R = 10; // Range
        
		if (hit.collider != null) {
			if ((hit.rigidbody != null ) && (hit.rigidbody.tag == "Player")){ 
                
				float DX = Mathf.Abs(hit.rigidbody.position.x - transform.position.x);
				float DY = Mathf.Abs(hit.rigidbody.position.y - transform.position.y);
				if (((DX <= R) && (dx != 0)) || ((DY <= R) && (dy != 0)) ){ //Kiem tra Muc Tieu Da Trong Tam ban R khong
					Debug.Log("Pew"); //Ban
					Debug.Log(hit.collider.name);
				}
            }
	    }
	}
	// Update is called once per frame
	void Update () {
		
	}
}
