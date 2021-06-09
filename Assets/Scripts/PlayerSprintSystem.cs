using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSprintSystem : MonoBehaviour {

    public float maxStamina; // maximum amount of stamina we can have
    public float staminaRegenDelay; // how long it takes to regen after we stop running
    public float staminaRegenSpeed; // base speed for regen
    public float staminaRegenAcceleration; // how quickly we speed up the regen speed
    public float staminaUsageRate; // how quickly we use up sprinting when running

    public Image sprintBar;

    public Transform sprintUI;

    float sprint;
    float sprintFillRate;
    float lastSprintTime;

    private void Start() {
        sprint = maxStamina;
    }

    private void Update() {
        if (Time.time - lastSprintTime > staminaRegenDelay) {
            sprint += sprintFillRate * Time.deltaTime;
            sprintFillRate += staminaRegenAcceleration * Time.deltaTime;
            sprint = Mathf.Clamp(sprint, 0f, maxStamina);
            sprintBar.fillAmount = sprint / maxStamina;
        }

        Vector3 targetScale = sprint < maxStamina ? Vector3.one : Vector3.zero;

        sprintUI.localScale = Vector3.Lerp(sprintUI.localScale, targetScale, 10f * Time.deltaTime);
    }

    public bool CanSprint() {
        return sprint > 0f;
    }

    public void Sprinting() {
        lastSprintTime = Time.time;
        sprintFillRate = staminaRegenSpeed;
        sprint -= staminaUsageRate * Time.deltaTime;

        sprintBar.fillAmount = sprint / maxStamina;
    }
}
