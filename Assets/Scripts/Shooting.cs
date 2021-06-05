using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    Camera mainCam;

    void Start() {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButton(0)) {
            Shoot();
        }
    }

    void Shoot() {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one/2f);

        if (Physics.Raycast(ray, out RaycastHit hit)) {

        }
    }
}
