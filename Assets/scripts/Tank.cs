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
        if (collision.gameObject.tag == "bullet")
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
}
