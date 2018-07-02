using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Eagle : MonoBehaviour
{   
    
    public Sprite damaged;
    public int hp = 2;
    public Node CurrentNode;
    void CheckV(ref Node u,ref Node v,ref Queue Q, ref HashSet<int> Free ){
        //int tmp = v.getName();
    
        if ((v != null) && (!Free.Contains(v.getName()) )) {
			Q.Enqueue(v);
            v.Dist_Base = u.Dist_Base + 1;
            Free.Add(v.getName());
        }
    }

    void Start(){
       
        //BFS(); Loang Ra De Tinh Khoang Cach Cai Tien BullDozer
        CurrentNode.Dist_Base = 0;
        Queue Q = new Queue();
        HashSet<int> Free = new HashSet<int>();
        
        Q.Enqueue(CurrentNode);
	    Free.Add(CurrentNode.getName());
	    

	    while (Q.Count > 0){ // Q k Rong
		    Node u = (Node) Q.Peek(); // U = Phan Tu Dau Tien Cua Q
		    Q.Dequeue();
            Node v = u.TopNode; //V la cac Node ke voi U
            CheckV(ref u,ref v,ref Q,ref Free);
            v = u.BottomNode;
            CheckV(ref u,ref v,ref Q,ref Free);
            v = u.LeftNode;
            CheckV(ref u,ref v,ref Q,ref Free);
            v = u.RightNode;
            CheckV(ref u,ref v,ref Q,ref Free);   
		}
	}
        
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            hp--;
            this.GetComponent<SpriteRenderer>().sprite = damaged;
            if (hp == 0) 
                SceneManager.LoadScene("game over");
        }
        if (collision.gameObject.tag == "bulldozer")
        {
            hp--;
            this.GetComponent<SpriteRenderer>().sprite = damaged;
            if (hp==0)
                SceneManager.LoadScene("game over");
        }
    }
}
