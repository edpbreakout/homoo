using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{

    // Start is called before the first frame update
    [Header("Inspector")]
    [Range(0, 2)]
    public int PlayerNo;
    public float speed;
    public float jumpPower;
    public int maxAirJump;
    public Rigidbody2D rigid;
    public Animator anim;
    [Header("onGroundChecking")]
    public Transform topLeft;
    public Transform bottomRight;
    public LayerMask groundLayer;
    public bool grounded;
    public int jumpCount;
    public List<Collider2D> groundCollisions;
    [Header("Dynamic")]
    public bool attacking;
    public bool shielding;
    public PlayerInfo cm;
    private Vector2 dir;
    void Start()
    {
        cm = ControllManager.S.players[PlayerNo];
    }



    
    // Update is called once per frame
    void Update()
    {
        float hor = 0, ver;
       
        if (Input.GetKey(cm.controlls["left"]))
        {
            hor = -speed;
        }
        if (Input.GetKeyUp(cm.controlls["left"]))
        {
            hor = 0;
        }
        if (Input.GetKey(cm.controlls["right"]))
        {
            hor = speed;
        }
        if (Input.GetKeyUp(cm.controlls["right"]))
        {
            hor = 0;

        }

        ver = rigid.velocity.y;
        if (shielding)
            hor /= 3;
        dir = new Vector2(hor, ver);
        rigid.velocity = dir;
        //Set animation  
        anim.SetFloat("speed", Mathf.Abs(hor));

        //Flip char to looking direction.
        if (hor > 0)
        {
            transform.localScale = Vector3.one;
        }
        if (hor < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Jumping
        grounded = Physics2D.OverlapArea(topLeft.position, bottomRight.position, groundLayer);
        if (grounded)
        {
            jumpCount = 0;
        }
        if (Input.GetKeyDown(cm.controlls["jump"]))
        {
            if (!grounded)
            {
                jumpCount++;
            }
            if (jumpCount <= maxAirJump && !shielding)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }

        }



    }




}