using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Demo : Tank {

	// Use this for initialization

	// Update is called once per frame
	
	 // De Tham Khao Toc Do Bay Cua Dan
	public bool Threat_Detection(RaycastHit2D hit){
		if (hit.collider != null) {
			if ((hit.rigidbody != null ) && (hit.rigidbody.tag == "bullet")){ 
				Bullet Ref = gameObject.GetComponent<Bullet>();
				float Impact_Time = hit.distance / Ref.Speed; // Thoi Gian Se Xay Ra Va Cham
				float Clear_Time = (hit.distance + 1) / Ref.Speed; // Thoi Gian Vien dan bay qua 
				float Evade_Time =(float)(1.5) / this.speed; // Thoi Gian Ne
				if (Impact_Time > Evade_Time ){
					return true;
				}
			}
		}
		return false;
	}
	
	
	public void Evade(int C,float){
		Debug.Log ("Ne Dan Bay Ra Tu Vi Tri ");// Them Ne
					//Doi Mot Khoang Thoi Gian Clear_Time
	}
	public void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
		//Khoi Tao
		RaycastHit2D R = Physics2D.Raycast(transform.position, Vector2.right );
		RaycastHit2D L = Physics2D.Raycast(transform.position, Vector2.left );
		RaycastHit2D D = Physics2D.Raycast(transform.position, Vector2.down );
		RaycastHit2D U = Physics2D.Raycast(transform.position, Vector2.up );

		int code = 0;
		if (Threat_Detection(R)) code |= 8;
		if (Threat_Detection(L)) code |= 4;
		if (Threat_Detection(D))code |= 2;
		if (Threat_Detection(U)) code |= 1;
		
    }
	void Update() {
		
	}
}
