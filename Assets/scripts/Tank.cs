using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Tank : MonoBehaviour
{
    public Node startNode;
    public Transform firePosition;
    public float fireRate = 0.5f;
    protected float timeFire = 0.0f;
    public GameObject bullet;


    public GameObject Base;
    public GameObject player;

    public Node currentNode, nextNode;
    protected Node[] pre;
    protected float[] g, fbase, fplayer;
    protected Node currentPoint;

    public const int row = 18;
    public const int col = 10;
    public float speed = 15.0f;
    static int Win_Condition = 3;


    protected Rigidbody2D rb;
    protected float dx = 0.0f, dy = 0.0f;
    protected GameObject projectile;

    //chạy cuối update class con bắn
    //private void checkBrick()
    //{
    //    if (nextNode.obstacle == 1)
    //        Shoot();
    //} 

    protected void Move()
    {
        try
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextNode.getGameobj().transform.position, step);
            facing();
            turnDirection();

        }
        catch (Exception e)
        {
            nextNode = findNextNode();
        }

    }

    protected void facing()
    {
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
    }

    protected void turnDirection()
    {
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
        // Debug.Log(gameObject.name + " go from " + CurrentNode.name + " to " + nexnode.getGameobj().name);
    }

    protected void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentNode = startNode;
    }

    protected void RayShoot()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //dx = 1;
        //dy = 0;

        float x = (firePosition.position.x - transform.position.x);
        float y = (firePosition.position.y - transform.position.y);
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

        //Ray Cast Theo Huong Quay
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dx, dy));
        //RaycastHit2D hit = Physics2D.Raycast(firePosition.position, new Vector2(1,0) );
        float R = 10;
        Debug.Log(hit.centroid);
        

        //Debug.Log(hit.rigidbody.name);
        if (hit.collider != null)
        {
            if (this.tag == "hunter"){
                if ((hit.collider.tag == "base")){
                    this.Shoot();
                }
            }
            if ((hit.collider.tag == "Player"))
            {
                float DX = Mathf.Abs(hit.rigidbody.position.x - transform.position.x);
                float DY = Mathf.Abs(hit.rigidbody.position.y - transform.position.y);
                if (((DX <= R) && (dx != 0)) || ((DY <= R) && (dy != 0)))
                //Kiem tra Muc Tieu Da Trong Tam ban R khong
                {
                    this.Shoot();
                }

            }
        }
    }


    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Win_Condition --;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "player"){
            collision.gameObject.GetComponent<Player>().hp--;
            Win_Condition --;
            Destroy(gameObject);
            if (collision.gameObject.GetComponent<Player>().hp  == 0){
                SceneManager.LoadScene("gameover");
            }
        }    
        if (collision.gameObject.tag == "base")
        {
            Win_Condition --;
            Destroy(gameObject);
        }
        if (Win_Condition == 0){
            SceneManager.LoadScene("game over");//Win roi Load Scene tiep theo
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        currentNode = collision.gameObject.GetComponent<Node>();
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {

    }


    // return heuristic value
    public virtual float calH(GameObject a, GameObject b)
    {
        float xa = a.transform.position.x;
        float ya = a.transform.position.y;
        float xb = b.transform.position.x;
        float yb = b.transform.position.y;

        // Return using the distance formula
        return Mathf.Sqrt((xa - xb) * (xa - xb) + (ya - yb) * (ya - yb));
    }


    public virtual Node findMinNode(ref float[] fBase, ref float[] fPlayer, ref List<Node> open)
    {
        Debug.Log("Called Min");
        Node tmp = new Node();
        return tmp;
    }

    public virtual void calF(ref float[] fBase, ref float[] fPlayer, Node q) { }

    public virtual void calG(ref float[] g, Node q) { }
    public virtual void Init()
    {
        g = new float[180];
        fbase = new float[180];
        fplayer = new float[180];

        pre = new Node[180];
        g[int.Parse(currentNode.getGameobj().name)] = 0;
        fbase[int.Parse(currentNode.getGameobj().name)] = calH(currentNode.getGameobj(), Base);
        fplayer[int.Parse(currentNode.getGameobj().name)] = calH(currentNode.getGameobj(), player);
    }

    public virtual bool CheckName() {
        return true;
    }

    public virtual Node TraceBack(Node currentPoint)
    {
        return new Node();
    }

    // find all acceptable next point with standind point
    protected Node findNextNode() //create node findNextNode
    {

        Init();

        // xác định vị trí ng chơi
        //target = playerControl.getCurentNode();

        List<Node> open = new List<Node>();
        List<Node> close = new List<Node>();
        open.Add(currentNode);
        while (open.Count != 0)
        {
            currentPoint = findMinNode(ref fbase, ref fplayer, ref open);


            //find the first current Node as minnimun in open

            //if (target.transform.position.x == currentPoint.getGameobj().transform.position.x || target.transform.position.y == currentPoint.getGameobj().transform.position.y)//chay toi eage
            if (CheckName())
            {
                break;
            }

            open.Remove(currentPoint);
            close.Add(currentPoint);

            List<Node> nextPoint = new List<Node>();

            #region thêm vào list các đỉnh quanh currentPoint
            if (currentPoint.TopNode != null && currentPoint.TopNode.obstacle != 9999)
            {
                nextPoint.Add(currentPoint.TopNode);
            }
            if (currentPoint.BottomNode != null  && currentPoint.BottomNode.obstacle != 9999)
            {
                nextPoint.Add(currentPoint.BottomNode);
            }
            if (currentPoint.LeftNode != null  && currentPoint.LeftNode.obstacle != 9999)
            {
                nextPoint.Add(currentPoint.LeftNode);
            }
            if (currentPoint.RightNode != null  && currentPoint.RightNode.obstacle != 9999)
            {
                nextPoint.Add(currentPoint.RightNode);
            }
            #endregion

            foreach (Node q in nextPoint)
            {
                //Node q isnot in close
                /*if (close.IndexOf(q) != -1)
                {
                    if (g[int.Parse(q.getGameobj().name)] > (g[int.Parse(currentPoint.getGameobj().name)] + calH(currentPoint.getGameobj(), q.getGameobj())))
                    {
                        close.Remove(q);
                        open.Add(q);
                    }
                }
                else
                {
                    //Node q is in open
                    if (open.IndexOf(q) == -1)
                    {
                        calG(ref g, q);
                        calF(ref fbase, ref fplayer, q);
                        pre[int.Parse(q.getGameobj().name)] = currentPoint;
                        open.Add(q);
                    }
                    //Node q isnot in close
                    if (open.IndexOf(q) != -1)
                    {
                        if (g[int.Parse(q.getGameobj().name)] > g[int.Parse(currentPoint.getGameobj().name)] + calH(currentPoint.getGameobj(), q.getGameobj()))
                        {
                            calG(ref g, q);
                            calF(ref fbase, ref fplayer, q);
                            pre[int.Parse(q.getGameobj().name)] = currentPoint;
                        }
                    }
                }*/
                if (close.IndexOf(q) == -1 && open.IndexOf(q) == -1)
                {
                    calG(ref g, q);
                    calF(ref fbase, ref fplayer, q);
                    pre[int.Parse(q.getGameobj().name)] = currentPoint;
                    open.Add(q);
                }

                //Node q is in open
                if (open.IndexOf(q) != -1)
                    if (g[q.getName()] > g[currentPoint.getName()] + calH(currentPoint.getGameobj(), q.getGameobj()))
                    {
                        calG(ref g, q);
                        calF(ref fbase, ref fplayer, q);
                        pre[q.getName()] = currentPoint;
                    }
                //Node q is in close
                if (close.IndexOf(q) == -1)
                {
                    if (g[q.getName()] > g[currentPoint.getName()] + calH(currentPoint.getGameobj(), q.getGameobj()))
                    {
                        close.Remove(q);
                        open.Add(q);
                        calG(ref g, q);
                        calF(ref fbase, ref fplayer, q);
                        pre[q.getName()] = currentPoint;
                    }
                }
            }

        }
        open.Clear();
        close.Clear();
        return TraceBack(currentPoint);

    }

    protected void Shoot()
    {
        if (Time.time > fireRate + timeFire)
        {
            Vector2 bulletPos = transform.position;
            GameObject a = Instantiate(bullet);
            a.SetActive(false);
            a.transform.position = firePosition.position;
            a.transform.rotation = transform.rotation;
            a.SetActive(true);
            timeFire = Time.time;
        }
    }
}