using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour, IInteractable {

    bool isHiding;

    public Cinemachine.CinemachineVirtualCamera vCam;
    Animator anim;

    public Vector2 cameraYClamp;

    Vector3 vCamRotation;

    void Start() {
        anim = GetComponent<Animator>();

        vCamRotation = vCam.transform.localEulerAngles;
    }

    public string GetDescription() {
        if (isHiding) return "Leave";
        return "Hide";
    }

    public void Interact() {
        isHiding = !isHiding;

        PlayerInteraction.instance.ToggleHidingUI(isHiding);

        if (isHiding) {
            anim.SetTrigger("enter");
        } else {
            anim.SetTrigger("leave");
            anim.SetBool("isPeeking", false);
        }

        vCam.Priority = isHiding ? 250 : 0;
    }

    void Update() {
        if (isHiding) {
            anim.SetBool("isPeeking", Input.GetKey(KeyCode.Space));

            if (Input.GetKeyDown(KeyCode.E)) {
                Interact();
            }
        }
    }

    void LateUpdate() {
        if (isHiding) {
            float move = Input.GetAxis("Mouse X");

            vCamRotation.y += move * 180f * Time.deltaTime;
            vCamRotation.y = Mathf.Clamp(vCamRotation.y, cameraYClamp.x, cameraYClamp.y);

            vCam.transform.localEulerAngles = vCamRotation;
        }
    }
}
