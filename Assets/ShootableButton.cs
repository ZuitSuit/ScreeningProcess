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
    }
    private void OnCollisionEnter(Collision other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            ToggleGlass(!open);
        }
    }

    public void ToggleGlass(bool toggle)
    {
        open = toggle;
        LeanTween.move(glassObject, open ? startingPosition : targetPosition, .9f);
    }
}
