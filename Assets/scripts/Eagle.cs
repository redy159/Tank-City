using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Eagle : MonoBehaviour
{
    public Sprite damaged;
    public int hp = 2;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            hp--;
            this.GetComponent<SpriteRenderer>().sprite = damaged;
            if (hp == 0) 
            SceneManager.LoadScene("game over");
        }
        if (collision.gameObject.tag == "bulldozer")
        {
            hp--;
            this.GetComponent<SpriteRenderer>().sprite = damaged;
            if (hp==0)
            SceneManager.LoadScene("game over");
        }
    }
}
