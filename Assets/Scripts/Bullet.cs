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

    public float range = 5f;

    public AudioClip explosionClip;

    void Start() {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();

        powerIndex = -1;

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

        ShootableButton button = collision.gameObject.GetComponent<ShootableButton>();

        if (button != null) {
            button.ToggleGlass();
        }

        if (cameraScreen == null) {
            GameObject newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            if (powerIndex == 1) {
                newExplosion.GetComponent<MeshRenderer>().material.SetColor("_ForceFieldColor", explosionColor);
                Explode();
            } else if (powerIndex >= 2) {
                newExplosion.GetComponent<MeshRenderer>().material.SetColor("_ForceFieldColor", empColor);
                EMPExplosion();
            }

            if (powerIndex == -1) {
                LeanTween.scale(newExplosion, Vector3.one * range * 0.1f, 0.2f).setEaseInOutSine();    
            } else {
                LeanTween.scale(newExplosion, Vector3.one * range, 0.2f).setEaseInOutSine();
            }

            AudioSource.PlayClipAtPoint(explosionClip, transform.position);

            Destroy(newExplosion, 0.2f);

            Destroy(gameObject);
        }
    }

    public void EMPExplosion() {
        Collider[] around = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in around) {
            IElectric explodeable = col.GetComponent<IElectric>();

            if (explodeable != null) {
                explodeable.TurnOffElectricity();
            }
        }
    }

    public void Explode() {
        Collider[] around = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in around) {
            IExplodeable explodeable = col.GetComponent<IExplodeable>();

            if (explodeable != null) {
                explodeable.Explode(transform.position);
            }
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
