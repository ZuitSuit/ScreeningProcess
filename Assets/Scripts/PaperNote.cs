using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperNote : MonoBehaviour, IInteractable {

    [TextArea(10, 25)]
    public string note;

    public string GetDescription() {
        return "Read note";
    }

    public void Interact() {
        PlayerInteraction.instance.ReadNote(note);
    }

}
