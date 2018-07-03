using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "bullet")|| (collision.gameObject.tag == "bulldozer")|| (collision.gameObject.tag == "bullet_enemy"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Node")
            collision.gameObject.GetComponent<Node>().obstacle = 0;
    }
}
