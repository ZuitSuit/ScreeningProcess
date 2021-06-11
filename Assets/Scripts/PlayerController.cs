using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float crouchSpeed;
    public float walkingSpeed;
    public float runningSpeed;
    public float gravityScale;
    public Transform playerT;

    Rigidbody rb;

    public Cinemachine.CinemachineVirtualCamera cam;
    public Cinemachine.CinemachineVirtualCamera crouchCam;

    float cameraXRotation;
    public Vector2 cameraClamp;
    public float mouseSens = 2f;

    public float footstepInterval = 5f;
    public FootstepManager footstepManager;

    public static PlayerController instance;

    bool canMove;

    float footstepTime;

    CapsuleCollider collider;

    public static bool isDead;

    bool isCrouching;

    PlayerSprintSystem playerSprintSystem;

    public WeaponSway weaponSway;

    private void Start() {
        rb = GetComponent<Rigidbody>();

        collider = GetComponent<CapsuleCollider>();

        playerSprintSystem = GetComponent<PlayerSprintSystem>();

        LockCursor(true);

        instance = this;
        canMove = true;
    }

    public void LockCursor(bool newState) {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !newState;

        ToggleMovement(newState);
    }
    
    public void ToggleMovement(bool newState) {
        canMove = newState;

        if (!canMove) rb.velocity = Vector3.zero;
    }

    private void Update() {
        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, IsRunning() ? 75f : 60f, 10f * Time.deltaTime);

        collider.height = Mathf.Lerp(collider.height, isCrouching ? 0.5f : 1.8f, 10f * Time.deltaTime);
        crouchCam.Priority = isCrouching ? 250 : 0;

        isCrouching = IsCrouching();

        if (canMove) {
            cameraController();
        }
    }

    private void FixedUpdate() {
        if (canMove) movementController();
    }

    private void cameraController() {
        Vector2 mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSens;
        
        playerT.Rotate(Vector3.up, mouse.x, Space.World);

        cameraXRotation -= mouse.y;
        cameraXRotation = Mathf.Clamp(cameraXRotation, cameraClamp.x, cameraClamp.y);

        cam.transform.eulerAngles = new Vector3(cameraXRotation, cam.transform.eulerAngles.y, 0f);
        crouchCam.transform.eulerAngles = cam.transform.eulerAngles;
    }

    public void MoveTo(Transform pos) {
        transform.position = pos.position;
    }

    private void movementController() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // do this before applying velocity, that way the ACTUAL velocity of the player is used and not the desired velocity
        ManageFootsteps();

        float gravity = rb.velocity.y + Physics.gravity.y * gravityScale * Time.fixedDeltaTime;

        if (IsGrounded() && rb.velocity.y < 0f) {
            gravity = 0f;
        }

        float speed = walkingSpeed;
        if (IsRunning()) {
            speed = runningSpeed;
            playerSprintSystem.Sprinting();
        } else if (isCrouching) {
            speed = crouchSpeed;
        }

        Vector3 direction = cam.transform.forward * input.y + cam.transform.right * input.x;
        direction.y = 0f;
        direction = direction.normalized * speed;

        direction *= transform.localScale.magnitude;

        rb.velocity = direction + Vector3.up * gravity;

        weaponSway.Sway(rb.velocity, false);
    }

    void ManageFootsteps() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector3 velocity = rb.velocity;
        velocity.y = 0f; // no footstep sounds in the air

        footstepTime += velocity.magnitude * Time.deltaTime;
        if (IsRunning()) footstepTime += input.magnitude * Time.deltaTime;

        if (footstepTime >= footstepInterval) {
            footstepTime = 0f;
            footstepManager.PlayFootstepSound();
            //footstepAudioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
        }
    }

    public void ToggleHiding(bool isHiding) {
        if (isHiding) {
            collider.enabled = false;
            rb.isKinematic = true;
        } else {
            collider.enabled = true;
            rb.isKinematic = false;
        }
    }

    public void Die() {
        if (isDead) return;

        isDead = true;

        LockCursor(false);

        canMove = false;

        collider.material = null;

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        rb.AddForce(Random.onUnitSphere * 1000f);
    }

    bool IsRunning() {
        return Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0f && playerSprintSystem.CanSprint();
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, collider.bounds.extents.y + 0.1f, 1);
    }

    bool IsCrouching() {
        return Input.GetKey(KeyCode.LeftControl) || Physics.Raycast(transform.position, Vector3.up, 1f);
    }
}
