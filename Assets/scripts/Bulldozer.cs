using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class Bulldozer : MonoBehaviour
{
    public Node StartNode;
    private GameObject player;
    public GameObject target;
    //private GameObject playerControl; //excute control
    private Node currentNode, nextNode;
    private float[] g, f;
    private Node[] pre;
    public const int row = 18;
    public const int col = 10;
    public float speed = 15.0f;


    private Rigidbody2D rb;
    private float dx = 0.0f, dy = 0.0f;
    private GameObject projectile;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentNode = StartNode;
    }

    void Update()
    {
        try
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, nextNode.getGameobj().transform.position, step);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Node")
        {
            currentNode = collision.gameObject.GetComponent<Node>();
            nextNode = findNextNode();
        }
    }

    //find maxnumber in 2 number
    private float maxIn2(int a, int b)
    {
        int max = a;
        if (b > max)
            max = b;
        return max;
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
    private Node findNextNode() //create node findNextNode
    {
        g = new float[180];
        f = new float[180];
        for (int i = 0; i < 179; i++)
        {
            g[i] = 10000000;
        }
        pre = new Node[180];
        g[int.Parse(currentNode.getGameobj().name)] = 0;

        for (int i = 0; i < 179; i++)
        {
            f[i] = g[i];
        }

        // xác định vị trí ng chơi
        //target = playerControl.getCurentNode();

        List<Node> open = new List<Node>();
        List<Node> close = new List<Node>();
        open.Add(currentNode);
        while (open.Count != 0)
        {
            Node currentPoint = new Node();
            float min = 100000;

            //find the first current Node as minnimun in open
            foreach (Node node in open)
            {
                if (g[int.Parse(node.getGameobj().name)] + calculateHValue(node.getGameobj(), target) < min)
                {
                    currentPoint = node;
                    min = g[int.Parse(node.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), target);
                }
            }

            //when AI come to destination
            //if (target.transform.position.x == currentPoint.getGameobj().transform.position.x || target.transform.position.y == currentPoint.getGameobj().transform.position.y)//chay toi eage
            if (target.name == currentPoint.name)
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
                    if (g[int.Parse(q.getGameobj().name)] > g[int.Parse(currentPoint.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
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
                        g[int.Parse(q.getGameobj().name)] = g[int.Parse(currentPoint.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), q.getGameobj());
                        f[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calculateHValue(target, q.getGameobj());
                        pre[int.Parse(q.getGameobj().name)] = currentPoint;
                        open.Add(q);
                    }
                    //Node q isnot in close
                    if (open.IndexOf(q) != -1)
                    {
                        if (g[int.Parse(q.getGameobj().name)] > g[int.Parse(currentPoint.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
                        {
                            g[int.Parse(q.getGameobj().name)] = g[int.Parse(currentPoint.getGameobj().name)] + calculateHValue(currentPoint.getGameobj(), q.getGameobj());
                            f[int.Parse(q.getGameobj().name)] = g[int.Parse(q.getGameobj().name)] + calculateHValue(target, q.getGameobj());
                            pre[int.Parse(q.getGameobj().name)] = currentPoint;
                        }
                    }
                }
            }

        }

        try
        {
            Node v = new Node();
            int u = int.Parse(target.name);
            while (true)
            {
                v = pre[u];
                if (v.getGameobj().name == currentNode.name)
                {
                    break;
                }
                u = int.Parse(v.getGameobj().name);
            }
            return GameObject.Find(u + "").GetComponent<Node>();
        }
        catch (System.Exception e)
        {
            //currentNode = Randomnode(currentNode); //excute ramdomnode
        }
        return currentNode;
    }   
}