using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public GameObject poi;
    public float u;
    Vector2 p0, p1, p01;
    private void Update()
    {

        p0 = this.transform.position;
        p1 = poi.transform.position;

        //p01 = (1-u)*p0 + u*p1;
        p01 = (p1 - p0) * u + p0;
        Vector3 pos = new Vector3(p01.x, p01.y, -10);
        transform.position = pos;

    }
}
