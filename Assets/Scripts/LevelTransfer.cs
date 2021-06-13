using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransfer : MonoBehaviour
{
    public Transform hide, show;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            hide.gameObject.SetActive(false);
            show.gameObject.SetActive(true);
        }
    }
}
