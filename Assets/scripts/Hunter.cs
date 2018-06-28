using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Hunter : MonoBehaviour
{
    public Node startNode;
    private GameObject player;
    public GameObject target;
    //private GameObject playerControl; //excute control
    private Node currentNode, nextNode;
    private float[] g, f;
    private Node[] pre;

    public float speed = 15.0f;
    public Transform firePosition;
    public GameObject bullet;
    public Sprite death;

    private Rigidbody2D rb;
    private float dx = 0.0f, dy = 0.0f;
    private GameObject projectile;

    bool canSwitch = false;
    bool waitActive = false; //so wait function wouldn't be called many times per frame
    private bool hit = false;
    

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        dx = 1;
        dy = 0;
       RaycastHit2D hit = Physics2D.Raycast(firePosition.position, new Vector2(1,0) );
		float R = 10;
        Debug.Log(hit.centroid);
         
        //Debug.Log(hit.rigidbody.name);
		if (hit.collider != null) {
            Debug.Log(hit.collider.transform);
			if (hit.rigidbody.tag == "Player"){ 
				float DX = Mathf.Abs(hit.rigidbody.position.x - transform.position.x);
				float DY = Mathf.Abs(hit.rigidbody.position.y - transform.position.y);
				if ((DX <= R) || (DY <= R) ){
					//this.shoot();
					Debug.Log(hit.rigidbody.position.x);
					Debug.Log(hit.rigidbody.position.y);
				}
				
			}
        }
        else 
            Debug.Log("Null");
    }

    IEnumerator Wait()
    {
        waitActive = true;
        this.GetComponent<SpriteRenderer>().sprite = death;
        yield return new WaitForSecondsRealtime(0.2f);
        hit = true;
        canSwitch = true;
        waitActive = false;
    }

    void Update()
    {
        dx = Input.GetAxisRaw("Horizontal");
        dy = dx == 0.0f ? Input.GetAxisRaw("Vertical") : 0.0f;


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


        rb.velocity = new Vector2(dx, dy) * speed;

        if (Input.GetButtonDown("Fire1"))
            Shoot();
        if (hit == true)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            
            StartCoroutine(Wait());
        }
    }

    void Shoot()
    {
        Vector2 bulletPos = transform.position;
        GameObject a = Instantiate(bullet);
        a.SetActive(false);
        a.transform.position = firePosition.position;
        a.transform.rotation = transform.rotation;
        a.SetActive(true);
    }
}
