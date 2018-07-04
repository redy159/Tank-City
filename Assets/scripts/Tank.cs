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
    private float timeFire = 0.0f;
    public GameObject bullet;

    private GameObject player;
    private GameObject target;
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
        currentNode = startNode;
    }

    void RayShoot()
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
            Debug.Log(hit.collider.tag);
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

    void Update()
    {
        target = Player.curNode;

        //raycast shoot 
        RayShoot();

        try
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
            // Debug.Log(gameObject.name + " go from " + CurrentNode.name + " to " + nexnode.getGameobj().name);

        }
        catch (Exception e)
        {
            nextNode = findNextNode();
        }
        if (nextNode.obstacle == 1)
            Shoot();
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
        currentNode = collision.gameObject.GetComponent<Node>();
    }

    private void OnTriggerStay2D(Collider2D collision)
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
    private Node findNextNode() //create node findNextNode
    {
        // chỗ này đề a sao ra Node tiếp theo
        g = new float[180];
        f = new float[180];

        pre = new Node[180];
        g[currentNode.getName()] = 0;
        f[currentNode.getName()] = calculateHValue(currentNode.getGameobj(), target);

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
                if (g[node.getName()] + calculateHValue(node.getGameobj(), target) < min)
                {
                    currentPoint = node;
                    min = g[node.getName()] + calculateHValue(currentPoint.getGameobj(), target);
                }
            }

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
                //Node q is not in close and open
                if (close.IndexOf(q) == -1 && open.IndexOf(q) == -1)
                {
                    g[q.getName()] = g[currentPoint.getName()] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()) + q.obstacle;
                    f[q.getName()] = g[q.getName()] + calculateHValue(target, q.getGameobj());
                    pre[q.getName()] = currentPoint;
                    open.Add(q);
                }

                //Node q is in open
                if (open.IndexOf(q) != -1)
                    if (g[q.getName()] > g[currentPoint.getName()] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
                    {
                        g[q.getName()] = g[currentPoint.getName()] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()) + q.obstacle;
                        f[q.getName()] = g[q.getName()] + calculateHValue(target, q.getGameobj());
                        pre[q.getName()] = currentPoint;
                    }
                //Node q is in close
                if (close.IndexOf(q) == -1)
                {
                    if (g[q.getName()] > g[currentPoint.getName()] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
                    {
                        close.Remove(q);
                        open.Add(q);
                        g[q.getName()] = g[currentPoint.getName()] + q.obstacle;
                        f[q.getName()] = g[q.getName()] + calculateHValue(target, q.getGameobj());
                        pre[q.getName()] = currentPoint;
                    }
                }
            }
        }

        try
        {
            Node nextMove = new Node();
            int u = int.Parse(target.name);
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
        return currentNode;
    }

    public void Shoot()
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