using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour {

    Camera mainCam;
    public float interactionDistance = 2f;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    public GameObject hidingUI;

    [Header("Notes")]
    public GameObject noteUI;
    public TextMeshProUGUI noteText;
    public AudioSource noteSound;

    public static PlayerInteraction instance;

    public float noteTime;

    void Start() {
        instance = this;

        mainCam = Camera.main;
    }

    public void ToggleHidingUI(bool state) {
        hidingUI.SetActive(state);
    }

    public void ReadNote(string note) {
        noteUI.SetActive(true);
        noteText.text = note;
        noteSound.Play();

        noteTime = Time.time;
    }

    private void Update() {
        InteractionRay();

        if (Input.anyKeyDown && Time.time - noteTime > 0.5f) {
            noteUI.SetActive(false);
        }
    }

    void InteractionRay() {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one/2f);
        RaycastHit hit;

        bool hitSomething = false;

        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null) {
                hitSomething = true;
                interactionText.text = interactable.GetDescription();

                if (Input.GetKeyDown(KeyCode.E)) {
                    interactable.Interact();
                }
            }
        }

        interactionUI.SetActive(hitSomething);
    }
}
