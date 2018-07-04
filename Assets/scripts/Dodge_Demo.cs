using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Demo : Tank {

	// Use this for initialization

	// Update is called once per frame

	 // De Tham Khao Toc Do Bay Cua Dan
	public int Threat_Detection(RaycastHit2D hit, ref float Clear_Time){
		if (hit.collider != null) {
			if ((hit.rigidbody != null ) && (hit.rigidbody.tag == "bullet")){ 
				Debug.Log(hit.rigidbody.name);
				float Impact_Time = hit.distance / 10; // Thoi Gian Se Xay Ra Va Cham Khoang Cach CHia cho 10 la Van Toc Vien Dan
				 Clear_Time = (hit.distance + 1) / 10; // Thoi Gian Vien dan bay qua , 10 la Van Toc Vien Dan
				float Evade_Time =(float)(1.2) / this.speed; // Thoi Gian Ne, 1 khoang cach de di chuyen qua vung co dan va 0.2 khoang cach tru hao de dua ra quyet dinh la di dau
				if (Impact_Time > Evade_Time ){
					return 1;
				}
				else return 2;
			}
		}
		return 0;
	}
	
	bool Reachable(Node u){
		if (u == null || u.obstacle == 9999){
			return false;
		}
		return true;
	}
	IEnumerator Wait(float Time) { 
		yield return new WaitForSeconds(Time);
	}
	public void Evade(int C,float Clear_Time){
		// Them Ne
		if (C % 4 == 0){
			//Dan Bay Toi Theo Phuong Ngang Nen Se Ne Len Tren Hoac Xuong
			Node u = this.currentNode.TopNode;
			if (Reachable(u)){
				nextNode = u;
			}
			else {
				u = this.currentNode.BottomNode;
				if (Reachable(u)){
					nextNode = u;
				}
				
			}
			//Wait(Clear_Time);
			Debug.Log("Up Or Down");
		}
		else {
			// Dan Bay Toi Theo Phuong Doc Nen Se Ne Trai Hoac Phai
			Node u = this.currentNode.LeftNode;
			if (Reachable(u)){
				nextNode = u;
			}
			else {
				u = this.currentNode.RightNode;
				if (Reachable(u)){
					nextNode = u;
				}
				
			}
        }
		//Doi Mot Khoang Thoi Gian Clear_Time
	}
    private void Move()
    {
        float step = speed * Time.deltaTime;
        if (nextNode != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextNode.getGameobj().transform.position, step);

            float sx = (nextNode.getGameobj().transform.position.x - currentNode.position.x);
            float sy = (nextNode.getGameobj().transform.position.y - currentNode.position.y);

            if (!(sx == 0 && sy == 0))
            {
                dx = (Mathf.Abs(sx) >= Mathf.Abs(sy)) ? 1 : 0;
                dy = 1 - (dx * 1);//(x < y) ? 1 : 0;

                if (sx > 0)
                    dx *= 1;
                else dx *= -1;

                if (sy > 0)
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
    }
    
	void Dodge(){
		RaycastHit2D R = Physics2D.Raycast(transform.position, Vector2.right ); 
		RaycastHit2D L = Physics2D.Raycast(transform.position, Vector2.left );
		RaycastHit2D D = Physics2D.Raycast(transform.position, Vector2.down );
		RaycastHit2D U = Physics2D.Raycast(transform.position, Vector2.up );

		int code = 0; //Bat Chuoc Cohen – Sutherland bitmask
		bool Died = false;
		float tmp = -1;
		float Clear_Time = 0; 
		switch (Threat_Detection(R,ref tmp)){
			case 1:{
				code |= 8;
				if (Clear_Time < tmp)
					Clear_Time = tmp;
				break;
			}
			case 2:{
				Died = true;
				break;
			}
			default:
				break;
      	}
		switch (Threat_Detection(L,ref tmp)){
			case 1:{
				code |= 4;
				if (Clear_Time < tmp)
					Clear_Time = tmp;
				break;
			}
			case 2:
				Died = true;
				break;
      	}
		switch (Threat_Detection(D,ref tmp)){
			case 1:{
				code |= 2;
				if (Clear_Time < tmp)
					Clear_Time = tmp;
				break;
			}
			case 2:
				Died = true;
				break;

      	}
		switch (Threat_Detection(U,ref tmp)){
			case 1:{
				code |= 8;
				if (Clear_Time < tmp)
					Clear_Time = tmp;
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
				Evade(code,Clear_Time); // Ne
			}
			else 
				Debug.Log("Clear"); // An Toan
		} 
		else {
			//Chet Roi Khoi Ne
			Debug.Log("Lol No Need To Dodge Cause U Will Be Died Anyway");
		}
        Move();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentNode = collision.gameObject.GetComponent<Node>();
    }

    void Update() {
		Dodge();
		//Contiue Wev Moving We Are
		

        
	}


}
