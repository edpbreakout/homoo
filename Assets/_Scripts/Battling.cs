using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Battling : MonoBehaviour
{
    [Header("Inspector")]
    public Movement parent;
    public float lowBound; //killing condition;
    public GameObject proj; //Projectile
    public float shootPower;
    public float shootDelay;
    public int maxHealth;
    public List<GameObject> hearts;
    [Header("Melee attacking")]
    public LayerMask attackables;
    public Collider2D attackZone;
    public List<Collider2D> attackCollisions;
    ContactFilter2D ctc;
    [Header("Dynamic")]
    public bool attacking;
    public bool shielding;
    public int gunType;
    public int health;
    public Rigidbody2D rigid;
    private Vector3 mousePos;
    private Camera mainCam;
    private float shootTime;
    //Variables for melee attack system;


    private void Awake()
    {
        Spawn();
        ctc = new ContactFilter2D();
        ctc.layerMask = attackables;
        mainCam = Camera.main;
        shootTime = 0;
    }
    public void Spawn()
    {
        gameObject.SetActive(true);
        rigid = GetComponent<Rigidbody2D>(  );
        health = maxHealth;
        rigid.velocity = Vector3.zero;
        foreach(GameObject go in hearts)
        {
            go.GetComponent<Animator>().SetBool("toIdle", true);
            go.GetComponent<Animator>().SetBool("isDead", false);
        }
        attacking = false;
        shielding = false;
        print("Spawned");
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null && !shielding)
        {
            Projectile proj = collision.gameObject.GetComponent<Projectile>();
            if(proj.playerNo != parent.PlayerNo)
            {
                onDamage();
                Destroy(collision.gameObject);
            }
        }
    }
    public void rangeAttack(GameObject pj)
    {
        if (Time.time - shootTime < shootDelay)
            return;
        shootTime = Time.time;

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

    public void meleeAttack()
    {
        attackZone.OverlapCollider(ctc, attackCollisions);
        parent.anim.SetBool("attack", true);
        attacking = true;    //This value set to false, when animation ends;
        foreach (Collider2D go in attackCollisions)
        {
            Battling btl = go.GetComponentInParent<Battling>();
            if (btl != null && !btl.shielding)
            {
                btl.onDamage();
            }
        }
    }

    public virtual void Shielding()
    {
        if (Input.GetKeyDown(parent.cm.controlls["shield"]))
        {
            parent.anim.SetBool("shield", true);
            shielding = true;
        }
        if (Input.GetKeyUp(parent.cm.controlls["shield"]))
        {
            parent.anim.SetBool("shield", false);
            shielding = false;
        }
    }
    void Update()
    {
        //Hitting
        if (Input.GetKey(parent.cm.controlls["attack"]) && !attacking && !shielding)
        {
            if(gunType == 0 )
            {
                meleeAttack();
            }
            else if(gunType == 1)
            {
                rangeAttack(proj);
            }

        }
      
        if(Input.GetKeyDown(parent.cm.controlls["change"]))
        {
            gunType++;
            gunType %= 2;//2 - max amount of guns

        }

        Shielding();
        

        if (transform.position.y < lowBound || health <= 0)
        {
            ControllManager.S.respawn(parent.PlayerNo);
        }
    }
    public void onDamage()
    {
        health--;
        if(health > 0)
        {
            hearts[health - 1].GetComponent<Animator>().SetBool("isDead", true);
        }

    }
    //Animation event functions
    public void onHit()
    {
        attacking = false;
        parent.anim.SetBool("attack", false);
    }
    public void onShieldOut()
    {
        parent.anim.SetBool("toIdle", true);
    }
}
