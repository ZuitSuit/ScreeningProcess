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

    private void Start() {
        RenderTexture renderTexture = new RenderTexture((int)(textureWidth),
                                        (int) (textureWidth * textureRatio),
                                        24);
        renderTexture.Create();

        print(renderTexture.width + " " + renderTexture.height);
        
        connectedCam.targetTexture = renderTexture;
        //rend.material.SetTexture("_MainTex",renderTexture);
        rend.material.mainTexture = renderTexture;

        /*screenRenderer.material = Instantiate(screenRenderer.material);
        screenRenderer.material.SetVector("_MatrixResolution", (Vector2)resolution);
        screenRenderer.material.SetTexture("_UITexture", renderTexture);*/

        CalculateCorners();
    }

    void CalculateCorners() {
        corners[0] = rend.bounds.min;

        corners[1] = rend.bounds.min;
        corners[1].y = rend.bounds.max.y;

        corners[2] = rend.bounds.max;

        corners[3] = rend.bounds.max;
        corners[3].y = rend.bounds.min.y;
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

        Vector3 center = rend.bounds.max;
        Vector3 size = rend.bounds.min;

        float x = InverseLerp(corners[1], corners[2], collisionPoint);
        float y = InverseLerp(corners[0], corners[1], collisionPoint);

        Ray rayOut = connectedCam.ViewportPointToRay(new Vector3(x, y, 0f));

        bullet.Shoot(rayOut.origin, rayOut.direction);
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value) {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
