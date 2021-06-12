using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchOpener : MonoBehaviour
{
    public Transform rotationJoint;
    public void Open()
    {
        LeanTween.rotateX(rotationJoint.gameObject, -90f, 1f);
    }
}
