using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableButton : MonoBehaviour
{
    public GameObject glassObject;
    bool open = false;
    Vector3 startingPosition, targetPosition;

    private void Start()
    {
        startingPosition = glassObject.transform.position;
        targetPosition = new Vector3(startingPosition.x, startingPosition.y - 10, startingPosition.z);

        glassObject.isStatic = false;
    }

    public void ToggleGlass()
    {
        open = !open;
        LeanTween.move(glassObject, open ? targetPosition : startingPosition, .9f).setEaseInOutSine();
    }
}
