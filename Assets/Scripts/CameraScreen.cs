using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreen : MonoBehaviour
{
    public Camera connectedCam;

    public MeshRenderer rend;

    Vector3[] corners = new Vector3[4];

    public int textureWidth;
    public float textureRatio = 2f/3.3f;

    RenderTexture renderTexture;

    public Camera[] connectedCams;

    private void Awake() {
        renderTexture = new RenderTexture((int) ( textureWidth ),
                                        (int) ( textureWidth * textureRatio ),
                                        24);
        renderTexture.Create();

        connectedCam.targetTexture = renderTexture;
        rend.material.mainTexture = renderTexture;
    }

    private void Start() {
        CalculateCorners();

        ToggleCamera(false);
    }

    void CalculateCorners() {

        List<Vector3> vertices = new List<Vector3> (gameObject.GetComponent<MeshFilter>().sharedMesh.vertices);
                
                        corners[0] = transform.TransformPoint(vertices[0]);
                        corners[1] = transform.TransformPoint(vertices[110]);
                        corners[2] = transform.TransformPoint(vertices[120]);
                        corners[3] = transform.TransformPoint(vertices[10]);
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            ToggleCamera(true);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Player")) {
            ToggleCamera(false);
        }
    }

    public void ToggleCamera(bool state) {
        connectedCam.enabled = state;

        foreach (Camera cam in connectedCams) {
            cam.enabled = state;
        }
    }

    public RenderTexture GetRenderTexture() {
        return renderTexture;
    }

    void OnDrawGizmos() {
        CalculateCorners();
        float index = 0;
        foreach (Vector3 corner in corners) {
            Gizmos.color = Color.Lerp(Color.red, Color.green, index/4);
            Gizmos.DrawCube(corner, Vector3.one * 0.1f);
            index += 1f;
        }
    }

    private void OnCollisionEnter(Collision collision){
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet == null) return;

        Vector3 collisionPoint = collision.contacts[0].point;

        float x = InverseLerp(corners[1], corners[2], collisionPoint);
        float y = InverseLerp(corners[0], corners[1], collisionPoint);

        Ray rayOut = connectedCam.ViewportPointToRay(new Vector3(x, y, 0f));

        bullet.IncrementPower();
        bullet.Shoot(rayOut.origin, rayOut.direction);
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value) {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
