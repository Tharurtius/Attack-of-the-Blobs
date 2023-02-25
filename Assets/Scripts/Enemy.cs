using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject FrontCheck;
    [SerializeField] private GameObject DropCheck;
    [SerializeField] private float speed;
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
        if (front.Length > 0 || drop.Length == 0)//if something in front or nothing below
        {
            transform.right = -transform.right;
        }
        transform.position += transform.right * speed * Time.deltaTime;
    }

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("Die");
        Destroy(gameObject, 1.2f);
    }
}
