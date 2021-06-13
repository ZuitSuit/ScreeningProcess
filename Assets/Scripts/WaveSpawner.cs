using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour, IInteractable
{
    bool spawnsActive = false;
    float timer = 0f;
    public Transform waveParent;
    List<Transform> waves = new List<Transform>();
    int currentWave = -1;
    public HatchOpener hatch;
    float totalTime;

    public TextMeshProUGUI timerText;

    public AudioSource drumsAudio;

    private void Start()
    {

        foreach (Transform child in waveParent)
        {
            waves.Add(child);
        }

        totalTime = (waves.Count+1) * 10f;
    }

    public void Interact()
    {
        if(!spawnsActive) StartSpawn();
    }

    public string GetDescription()
    {
        return spawnsActive ? "Hatch Opening...": "Override Hatch Controls";
    }

    void StartSpawn()
    {
        spawnsActive = true;
        timer = 0f;
        drumsAudio.Play();
    }

    private void Update()
    {
        if (!spawnsActive) return;

        totalTime -= Time.deltaTime;
        
        timerText.text = string.Format("{0:D2}:{1:D2}", (int)totalTime/60, (int)totalTime %60);

        timer += Time.deltaTime;
        if (timer > 10f)
        {
            currentWave++;
            if(currentWave >= waves.Count)
            {
                hatch.Open();
                timerText.color = Color.green;
                timerText.text = "Open";
                enabled = false;

                return;
            }

            timer = 0f;
            waves[currentWave].gameObject.SetActive(true);
        }
    }
}
