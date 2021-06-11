using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour {

    public Transform[] cells;
    public GameObject wall;

    bool canShatter = true;

    public float force = 10f;

    public void Shatter(Vector3 pos) {
        if (!canShatter) return;
        canShatter = false;
        Destroy(wall);
        Destroy(GetComponent<Collider>());

        foreach (Transform cell in cells) {
            cell.gameObject.SetActive(true);

            float distance = Vector3.Distance(cell.position, pos);
            Vector3 direction = (cell.position - pos) * force / (distance * distance);

            cell.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);

            LeanTween.scale(cell.gameObject, Vector3.zero, 1f).setEaseInOutSine().setDelay(Random.Range(2f, 4f));

            Destroy(cell.gameObject, 5f);
        }

        Destroy(gameObject, 10f);
    }
    
}
