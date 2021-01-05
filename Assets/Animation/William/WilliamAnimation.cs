using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilliamAnimation : MonoBehaviour
{
    public Animator anim;
    public GameObject shield;

   

    public void onShield()
    {
        anim.SetBool("toIdle", true);
        anim.SetBool("shield", false);
    }

    public void outShield()
    {
        
    }
}
