using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement: MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Inspector")]
    public float speed;
    public float jumpPower;
    public int maxAirJump;
    public Rigidbody2D rigid;
    public Animator anim;
    [Header("onGroundChecking")]
    public Transform topLeft;
    public Transform bottomRight;
    public LayerMask groundLayer;
    [Header("Dynamic")]
    public Vector2 dir;
    public int toRightScale;
    public bool grounded;
    public int jumpCount;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hor, ver;
        hor = Input.GetAxisRaw("Horizontal") * speed;
        ver = rigid.velocity.y;
        dir = new Vector2(hor, ver);
        rigid.velocity = dir; 
        //Set animation 
        anim.SetFloat("speed", Mathf.Abs(hor));

        //Flip char to looking direction.
        if(hor > 0)
        {
            transform.localScale = Vector3.one;
        }
        if(hor < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Jumping
        grounded = Physics2D.OverlapArea(topLeft.position, bottomRight.position, groundLayer);
        if (grounded)
        {
            jumpCount = 0;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if(!grounded)
            {
                jumpCount++;
            }
            if(jumpCount <= maxAirJump)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
            
        }
    }
}
