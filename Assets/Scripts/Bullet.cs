using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 originPoint { get; set; }

    public void Shoot(Vector3 position, Vector3 direction)
    {
        //pool 'em - get the bullets from the pool
        transform.position = position;
        transform.LookAt(direction);

        //fling it
        originPoint = transform.position;
        
        
    }
}
