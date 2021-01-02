using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Battling : MonoBehaviour
{
    [Header("Inspector")]
    public float lowBound; //killing condition;
    public GameObject proj; //Projectile
    public float shootPower;
    public int maxHealth;
    public float respawnTime;
    public List<GameObject> hearts;
    [Header("Melee attacking")]
    public LayerMask attackables;
    public Collider2D attackZone;
    public List<Collider2D> attackCollisions;
    ContactFilter2D ctc;
    [Header("Dynamic")]
    public Rigidbody2D rigid;
    public int gunType;
    public int health;
    public Movement parent;
    private Vector3 mousePos;
    private Camera mainCam;
    //Variables for melee attack system;


    private void Awake()
    {
        Spawn();
        ctc = new ContactFilter2D();
        ctc.layerMask = attackables;
        mainCam = Camera.main;
    }
    public void Spawn()
    {
        rigid = GetComponent<Rigidbody2D>();
        health = maxHealth;
        rigid.velocity = Vector3.zero;
        foreach(GameObject go in hearts)
        {
            go.GetComponent<Animator>().SetBool("toIdle", true);
            go.GetComponent<Animator>().SetBool("isDead", false);

        }

        print("Spawned");

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null && !parent.shielding)
        {
            Projectile proj = collision.gameObject.GetComponent<Projectile>();
            if(proj.playerNo != parent.PlayerNo)
            {
                onDamage();
                Destroy(collision.gameObject);
            }
        }
    }
    void launchProjectile(GameObject pj)
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 centerPos = transform.position + Vector3.up * 0.5f;
        Vector3 vectorToTarget = (mousePos - centerPos).normalized * 0.5f;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);


        GameObject go = Instantiate<GameObject>(pj);
        go.transform.position = centerPos + vectorToTarget;
        go.transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
        Rigidbody2D rg = go.GetComponent<Rigidbody2D>();
        rg.velocity = (mousePos - go.transform.position).normalized * shootPower;


        //Setting the number of the "author" of projectile, so proj won't hit author himself
        go.GetComponent<Projectile>().playerNo = parent.PlayerNo;
    }
    void Update()
    {
        //Hitting
        if (Input.GetKeyDown(parent.cm.controlls["attack"]))
        {
            if(gunType == 0)
            {
                attackZone.OverlapCollider(ctc, attackCollisions);
                parent.anim.SetBool("attack", true);
                parent.attacking = true;    
                foreach(Collider2D go in attackCollisions)
                {
                    Battling btl = go.GetComponentInParent<Battling>();
                    if (btl != null)
                    {
                        btl.onDamage();
                    }
                }
            }
            else if(gunType == 1)
            {
                launchProjectile(proj);

            }

        }
        if (Input.GetKeyUp(parent.cm.controlls["attack"]))
        {
            parent.attacking = false;
        }
       
        //ShieldingIn
        if (Input.GetKeyDown(parent.cm.controlls["shield"]))
        {
            parent.anim.SetBool("shield", true);
            parent.shielding = true;
        }
        if (Input.GetKeyUp(parent.cm.controlls["shield"]))
        {
            parent.anim.SetBool("shield", false);
            parent.shielding = false;
        }

        if(Input.GetKeyDown(parent.cm.controlls["change"]))
        {
            gunType++;
            gunType %= 2;//2 - max amount of guns

        }

    }

    private void LateUpdate()
    {
        //killing conditions;
        if (transform.position.y < lowBound || health <= 0)
        {
            ControllManager.S.respawn(parent.PlayerNo);
        }
    }
    
    public void onDamage()
    {
        health--;
        if(health > 0)
            hearts[health - 1].GetComponent<Animator>().SetBool("isDead", true);

    }

    //Animation event functions
    public void onHit()
    {
        if (!parent.attacking)
            parent.anim.SetBool("attack", false);
    }

    public void onShield()
    {
        parent.anim.SetBool("toIdle", true);
    }
}
