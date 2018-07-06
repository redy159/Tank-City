using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class Seeker : Tank
{
    void Update()
    {
        player = Player.curNode;

        RayShoot();

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
        if (collision.gameObject.tag == "base")
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
    public override Node findMinNode(ref float[] fBase, ref float[] fPlayer, ref List<Node> open)
    {
        
        float min = 10000;
        Node currentPoint = new Node();
        foreach (Node node in open)
        {
            if (g[int.Parse(node.getGameobj().name)] + calH(node.getGameobj(), player) < min)
            {
                currentPoint = node;
                min = g[int.Parse(node.getGameobj().name)] + calH(currentPoint.getGameobj(), player);
            }
        }
        return currentPoint;
    }

    public override void calF(ref float[] fBase, ref float[] fPlayer, Node q)
    {
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
        //fbase[int.Parse(currentNode.getGameobj().name)] = calH(currentNode.getGameobj(), Base);
        fplayer[int.Parse(currentNode.getGameobj().name)] = calH(currentNode.getGameobj(), player);
    }

    public override bool CheckName()
    {
        return ((player.name == currentPoint.name));
    }

    public override Node TraceBack(Node currentPoint)
    {
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
