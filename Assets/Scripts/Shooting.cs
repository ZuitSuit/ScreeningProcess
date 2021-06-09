using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    Camera mainCam;
    public GameObject bulletPrefab;

    public AudioSource shootSound;

    void Start() {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    void Shoot() {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one/2f);

        shootSound.Play();

        GameObject newBullet = Instantiate(bulletPrefab, ray.origin, Quaternion.identity);

        Bullet bullet = newBullet.GetComponent<Bullet>();

        bullet.Shoot(ray.origin, ray.direction);
    }


}
