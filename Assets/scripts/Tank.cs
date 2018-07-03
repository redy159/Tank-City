using UnityEngine;
using UnityEngine.SceneManagement;

public class Tank : MonoBehaviour {
    public float speed = 15.0f;
    public Transform firePosition;
    public GameObject bullet;
    public int Type; // Loai
    public int Health = 1; // Mau
    public Vector3 position; // Vi Tri Xe Tang

    protected Node CurrentNode; // Node Dang O
    public Rigidbody2D rb;
    protected float dx = 0.0f, dy = 0.0f;
    //public GameObject projectile;

    public void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }


    public void FacingDirection(){// Quay Mat Ve Huong Nao
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

    void Move(float x,float y){// Di Chuyen Sang X Hoac Y do ( Dang Sai Chua Fix)
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
		rb.velocity = new Vector2(dx, dy) * speed;
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet_enemy")
        {
            Destroy(gameObject);
            SceneManager.LoadScene("game over");
        }
    }

    public void Shoot()
    {
        Vector2 bulletPos = transform.position;
        GameObject a= Instantiate(bullet);
        a.SetActive(false);
        a.transform.position = firePosition.position;
        a.transform.rotation = transform.rotation;
        a.SetActive(true);
    }

    //public Node astar(float )
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
}
