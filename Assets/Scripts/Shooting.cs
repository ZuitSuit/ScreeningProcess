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
        if (Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    void Shoot() {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one/2f);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;
                

/*            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null) {
                print(rend);
                print(rend.sharedMaterial);
                print(rend.sharedMaterial.mainTexture);
                print(meshCollider);

                return;
            }*/

            Vector2 pixelUV = hit.textureCoord;

            print(pixelUV);
        }
    }


}
