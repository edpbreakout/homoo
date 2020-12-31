using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Battling : MonoBehaviour
{
    [Header("Inspector")]
    public float lowBound; //killing condition;
    public Rigidbody2D rigid;
    public GameObject proj; //Projectile
    public float shootPower;
    public Camera mainCam;
    public int maxHealth;
    public float respawnTime;
    [Header("Dynamic")]
    public int gunType;
    public Vector3 mousePos;
    public int health;
    public Movement parent;
    
    private void Awake()
    {
        Spawn();
    }
    public void Spawn()
    {
        rigid = GetComponent<Rigidbody2D>();
        health = maxHealth;
        rigid.velocity = Vector3.zero;
        print("Spawned");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null && !parent.shielding)
        {
            Projectile proj = collision.gameObject.GetComponent<Projectile>();
            if(proj.playerNo != parent.PlayerNo)
            {
                health -= proj.damage;
            }
        }
        print("ouch");  
    }

    void launchProjectile(GameObject pj)
    {
        GameObject go = Instantiate<GameObject>(pj);

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        go.transform.position = transform.position + Vector3.up*0.5f;

        Vector3 vectorToTarget = mousePos - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
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
                parent.anim.SetBool("attack", true);
                parent.attacking = true;    
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
