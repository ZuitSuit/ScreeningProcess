using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableCamera : MonoBehaviour
{

    [MinMaxSlider(-180f, 180f)]
    public Vector2 topDownClamp = new Vector2(0f, 30f);
    [MinMaxSlider(-180f, 180f)]
    public Vector2 leftRightClamp = new Vector2(-30f, 30f);

    Vector3 startingRotation, currentRotation;
    float horizontalMovement, verticalMovement;
    public float movementSpeed = 20f;
    public Transform rotationJoint;

    private void Awake()
    {
        currentRotation = startingRotation = rotationJoint.transform.localRotation.eulerAngles;
    }


    private void Update()
    {
        //horizontalMovement = Time.deltaTime * (Input.GetKey(KeyCode.LeftArrow) ? -movementSpeed : Input.GetKey(KeyCode.RightArrow) ? movementSpeed : 0f);
        //verticalMovement = Time.deltaTime * (Input.GetKey(KeyCode.UpArrow) ? -movementSpeed : Input.GetKey(KeyCode.DownArrow) ? movementSpeed : 0f);
        horizontalMovement = Input.GetAxisRaw("Horizontal") * Time.deltaTime * movementSpeed;
        verticalMovement = -Input.GetAxisRaw("Vertical") * Time.deltaTime * movementSpeed;

        currentRotation.y = Mathf.Clamp(currentRotation.y + horizontalMovement, leftRightClamp.x, leftRightClamp.y);
        currentRotation.x = Mathf.Clamp(currentRotation.x + verticalMovement, topDownClamp.x, topDownClamp.y);

        rotationJoint.transform.rotation = Quaternion.Euler(currentRotation);
    }

}
