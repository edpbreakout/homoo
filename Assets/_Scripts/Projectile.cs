using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeSpan;
    private void Awake()
    {
        Destroy(this.gameObject, lifeSpan);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.GetComponent<CircleCollider2D>().enabled = false;
        Rigidbody2D rg = this.GetComponent<Rigidbody2D>();
        rg.velocity = Vector2.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
