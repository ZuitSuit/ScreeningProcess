using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    Camera mainCam;
    public GameObject bulletPrefab;

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

        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        GameObject newBullet = Lean.Pool.LeanPool.Spawn(bulletPrefab, ray.origin, Quaternion.identity);

        newBullet.GetComponent<Bullet>().Shoot(ray.origin, ray.direction);
    }


}
