using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Rigidbody rb;

    public float speed = 10f;
    public float lifetime = 10f;

    void Start() {
        rb = GetComponent<Rigidbody>();

        Lean.Pool.LeanPool.Despawn(gameObject, lifetime);
    }

    public void Shoot(Vector3 position, Vector3 direction){
        rb = GetComponent<Rigidbody>();
        //pool 'em - get the bullets from the pool
        transform.position = position;
        transform.up = direction;

        rb.angularVelocity = Vector3.zero;
        rb.velocity = direction * speed;
    }
}
