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

    public float reloadTime = 2f;

    public LayerMask shootLayer;

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

    void OnDrawGizmos() {
        Transform camT = Camera.main.transform;
        Vector3 targetShootPoint = camT.position + camT.forward * 10f;

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one/2f);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, shootLayer)) {
            targetShootPoint = hit.point;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawCube(targetShootPoint, Vector3.one * 0.1f);
    }

    void Shoot() {
        if (!canShoot) return;
        canShoot = false;

        Transform camT = Camera.main.transform;
        Vector3 targetShootPoint = camT.position + camT.forward * 10f;

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one/2f);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, shootLayer)) {
            targetShootPoint = hit.point;
        }

        shootSound.Play();

        GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        Bullet bullet = newBullet.GetComponent<Bullet>();

        bullet.Shoot(shootPoint.position, (targetShootPoint - shootPoint.position).normalized);

        LeanTween.rotateAroundLocal(gunT.gameObject, Vector3.right, 360f, reloadTime).setEaseInOutQuint();
        LeanTween.delayedCall(reloadTime, () => {
            canShoot = true;
        });
    }


}
