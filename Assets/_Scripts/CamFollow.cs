using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [Header("Inspector")]
    public Transform poi; //point of interest;
    public Vector2 offset;
    [Range(0,1)]
    public float u; //Cam following rate;
    private Vector2 tmp;
    private void Awake()
    {
    }


    
    private void FixedUpdate()
    {
        if(poi != null)
        {
            tmp = (poi.position - transform.position) * u + transform.position;
            transform.position = new Vector3(tmp.x + offset.x, tmp.y + offset.y, -1);
        }
    }

    
   
}
