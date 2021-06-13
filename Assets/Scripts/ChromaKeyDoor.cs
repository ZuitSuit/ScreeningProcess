using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaKeyDoor : MonoBehaviour, IInteractable
{
    public string GetDescription(){
        int keys = PlayerController.instance.keycardPieces;

        string keyString = keys == 1 ? "key" : "keys";

        if (keys < 4) {
            return $"You need {4 - keys} more {keyString} to open";
        }
        return "Open";
    }

    public void Interact()
    {
        if (PlayerController.instance.keycardPieces >= 4)
        {
            gameObject.SetActive(false);
        }
    }
    
}
