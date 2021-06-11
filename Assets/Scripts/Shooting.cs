using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    Camera mainCam;
    public GameObject bulletPrefab;

    public AudioSource shootSound;

    public Transform shootPoint;
    public Transform gunT;

    bool canShoot;

    void Start() {
        mainCam = Camera.main;

        canShoot = true;
    }

    // Update is called once per frame
    void LateUpdate(){
        if (Input.GetMouseButton(0) && PlayerController.instance.CanShoot()) {
            Shoot();
        }
    }

    void Shoot() {
        if (!canShoot) return;
        canShoot = false;

        Transform camT = Camera.main.transform;
        Vector3 targetShootPoint = camT.position + camT.forward * 100f;

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one/2f);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            targetShootPoint = hit.point;
        }

        shootSound.Play();

        GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        Bullet bullet = newBullet.GetComponent<Bullet>();

        bullet.Shoot(shootPoint.position, (targetShootPoint - shootPoint.position).normalized);

        LeanTween.rotateAroundLocal(gunT.gameObject, Vector3.right, 360f, 1f).setEaseInOutQuint();
        LeanTween.delayedCall(1f, () => {
            canShoot = true;
        });
    }


}
