using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject FrontCheck;
    [SerializeField] private GameObject DropCheck;
    [SerializeField] private float speed;
    [SerializeField] private Animator anim;
    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null) gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.state == GameManager.GameStates.InGame)
        {
            int layers;
            if (gameObject.layer == 0)
            {
                layers = 1 << 0 | 1 << 6 | 1 << 7;//checks all layers
            }
            else
            {
                layers = 1 << gameObject.layer | 1 << 0;
            }
            Collider2D[] front = Physics2D.OverlapCircleAll(FrontCheck.transform.position, 0.005f, layers);
            Collider2D[] drop = Physics2D.OverlapCircleAll(DropCheck.transform.position, 0.005f, layers);
            foreach (Collider2D item in front)
            {
                if (item.gameObject.CompareTag("Player"))
                {
                    front = new Collider2D[front.Length - 1];
                }
            }
            if (front.Length > 0 || drop.Length == 0)//if something in front or nothing below
            {
                transform.right = -transform.right;
            }
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        speed = 0;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        anim.SetTrigger("Die");
        Destroy(gameObject, 1.2f);
    }
}
