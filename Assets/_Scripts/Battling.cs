using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Battling : MonoBehaviour
{
    [Header("Inspector")]
    public float lowBound; //killing condition;
    public GameObject proj; //Projectile
    public float shootPower;
    public Animator anim;
    public Camera mainCam;
    [Header("Dynamic")]
    public bool attacking;
    public bool shielding;
    public Vector3 mousePos;


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
    }
    void Update()
    {
        //Hitting
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("attack", true);
            attacking = true;
            this.GetComponent<Movement>().attacking = true;
            launchProjectile(proj);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            attacking = false;
            this.GetComponent<Movement>().attacking = false;
        }


        //ShieldingIn
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetBool("shield", true);
            shielding = true;
            this.GetComponent<Movement>().shielding = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetBool("shield", false);
            shielding = false;
            this.GetComponent<Movement>().shielding = false;
        }


    }

    private void LateUpdate()
    {
        //killing conditions;
        if (transform.position.y < lowBound)
        {
            Kill();
        }
    }
    
    
    
    public void Kill()
    {
        Destroy(this.gameObject);
    }

    //Animation event functions
    public void onHit()
    {
        if (!attacking)
            anim.SetBool("attack", false);
    }

    public void onShield()
    {
        anim.SetBool("toIdle", true);
    }
}
