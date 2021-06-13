using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaKeyDoor : MonoBehaviour, IInteractable
{
    public string GetDescription()
    {
        return "open?";
    }

    public void Interact()
    {
        if (PlayerController.instance.keycardPieces >= 3)
        {
            gameObject.SetActive(false);
        }
    }
    
}
