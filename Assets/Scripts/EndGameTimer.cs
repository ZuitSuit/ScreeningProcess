using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndGameTimer : MonoBehaviour
{
    public TextMeshProUGUI timer;

    private void Update()
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        timer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
    }
}
