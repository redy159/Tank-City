using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

 public class PriorityQueue<T> where T : IComparable<T>
  {
    private List<T> data;

    public PriorityQueue()
    {
      this.data = new List<T>();
    }

    public void Enqueue(T item)
    {
      data.Add(item);
      int ci = data.Count - 1; // child index; start at end
      while (ci > 0)
      {
        int pi = (ci - 1) / 2; // parent index
        if (data[ci].CompareTo(data[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
        T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
        ci = pi;
      }
    }

    public T Dequeue()
    {
      // assumes pq is not empty; up to calling code
      int li = data.Count - 1; // last index (before removal)
      T frontItem = data[0];   // fetch the front
      data[0] = data[li];
      data.RemoveAt(li);

      --li; // last index (after removal)
      int pi = 0; // parent index. start at front of pq
      while (true)
      {
        int ci = pi * 2 + 1; // left child index of parent
        if (ci > li) break;  // no children so done
        int rc = ci + 1;     // right child
        if (rc <= li && data[rc].CompareTo(data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
          ci = rc;
        if (data[pi].CompareTo(data[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
        T tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp; // swap parent and child
        pi = ci;
      }
      return frontItem;
    }

    public T Peek()
    {
      T frontItem = data[0];
      return frontItem;
    }

    public int Count()
    {
      return data.Count;
    }

    public override string ToString()
    {
      string s = "";
      for (int i = 0; i < data.Count; ++i)
        s += data[i].ToString() + " ";
      s += "count = " + data.Count;
      return s;
    }

    public bool IsConsistent()
    {
      // is the heap property true for all data?
      if (data.Count == 0) return true;
      int li = data.Count - 1; // last index
      for (int pi = 0; pi < data.Count; ++pi) // each parent index
      {
        int lci = 2 * pi + 1; // left child index
        int rci = 2 * pi + 2; // right child index

        if (lci <= li && data[pi].CompareTo(data[lci]) > 0) return false; // if lc exists and it's greater than parent then bad.
        if (rci <= li && data[pi].CompareTo(data[rci]) > 0) return false; // check the right child too.
      }
      return true; // passed all checks
    } // IsConsistent
  } // PriorityQueue
 
public class Tank : MonoBehaviour {
    public float speed = 15.0f;
    public float fireRate = 0.5f;
    public Transform firePosition;
    public GameObject bullet;
    public int Type; // Loai
    public int Health = 1; // Mau
    public Vector3 position; // Vi Tri Xe Tang

    public float timeFire = 0.0f;
    public Rigidbody2D rb;
    public Node startNode;
    public GameObject player;
    public GameObject target;
    //public GameObject playerControl; //excute control
    public Node currentNode, nextNode;
    public float[] g, f;
    public Node[] pre;
    public const int row = 18;
    public const int col = 10;
    public float dx = 0.0f, dy = 0.0f;
    public GameObject projectile;
    //public GameObject projectile;
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

    

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentNode = startNode;
    }

       public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Node")
            currentNode = collision.gameObject.GetComponent<Node>();

    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.tag == "Node") && (Math.Abs(collision.bounds.center.y - GetComponent<Collider2D>().bounds.center.y) <= 0.11) && (Math.Abs(collision.bounds.center.x - GetComponent<Collider2D>().bounds.center.x) <= 0.11))
        {
            currentNode = collision.gameObject.GetComponent<Node>();
            nextNode = findNextNode();
        }
    }


    // return heuristic value
    public float calculateHValue(GameObject a, GameObject b){
        return 0f;
    }

    // find all acceptable next point with standind point
    public Node findNextNode() //create node findNextNode
    {
        // chỗ này đề a sao ra Node tiếp theo
        g = new float[180];
        f = new float[180];

        pre = new Node[180];
        g[currentNode.getName()] = 0;
        f[currentNode.getName()] = calculateHValue(currentNode.getGameobj(), target);

        // xác định vị trí ng chơi
        //target = playerControl.getCurentNode();

        //List <Node> open = new List<Node>();
        //List <Node> close = new List<Node>();

        SortedDictionary <float, Node> open = new  SortedDictionary <float, Node>();
        SortedDictionary <int, Node> close = new  SortedDictionary <int, Node>();

        open.Add(0,currentNode);
        while (open.Count != 0)
        {
            Node currentPoint = new Node();
            currentPoint = open.First().Value;
            //find the first current Node as minnimun in open
            open.Remove(open.Keys.First());
            close.Add(currentPoint.getName(),currentPoint);

            List<Node> adjacentNode = new List<Node>();

            //thêm vào list các Node Ke currentPoint
            if (currentPoint.TopNode != null)
            {
                adjacentNode.Add(currentPoint.TopNode);
            }
            if (currentPoint.BottomNode != null)
            {
                adjacentNode.Add(currentPoint.BottomNode);
            }
            if (currentPoint.LeftNode != null)
            {
                adjacentNode.Add(currentPoint.LeftNode);
            }
            if (currentPoint.RightNode != null)
            {
                adjacentNode.Add(currentPoint.RightNode);
            }
            

            foreach (Node q in adjacentNode) {
                //Node q is not in close and open
                if (!(close.ContainsKey(q.getName()) && open.ContainsValue(q)))
                {
                    g[q.getName()] = g[currentPoint.getName()] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()) + q.obstacle;
                    f[q.getName()] = g[q.getName()] + calculateHValue(target, q.getGameobj());
                    pre[q.getName()] = currentPoint;
                   
                }

                //Node q is in open
                if (open.ContainsValue(q))
                    if (g[q.getName()] > g[currentPoint.getName()] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
                    {
                        g[q.getName()] = g[currentPoint.getName()] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()) + q.obstacle;
                        f[q.getName()] = g[q.getName()] + calculateHValue(target, q.getGameobj());
                        pre[q.getName()] = currentPoint;
                    }
                //Node q is in close
             
                if (close.ContainsKey(q.getName()))
                {
                    if (g[q.getName()] > g[currentPoint.getName()] + calculateHValue(currentPoint.getGameobj(), q.getGameobj()))
                    {
                        close.Remove(q.getName());
                        g[q.getName()] = g[currentPoint.getName()] + q.obstacle;
                        f[q.getName()] = g[q.getName()] + calculateHValue(target, q.getGameobj());
                        pre[q.getName()] = currentPoint;
                        open.Add(f[q.getName()],q);
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
}
