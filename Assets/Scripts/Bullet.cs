using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Rigidbody rb;

    public float speed = 10f;
    public float lifetime = 10f;

    public GameObject explosionPrefab;

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

    void OnCollisionEnter(Collision collision) {
        CameraScreen cameraScreen = collision.gameObject.GetComponent<CameraScreen>();

        if (cameraScreen == null) {
            GameObject newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            LeanTween.scale(newExplosion, Vector3.one * 5f, 0.2f).setEaseInOutSine();

            Destroy(newExplosion, 0.2f);

            Lean.Pool.LeanPool.Despawn(gameObject);
        }
    }
}
