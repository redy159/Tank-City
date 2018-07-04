using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Seeker : Tank
{
    private Node[] pre; //trace cua a*
    //void RayShoot()
    //{
    //    rb = gameObject.GetComponent<Rigidbody2D>();
    //    //dx = 1;
    //    //dy = 0;

    //    float x = (firePosition.position.x - transform.position.x);
    //    float y = (firePosition.position.y - transform.position.y);
    //    if (!(x == 0 && y == 0))
    //    {
    //        dx = (Mathf.Abs(x) >= Mathf.Abs(y)) ? 1 : 0;
    //        dy = 1 - (dx * 1);//(x < y) ? 1 : 0;

    //        if (x > 0)
    //            dx *= 1;
    //        else dx *= -1;

    //        if (y > 0)
    //            dy *= 1;
    //        else dy *= -1;
    //    }

    //    //Ray Cast Theo Huong Quay
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dx, dy));
    //    //RaycastHit2D hit = Physics2D.Raycast(firePosition.position, new Vector2(1,0) );
    //    float R = 10;
    //    Debug.Log(hit.centroid);

    //    //Debug.Log(hit.rigidbody.name);
    //    if (hit.collider != null)
    //    {
    //        Debug.Log(hit.collider.tag);
    //        if ((hit.collider.tag == "Player"))
    //        {
    //            float DX = Mathf.Abs(hit.rigidbody.position.x - transform.position.x);
    //            float DY = Mathf.Abs(hit.rigidbody.position.y - transform.position.y);
    //            if (((DX <= R) && (dx != 0)) || ((DY <= R) && (dy != 0)))
    //            //Kiem tra Muc Tieu Da Trong Tam ban R khong
    //            {
    //                this.Shoot();
    //            }

    //        }
    //    }
    //}

    void Update()
    {
        target = Player.curNode;

        //raycast shoot 
        RayShoot();

        Move();
        
        if (nextNode.obstacle == 1)
            Shoot();
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        currentNode = collision.gameObject.GetComponent<Node>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if ((collision.tag == "Node") && (Math.Abs(collision.bounds.center.y - GetComponent<Collider2D>().bounds.center.y) <= 0.13) && (Math.Abs(collision.bounds.center.x - GetComponent<Collider2D>().bounds.center.x) <= 0.13))
        {

            nextNode = findNextNode();
        }
    }


    // return heuristic value
    private float calculateHValue(GameObject a, GameObject b)
    {
        float xa = a.transform.position.x;
        float ya = a.transform.position.y;
        float xb = b.transform.position.x;
        float yb = b.transform.position.y;

        // Return using the distance formula
        return Mathf.Sqrt((xa - xb) * (xa - xb) + (ya - yb) * (ya - yb));
    }

    // find all acceptable next point with standind point
    //private Node findNextNode() //create node findNextNode
    //{
    //    g = new float[180];
    //    f = new float[180];

    //    pre = new Node[180];
    //    g[int.Parse(currentNode.getGameobj().name)] = 0;
    //    f[int.Parse(currentNode.getGameobj().name)] = calculateHValue(currentNode.getGameobj(), target);

    //    // xác định vị trí ng chơi
    //    //target = playerControl.getCurentNode();

    //    List<Node> open = new List<Node>();
    //    List<Node> close = new List<Node>();
    //    open.Add(currentNode);
    //    while (open.Count != 0)
    //    {
    //        Node currentPoint = new Node();
    //        float min = 100000;

    //        //find the first current Node as minnimun in open
    //        foreach (Node node in open)
    //        {
    //            if (g[int.Parse(node.getGameobj().name)] + calculateHValue(node.getGameobj(), target) < min)
    //            {
    //                currentPoint = node;
    //                min = g[int.Parse(node.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), target);
    //            }
    //        }

    //        //if (target.transform.position.x == currentPoint.getGameobj().transform.position.x || target.transform.position.y == currentPoint.getGameobj().transform.position.y)//chay toi eage
    //        if (target.name == currentPoint.name)
    //        {
    //            break;  //shoot for AI
    //        }

    //        open.Remove(currentPoint);
    //        close.Add(currentPoint);

    //        List<Node> nextPoint = new List<Node>();

    //        #region thêm vào list các đỉnh quanh currentPoint
    //        if (currentPoint.TopNode != null)
    //        {
    //            nextPoint.Add(currentPoint.TopNode);
    //        }
    //        if (currentPoint.BottomNode != null)
    //        {
    //            nextPoint.Add(currentPoint.BottomNode);
    //        }
    //        if (currentPoint.LeftNode != null)
    //        {
    //            nextPoint.Add(currentPoint.LeftNode);
    //        }
    //        if (currentPoint.RightNode != null)
    //        {
    //            nextPoint.Add(currentPoint.RightNode);
    //        }
    //        #endregion

    //        foreach (Node q in nextPoint)
    //        {
    //            //Node q isnot in close
    //            if (close.IndexOf(q) != -1)
    //            {
    //                if (g[int.Parse(q.getGameobj().name)] > g[int.Parse(currentPoint.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
    //                {
    //                    close.Remove(q);
    //                    open.Add(q);
    //                }
    //            }
    //            else
    //            {
    //                //Node q is in open
    //                if (open.IndexOf(q) == -1)
    //                {
    //                    g[int.Parse(q.getGameobj().name)] = g[int.Parse(currentPoint.getGameobj().name)] + q.obstacle;
    //                    f[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calculateHValue(target, q.getGameobj());
    //                    pre[int.Parse(q.getGameobj().name)] = currentPoint;
    //                    open.Add(q);
    //                }
    //                //Node q isnot in close
    //                if (open.IndexOf(q) != -1)
    //                {
    //                    if (g[int.Parse(q.getGameobj().name)] > g[int.Parse(currentPoint.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
    //                    {
    //                        g[int.Parse(q.getGameobj().name)] = g[int.Parse(currentPoint.getGameobj().name)] + q.obstacle;
    //                        f[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calculateHValue(target, q.getGameobj());
    //                        pre[int.Parse(q.getGameobj().name)] = currentPoint;
    //                    }
    //                }
    //            }
    //        }

    //    }

    //    try
    //    {
    //        Node nextMove = new Node();
    //        int u = int.Parse(target.name);
    //        while (true)
    //        {
    //            nextMove = pre[u];
    //            if (nextMove.getGameobj().name == currentNode.name)
    //            {
    //                break;
    //            }
    //            u = int.Parse(nextMove.getGameobj().name);
    //        }
    //        return GameObject.Find(u + "").GetComponent<Node>();
    //    }
    //    catch (System.Exception e)
    //    {
    //        //currentNode = Randomnode(currentNode); //excute ramdomnode
    //    }
    //    return currentNode;
    //}

    //protected override void Shoot()
    //{
    //    if (Time.time > fireRate + timeFire)
    //    {
    //        Vector2 bulletPos = transform.position;
    //        GameObject a = Instantiate(bullet);
    //        a.SetActive(false);
    //        a.transform.position = firePosition.position;
    //        a.transform.rotation = transform.rotation;
    //        a.SetActive(true);
    //        timeFire = Time.time;
    //    }
    //}
}