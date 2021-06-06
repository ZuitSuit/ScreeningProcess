using GD.MinMaxSlider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableCamera : MonoBehaviour
{

    [MinMaxSlider(-180f, 180f)]
    public Vector2 vertical = new Vector2(-30f, 30f);
    [MinMaxSlider(-180f, 180f)]
    public Vector2 horizontal = new Vector2(-30f, 30f);

    void Update()
    {
        
    }
}
