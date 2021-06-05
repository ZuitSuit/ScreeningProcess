using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreen : MonoBehaviour
{
    public Camera connectedCam;

    public Vector2Int resolution;
    public RenderTexture renderTexturePrefab;
    public RenderTexture renderTexture { get; set; }

    public Renderer screenRenderer;

    private void Start()
    {
        renderTexture = Instantiate(renderTexturePrefab);
        connectedCam.targetTexture = renderTexture;
        screenRenderer.material = Instantiate(screenRenderer.material);
        screenRenderer.material.SetVector("_MatrixResolution", (Vector2)resolution);
        screenRenderer.material.SetTexture("_UITexture", renderTexture);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>();
        if (bullet == null) return;

        Vector3 direction = (collision.contacts[0].point - bullet.originPoint).normalized;


    } 
}
