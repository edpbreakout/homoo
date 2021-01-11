using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onShieldBack : MonoBehaviour
{
    public Battling btl;

    public void ShieldBack()
    {
        btl.shielding = false;
    }
}
