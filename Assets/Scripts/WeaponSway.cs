using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    public Vector2 mouseSwayMultiplier = Vector2.one;

    public MeshRenderer meshRend;

    public void Sway(Vector3 velocity, bool aiming) {
        transform.Rotate(Input.GetAxis("Mouse Y") * transform.right * 0.3f * mouseSwayMultiplier.x);
        transform.Rotate(-Input.GetAxis("Mouse X") * (Vector3.up + Vector3.forward) * 0.2f * mouseSwayMultiplier.y);

        float time = Time.time * 0.4f;

        float noiseX = Mathf.PerlinNoise(time * 2f, time * 4f) * 6f;
        float noiseY = Mathf.PerlinNoise(time + 100f, time + 100f) * 3f;
        float noiseZ = Mathf.PerlinNoise(time + 200f, time + 200f) * 3f;

        Quaternion targetRotation = Quaternion.Euler(noiseX, noiseY, noiseZ);

        /*if (sprinting) {
            //targetRotation = Quaternion.Euler(20f, -30f + Mathf.Sin(Time.time * 10f) * 15f, -10f);
            float runningTime = Time.time * 20f;
            targetRotation = Quaternion.Euler(SineRange(runningTime, 20f, 10f), SineRange(runningTime, -30f, -15f), SineRange(runningTime, -10f, 5f));
            targetRotation *= Quaternion.Euler(noiseX, noiseY, noiseZ);
            // from 20f, -30f, -10f
            // to 10f, -15f, -5f
        }*/

        float angle = Quaternion.Angle(transform.localRotation, targetRotation);

        Vector3 targetPosition = Vector3.zero;
        targetPosition -= new Vector3(Vector3.Dot(velocity, transform.right), 0f, Vector3.Dot(velocity, transform.forward)) * 0.001f;

        float rotationSpeedMultiplier = 1f;

        if (aiming) {
            targetPosition = new Vector3(-0.01997f, 0f, -0.0297f);
            targetRotation = Quaternion.Euler(noiseX / 5f, noiseY / 5f, noiseZ / 5f);
            rotationSpeedMultiplier = 3f;
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 5f * Time.deltaTime * angle / 10f * rotationSpeedMultiplier);
        transform.localPosition = Vector3.Slerp(transform.localPosition, targetPosition, 10f * Time.deltaTime);
    }
    
    float SineRange(float time, float min, float max) {
        float normalized = ( Mathf.Sin(time) + 1f ) / 2f;
        return normalized * ( max - min ) + min;
    }

    public void EnableWeapon(bool enabled) {
        meshRend.enabled = enabled;
    }

}
