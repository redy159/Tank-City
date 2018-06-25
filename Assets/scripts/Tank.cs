using UnityEngine;
using UnityEngine.SceneManagement;

public class Tank : MonoBehaviour {
    public float speed = 15.0f;
    public Transform firePosition;
    public GameObject bullet;
    public int Type; // Loai
    public int Health = 1; // Mau
    public Vector3 position; // Vi Tri Xe Tang

    public Node CurrentNode; // Node Dang O
    public Rigidbody2D rb;
    public float dx = 0.0f, dy = 0.0f;
    public GameObject projectile;

    public void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

	public void Update ()
    {
       /* dx = Input.GetAxisRaw("Horizontal");
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
            Shoot();*/
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
        dx = (x >= 1.0f)?1:0;
        dy = x == 0.0f ? ( y >= 1.0f ? 1 : 0 ) : 0.0f;
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
