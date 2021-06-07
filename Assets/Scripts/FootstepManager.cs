using UnityEngine;

public class FootstepManager : MonoBehaviour {
    public AudioSource audioSource;

    public AudioClip[] dirtyGround;
    public AudioClip[] grass;
    public AudioClip[] gravel;
    public AudioClip[] metal;
    public AudioClip[] rock;
    public AudioClip[] tile;
    public AudioClip[] wood;


    AudioClip[] clipsToUse;

    public LayerMask layerMask;

    public enum Location {
        DirtyGround,
        Grass,
        Gravel,
        Metal,
        Rock,
        Tile,
        Wood
    }

    private void Start() {
        ChangeLocation(Location.Rock);
    }

    void PlayFootstepSound(AnimationEvent evt) {
        if (evt.animatorClipInfo.weight < 0.5f) return;
        PlayFootstepSound();
    }

    public void PlayFootstepSound() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f, layerMask)) {
            GroundIdentifier groundIdentifier = hit.collider.GetComponent<GroundIdentifier>();
            if (groundIdentifier != null) {
                ChangeLocation(groundIdentifier.location);
            }
        }

        AudioClip clip = clipsToUse[Random.Range(0, clipsToUse.Length)];
        audioSource.PlayOneShot(clip);
    }

    public void ChangeLocation(Location location) {
        switch (location) {
            case Location.DirtyGround:
                clipsToUse = dirtyGround;
                break;
            case Location.Grass:
                clipsToUse = grass;
                break;
            case Location.Gravel:
                clipsToUse = gravel;
                break;
            case Location.Metal:
                clipsToUse = metal;
                break;
            case Location.Rock:
                clipsToUse = rock;
                break;
            case Location.Tile:
                clipsToUse = tile;
                break;
            case Location.Wood:
                clipsToUse = wood;
                break;
            default:
                throw new System.Exception("Invalid location.");
                break;
        }
    }

    
}
