using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class HunterA : MonoBehaviour
{
    public Node startNode;

    public GameObject mainTower;// main tower
    public GameObject player;// player
    //private GameObject playerControl; //excute control
    private Node currentNode, nextNode;
    private float[] g, f_for_mainTower, f_for_player;
    private Node[] pre;
    public const int row = 18;
    public const int col = 10;
    public float speed = 15.0f;
    private Node currentPoint;

    private Rigidbody2D rb;
    private float dx = 0.0f, dy = 0.0f;
    private GameObject projectile;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentNode = startNode;
    }

    void Update()
    {
        player = Player.curNode;
        try
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextNode.getGameobj().transform.position, step);

            float x = (nextNode.getGameobj().transform.position.x - currentNode.position.x);
            float y = (nextNode.getGameobj().transform.position.y - currentNode.position.y);

            if (!(x == 0 && y == 0)) {
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
            // Debug.Log(gameObject.name + " go from " + CurrentNode.name + " to " + nexnode.getGameobj().name);

        }
        catch (Exception e)
        {
            nextNode = findNextNode();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.bounds.center);
    //    Debug.Log(this.GetComponent<Collider2D>().bounds.center);
    //}

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
    private float calculateHValue(GameObject a, GameObject b)
    {
        float xa = a.transform.position.x;
        float ya = a.transform.position.y;
        float xb = b.transform.position.x;
        float yb = b.transform.position.y;

        // Return using the distance formula
        return Mathf.Sqrt((xa - xb) * (xa - xb) + (ya - yb) * (ya - yb));
    }

    //private Node findMinNode(float[] f_for_mainTower, float[] f_for_player, List<Node> open)
    //{
    //    Node result;
       
    //    return result;
    //}

    // find all acceptable next point with standind point
    private Node findNextNode() //create node findNextNode
    {
        g = new float[180];
        f_for_mainTower = new float[180];
        f_for_player = new float[180];

        pre = new Node[180];
        g[int.Parse(currentNode.getGameobj().name)] = 0;
        f_for_mainTower[int.Parse(currentNode.getGameobj().name)] = calculateHValue(currentNode.getGameobj(), mainTower);
        f_for_player[int.Parse(currentNode.getGameobj().name)] = calculateHValue(currentNode.getGameobj(), player);

        // xác định vị trí ng chơi
        //target = playerControl.getCurentNode();
        
        List<Node> open = new List<Node>();
        List<Node> close = new List<Node>();
        open.Add(currentNode);
        while (open.Count != 0)
        {
            currentPoint = new Node();
          
            float min = 10000;

            //find the first current Node as minnimun in open
            foreach (Node node in open)
            {
                if (g[int.Parse(node.getGameobj().name)] + calculateHValue(node.getGameobj(), mainTower) < min)
                {
                    currentPoint = node;
                    min = g[int.Parse(node.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), mainTower);
                    Debug.Log(currentPoint);
                }
            }

            foreach (Node node in open)
            {
                if (g[int.Parse(node.getGameobj().name)] + calculateHValue(node.getGameobj(), player) < min)
                {
                    currentPoint = node;
                    min = g[int.Parse(node.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), player);
                    Debug.Log(currentPoint);
                }
            }

            //if (target.transform.position.x == currentPoint.getGameobj().transform.position.x || target.transform.position.y == currentPoint.getGameobj().transform.position.y)//chay toi eage
            if ((mainTower.name == currentPoint.name) || (player.name == currentPoint.name))
            {
                break;  //shoot for AI
            }

            open.Remove(currentPoint);
            close.Add(currentPoint);

            List<Node> nextPoint = new List<Node>();

            #region thêm vào list các đỉnh quanh currentPoint
            if (currentPoint.TopNode != null)
            {
                nextPoint.Add(currentPoint.TopNode);
            }
            if (currentPoint.BottomNode != null)
            {
                nextPoint.Add(currentPoint.BottomNode);
            }
            if (currentPoint.LeftNode != null)
            {
                nextPoint.Add(currentPoint.LeftNode);
            }
            if (currentPoint.RightNode != null)
            {
                nextPoint.Add(currentPoint.RightNode);
            }
            #endregion

            foreach (Node q in nextPoint)
            {
                //Node q isnot in close
                if (close.IndexOf(q) != -1)
                {
                    if (g[int.Parse(q.getGameobj().name)] > (g[int.Parse(currentPoint.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), q.getGameobj())))
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
                        g[int.Parse(q.getGameobj().name)] = g[int.Parse(currentPoint.getGameobj().name)] + q.obstacle;
                        f_for_mainTower[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calculateHValue(mainTower, q.getGameobj());
                        f_for_player[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calculateHValue(player, q.getGameobj());
                        pre[int.Parse(q.getGameobj().name)] = currentPoint;
                        open.Add(q);
                    }
                    //Node q isnot in close
                    if (open.IndexOf(q) != -1)
                    {
                        if (g[int.Parse(q.getGameobj().name)] > g[int.Parse(currentPoint.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
                        {
                            g[int.Parse(q.getGameobj().name)] = g[int.Parse(currentPoint.getGameobj().name)] + q.obstacle;
                            f_for_mainTower[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calculateHValue(mainTower, q.getGameobj());
                            f_for_player[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calculateHValue(player, q.getGameobj());
                            pre[int.Parse(q.getGameobj().name)] = currentPoint;
                        }
                    }
                }
            }

        }

        if (currentPoint.name == mainTower.name)
        {
            try
            {
                Node nextMove = new Node();
                int u = int.Parse(mainTower.name);
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
