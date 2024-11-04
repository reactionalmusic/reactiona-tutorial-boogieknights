using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject destination;
    public Vector2 offset;

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Player")
            StartCoroutine(TeleportPlayer(other.gameObject));
    }
    IEnumerator TeleportPlayer(GameObject player) {
        player.GetComponent<Knight>().blockInput = true;
        player.GetComponent<Knight>().StopCoroutine("SmoothMove");
        yield return new WaitForSeconds(0.25f);
        player.transform.position = destination.transform.position + new Vector3(offset.x, offset.y, 0);
        player.GetComponent<Knight>().gridPosition = Vector3Int.RoundToInt(player.transform.position);
        yield return new WaitForSeconds(0.25f);
        player.GetComponent<Knight>().blockInput = false;
    }
}
