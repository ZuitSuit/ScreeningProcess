using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour, IInteractable
{
    bool spawnsActive = false;
    float timer = 0f;
    public Transform waveParent;
    List<Transform> waves = new List<Transform>();
    int currentWave = -1;
    public HatchOpener hatch;
    private void Start()
    {

        foreach (Transform child in waveParent)
        {
            waves.Add(child);
        }
    }

    public void Interact()
    {
        if(!spawnsActive) StartSpawn();
    }

    public string GetDescription()
    {
        return spawnsActive ? "hatch opening...": "override hatch controls";
    }

    void StartSpawn()
    {
        spawnsActive = true;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (spawnsActive && timer > 10f)
        {
            currentWave++;
            if(currentWave >= waves.Count)
            {
                hatch.Open();
                enabled = false;
                return;
            }

            timer = 0f;
            waves[currentWave].gameObject.SetActive(true);
        }
    }
}
