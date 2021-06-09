using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour, IInteractable {

    bool isHiding;

    public Cinemachine.CinemachineVirtualCamera vCam;
    Animator anim;

    public Vector2 cameraYClamp;

    Vector3 vCamRotation;

    float interactionDelay = 0f;

    void Start() {
        anim = GetComponent<Animator>();

        vCamRotation = vCam.transform.localEulerAngles;
    }

    public string GetDescription() {
        if (isHiding) return "Leave";
        return "Hide";
    }

    public void Interact() {
        if (interactionDelay > 0f) return;

        interactionDelay = 0.1f;

        isHiding = !isHiding;

        PlayerInteraction.instance.ToggleHidingUI(isHiding);

        if (isHiding) {
            anim.SetTrigger("enter");
            vCam.Priority = 250;
        } else {
            anim.SetTrigger("leave");
            anim.SetBool("isPeeking", false);
            LeanTween.delayedCall(0.1f, () => {
                vCam.Priority = 0;
            });
        }
    }

    void Update() {
        interactionDelay -= Time.deltaTime;

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
