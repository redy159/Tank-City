using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class HunterA : Tank
{

     void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        shootaudio = gameObject.GetComponent<AudioSource>();
    }

    public int Threat_Detection(RaycastHit2D hit, ref float Clear_Time){
		if (hit.collider != null) {
			if ((hit.rigidbody != null ) && (hit.rigidbody.tag == "bullet")){ 
				float Impact_Time = hit.distance / 7; // Thoi Gian Se Xay Ra Va Cham Khoang Cach CHia cho 10 la Van Toc Vien Dan
				 Clear_Time = (hit.distance + 1) / 7; // Thoi Gian Vien dan bay qua , 10 la Van Toc Vien Dan
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
				code |= 1;
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
    void Update()
    {
        player = Player.curNode;
        
        RayShoot();
        Dodge();

        Move();
        this.facing();
        this.turnDirection();
        if (nextNode.obstacle == 1)
            Shoot();
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Destroy(gameObject);
        }
    }*/



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Node")
            currentNode = collision.gameObject.GetComponent<Node>();

    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        if ((collision.tag == "Node") && (Math.Abs(collision.bounds.center.y - GetComponent<Collider2D>().bounds.center.y) <= 0.11) && (Math.Abs(collision.bounds.center.x - GetComponent<Collider2D>().bounds.center.x) <= 0.11))
        {
            currentNode = collision.gameObject.GetComponent<Node>();
            nextNode = findNextNode();
        }
    }


    // return heuristic value
    public override Node findMinNode(ref float[] fBase,ref float[] fPlayer,ref List<Node> open)
    {
        float min = 10000;
        Node currentPoint = new Node();
        foreach (Node node in open)
        {
            if (g[int.Parse(node.getGameobj().name)] + calH(node.getGameobj(), Base) < min)
            {
                currentPoint = node;
                min = g[int.Parse(node.getGameobj().name)] + calH(currentPoint.getGameobj(), Base);
            }
            if (g[int.Parse(node.getGameobj().name)] + calH(node.getGameobj(), player) < min)
            {
                currentPoint = node;
                min = g[int.Parse(node.getGameobj().name)] + calH(currentPoint.getGameobj(), player);
            }
        }
        return currentPoint;
    }

    public override float calH(GameObject a, GameObject b)
    {
        if (b.tag == "base")
            return a.GetComponent<Node>().Dist_Base;
        return  base.calH(a,b);//base khong phai Base Nha Chinh Dau Ma Lop Cha
    }
    public override void calF(ref float[] fBase, ref float[] fPlayer, Node q)
    {
        fbase[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calH(Base, q.getGameobj());
        fplayer[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calH(player, q.getGameobj());
    }

    public override void calG(ref float[] g, Node q)
    {
        g[int.Parse(q.getGameobj().name)] = g[int.Parse(currentPoint.getGameobj().name)] + q.obstacle;
    }
    public override void Init()
    {
        g = new float[180];
        fbase = new float[180];
        fplayer = new float[180];

        pre = new Node[180];
        g[int.Parse(currentNode.getGameobj().name)] = 0;
       fbase[int.Parse(currentNode.getGameobj().name)] = calH(currentNode.getGameobj(), Base);
       fplayer[int.Parse(currentNode.getGameobj().name)] = calH(currentNode.getGameobj(), player);
    }

    public override bool CheckName(){
        return ((Base.name == currentPoint.name) || (player.name == currentPoint.name));
    }

    public override Node TraceBack(Node currentPoint)
    {
        if (currentPoint.name == Base.name)
        {
            try
            {
                Node nextMove = new Node();
                int u = int.Parse(Base.name);
                while (true)
                {
                    nextMove = pre[u];
                    if (nextMove.getGameobj().name == currentNode.name)
                    {
                        break;
                    }
                    u = int.Parse(nextMove.getGameobj().name);
                }
                return GameObject.Find(u + "").GetComponent<Node>();
            }
            catch (System.Exception e)
            {
                //currentNode = Randomnode(currentNode); //excute ramdomnode
            }
        }

        if (currentPoint.name == player.name)
        {
            try
            {
                Node nextMove = new Node();
                int u = int.Parse(player.name);
                while (true)
                {
                    nextMove = pre[u];
                    if (nextMove.getGameobj().name == currentNode.name)
                    {
                        break;
                    }
                    u = int.Parse(nextMove.getGameobj().name);
                }
                return GameObject.Find(u + "").GetComponent<Node>();
            }
            catch (System.Exception e)
            {
                //currentNode = Randomnode(currentNode); //excute ramdomnode
            }
        }
        return currentNode;
    }
   
}
