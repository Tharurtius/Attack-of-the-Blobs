using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Flash()//flash green when triggered
    {
        float timer = 1f;
        Color color;
        while (timer >= 0)
        {
            color = sprite.color;
            color.r -= Time.deltaTime;
            color.b -= Time.deltaTime;
            sprite.color = color;
            timer -= Time.deltaTime;
            yield return null;
        }
        timer = 1f;
        while (timer >= 0)
        {
            color = sprite.color;
            color.r += Time.deltaTime;
            color.b += Time.deltaTime;
            sprite.color = color;
            timer -= Time.deltaTime;
            yield return null;
        }
        sprite.color = Color.white;
    }
}
