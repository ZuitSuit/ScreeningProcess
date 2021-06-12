using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControllerPanel : MonoBehaviour, IInteractable {

    public Cinemachine.CinemachineVirtualCamera vCam;

    bool interacting;

    Canvas canvas;

    public RawImage cameraImagePreview;

    public CameraSystem[] cameraSystems;

    int currentCameraIndex = 0;

    void Start() {
        canvas = GetComponentInChildren<Canvas>();

        canvas.worldCamera = Camera.main;

        foreach (CameraSystem cameraSystem in cameraSystems) {
            cameraSystem.controllableCamera.enabled = false;
        }

        SetCameraPreview(0);
    }

    public void SetCameraPreview(int index) {
        cameraImagePreview.texture = cameraSystems[index].screen.GetRenderTexture();
        currentCameraIndex = index;
    }

    public string GetDescription() {
        if (interacting) return "Leave Camera Controller Software"; 
        return "Use Camera Controller Software";
    }

    public void Interact() {
        interacting = !interacting;

        SetCameraPreview(0);

        if (interacting) {
            for (int i = 0; i < cameraSystems.Length; i++) {
                cameraSystems[i].controllableCamera.enabled = i == currentCameraIndex;
            }
        } else {
            foreach (CameraSystem cameraSystem in cameraSystems) {
                cameraSystem.controllableCamera.enabled = false;
            }
        }

        vCam.Priority = interacting ? 250 : 10;

        PlayerController.instance.LockCursor(!interacting);
    }


    [System.Serializable]
    public class CameraSystem {
        public CameraScreen screen;
        public ControllableCamera controllableCamera;
    }
}
