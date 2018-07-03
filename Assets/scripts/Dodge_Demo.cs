﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Demo : Tank {

	// Use this for initialization

	// Update is called once per frame

	 // De Tham Khao Toc Do Bay Cua Dan
	public int Threat_Detection(RaycastHit2D hit){
		if (hit.collider != null) {
			if ((hit.rigidbody != null ) && (hit.rigidbody.tag == "bullet")){ 
				Debug.Log(hit.rigidbody.name);
				float Impact_Time = hit.distance / hit.collider.gameObject.GetComponent<Bullet>().Speed; // Thoi Gian Se Xay Ra Va Cham Khoang Cach CHia cho 10 la Van Toc Vien Dan
				float Clear_Time = (hit.distance + 1) / 10; // Thoi Gian Vien dan bay qua , 10 la Van Toc Vien Dan
				float Evade_Time =(float)(1.2) / this.speed; // Thoi Gian Ne, 1 giay de duy chuyen qua vung co dan va 0.2 giay de dua ra quyet dinh la di dau
				if (Impact_Time > Evade_Time ){
					return 1;
				}
				else return 2;
			}
		}
		return 0;
	}

    void Move()//di chuyen toi next node
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, nextNode.getGameobj().transform.position, step);

        float x = (nextNode.getGameobj().transform.position.x - currentNode.position.x);
        float y = (nextNode.getGameobj().transform.position.y - currentNode.position.y);

        if (!(x == 0 && y == 0))
        {
            dx = (Mathf.Abs(x) >= Mathf.Abs(y)) ? 1 : 0;
            dy = 1 - (dx * 1);//(x < y) ? 1 : 0;

            if (x > 0)
                dx *= 1;
            else dx *= -1;

            if (y > 0)
                dy *= 1;
            else dy *= -1;
        }
        //Quay Mat
        if (dx != 0)
        {
            if (dx == 1)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else if (dy != 0)
        {
            if (dy == 1)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }
	
	
	public void Evade(int C){
		// Them Ne
		if (C % 4 == 0){
            //Dan Bay Toi Theo Phuong Ngang Nen Se Ne Len Tren Hoac Xuong
            if ((currentNode.TopNode != null) && (currentNode.TopNode.obstacle != 9999))
                nextNode = currentNode.TopNode;
            if ((currentNode.BottomNode != null) && (currentNode.BottomNode.obstacle != 9999))
                nextNode = currentNode.BottomNode;
        }
		else {
            if ((currentNode.LeftNode != null) && (currentNode.LeftNode.obstacle != 9999))
                nextNode = currentNode.LeftNode;
            if ((currentNode.RightNode != null) && (currentNode.RightNode.obstacle != 9999))
                nextNode = currentNode.RightNode;
        }
					//Doi Mot Khoang Thoi Gian Clear_Time
	}
	public void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
		//Khoi Tao
		
		
    }
	void Update() {
		RaycastHit2D R = Physics2D.Raycast(transform.position, Vector2.right ); 
		RaycastHit2D L = Physics2D.Raycast(transform.position, Vector2.left );
		RaycastHit2D D = Physics2D.Raycast(transform.position, Vector2.down );
		RaycastHit2D U = Physics2D.Raycast(transform.position, Vector2.up );

		int code = 0; //Bat Chuoc Cohen – Sutherland bitmask
		bool Died = false;
		switch (Threat_Detection(R)){
			case 1:{
				code |= 8;
				break;
			}
			case 2:{
				Died = true;
				break;
			}
			default:
				break;
      	}
		switch (Threat_Detection(L)){
			case 1:
				code |= 4;
				break;
			case 2:
				Died = true;
				break;
      	}
		 switch (Threat_Detection(D)){
			case 1:
				code |= 2;
				break;
			case 2:
				Died = true;
				break;

      	}
		  switch (Threat_Detection(U)){
			case 1:{
				code |= 1;
				break;
			}
			case 2:{
				Died = true;
				break;
			}
			default:
				break;
	
      	} 
		Debug.ClearDeveloperConsole();
		if (!Died){
			if (code != 0){
				Evade(code); // Ne
			}
			else 
				Debug.Log("Clear"); // An Toan
		} 
		else {
			//Chet Roi Khoi Ne
			Debug.Log("Lol No Need To Do Shit Cause U Will Be Died Anyway");
		}
		
	}
}
