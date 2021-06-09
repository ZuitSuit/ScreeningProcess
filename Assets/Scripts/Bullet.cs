using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Rigidbody rb;

    public float speed = 10f;
    public float lifetime = 10f;

    public GameObject explosionPrefab;

    public Light m_Light;
    TrailRenderer trail;

    [ColorUsage(false, true)]
    public Color explosionColor;
    [ColorUsage(false, true)]
    public Color empColor;

    int powerIndex = 0;

    public AudioClip explosionClip;

    void Start() {
        rb = GetComponent<Rigidbody>();

        powerIndex = -1;

        trail = GetComponent<TrailRenderer>();

        Destroy(gameObject, lifetime);
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

            if (powerIndex == 1) {
                newExplosion.GetComponent<MeshRenderer>().material.SetColor("_ForceFieldColor", explosionColor);
            } else if (powerIndex >= 2) {
                newExplosion.GetComponent<MeshRenderer>().material.SetColor("_ForceFieldColor", empColor);
            }

            LeanTween.scale(newExplosion, Vector3.one * 5f, 0.2f).setEaseInOutSine();

            AudioSource.PlayClipAtPoint(explosionClip, transform.position);

            Destroy(newExplosion, 0.2f);

            Destroy(gameObject);
        }
    }

    public void IncrementPower() {
        powerIndex++;
        SetColor(powerIndex);
    }

    public void SetColor(int index) {
        if (index == 1) {
            m_Light.color = explosionColor;
            trail.material.color = explosionColor;
        } else if (index >= 2) {
            m_Light.color = empColor;
            trail.material.color = empColor;
        }
    }

    public void ResetPower() {
        powerIndex = 0;
    }
}
