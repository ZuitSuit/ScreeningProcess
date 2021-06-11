using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    float targetYRotation;

    public float smooth;
    public bool autoClose;

    Transform player;

    bool isOpen;

    float timer = 0f;
    float defaultYRot = 0f;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        defaultYRot = transform.eulerAngles.y;
    }

    private void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, defaultYRot+targetYRotation, 0f), smooth * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer <= 0f && isOpen && autoClose) {
            ToggleDoor(player.position);
        }
    }

    public void Interact() {
        ToggleDoor(player.position);
    }

    public void ToggleDoor(Vector3 pos) {
        isOpen = !isOpen;

        if (isOpen) {
            Vector3 dir = (pos - transform.position);
            targetYRotation = Mathf.Sign(Vector3.Dot(transform.forward, dir)) * 90f;
            timer = 5f;
        } else {
            targetYRotation = 0f;
        }
    }

    public void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Enemy")) {
            Open(collider.transform.position);
        }
    }

    public void OnTriggerStay(Collider collider) {
        if (collider.CompareTag("Enemy")) {
            Open(collider.transform.position);
        }
    }

    public void Open(Vector3 pos) {
        if (!isOpen) {
            ToggleDoor(pos);
        }
    }
    public void Close(Vector3 pos) {
        if (isOpen) {
            ToggleDoor(pos);
        }
    }

    public string GetDescription() {
        if (isOpen) return "Close the door";
        return "Open the door";
    }
}
