using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaKey : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PlayerController.instance.CollectKeyCard();
        gameObject.SetActive(false);
    }

    public string GetDescription()
    {
        return "Collect Chroma Key";
    }
}
