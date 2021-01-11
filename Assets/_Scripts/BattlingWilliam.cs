using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlingWilliam : Battling
{
    // Start is called before the first frame update
    public Animator shieldAnim;

    public override void Shielding()
    {
        if (Input.GetKeyDown(parent.cm.controlls["shield"]))
        {
            shieldAnim.SetBool("shield", true);
            shielding = true;
        }
        if (Input.GetKeyUp(parent.cm.controlls["shield"]))
        {
            shieldAnim.SetBool("shield", false);
        }
    }
}
