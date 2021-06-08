using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControllerPanel : MonoBehaviour, IInteractable {

    public Cinemachine.CinemachineVirtualCamera vCam;

    bool interacting;

    Canvas canvas;

    public CameraScreen[] controllableCameras;
    public RawImage cameraImagePreview;

    void Start() {
        canvas = GetComponentInChildren<Canvas>();

        canvas.worldCamera = Camera.main;
    }

    public void SetCameraPreview(int index) {
        cameraImagePreview.texture = controllableCameras[index].GetRenderTexture();
    }

    public string GetDescription() {
        if (interacting) return "Leave Camera Controller Software"; 
        return "Use Camera Controller Software";
    }

    public void Interact() {
        interacting = !interacting;

        SetCameraPreview(0);

        vCam.Priority = interacting ? 250 : 10;

        PlayerController.instance.LockCursor(!interacting);
    }
}
