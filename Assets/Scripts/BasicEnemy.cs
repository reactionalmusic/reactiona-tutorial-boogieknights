using System.Collections;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public Transform player;            // Reference to the player
    public float moveInterval = 0.5f;   // Time between each movement
    public int detectionRange = 5;      // How many grid spaces away the enemy can "see" the player

    private Vector3Int enemyPosition;   // Enemy's position on the grid
    private Vector3Int playerPosition;  // Player's position on the grid

    public int health = 3;

    public bool hasKnockBack = true;
    public AudioClip hitSound;
    public AudioClip deathSound;
    AudioSource audioSource;
    private float nextBeat = 1f;

    bool knockedBack = false;
    bool takingDamage = false;

    void Start()
    {
        // Initialize enemy's grid position
        enemyPosition = Vector3Int.RoundToInt(transform.position);
        transform.position = enemyPosition;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (knockedBack)
        {
            knockedBack = false;
            return;
        }
        var currentBeat = Reactional.Playback.MusicSystem.GetCurrentBeat();
        if (currentBeat < 1f)
        {
            nextBeat = 1f;
        }
        if (currentBeat >= nextBeat)
        {
            nextBeat = Reactional.Playback.MusicSystem.GetNextBeat(moveInterval, -0.125f);
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Get player and enemy grid positions
        playerPosition = Vector3Int.RoundToInt(player.position);

        // Calculate the distance to the player
        int distanceToPlayer = Mathf.Abs(playerPosition.x - enemyPosition.x) + Mathf.Abs(playerPosition.y - enemyPosition.y);

        // Check if the player is within the detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Determine movement direction towards the player
            Vector3Int direction = Vector3Int.zero;

            if (playerPosition.x > enemyPosition.x)     // Move right
            {
                direction = new Vector3Int(1, 0, 0);
            }
            else if (playerPosition.x < enemyPosition.x) // Move left
            {
                direction = new Vector3Int(-1, 0, 0);
            }
            else if (playerPosition.y > enemyPosition.y) // Move up
            {
                direction = new Vector3Int(0, 1, 0);
            }
            else if (playerPosition.y < enemyPosition.y) // Move down
            {
                direction = new Vector3Int(0, -1, 0);
            }

            if (takingDamage)
                return;
            StartCoroutine(SmoothMove(direction));

        }
    }

    IEnumerator SmoothMove(Vector3Int direction)
    {
        Vector3Int targetPosition = enemyPosition + direction;
        float t = 0f;
        float duration = 0.05f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(enemyPosition, targetPosition, t);
            yield return null;
        }
        transform.position = targetPosition;

        if (player.position == transform.position)
        {
            player.GetComponent<Knight>().TakeDamage();
            // Move back to the previous position
            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(targetPosition, enemyPosition, t);
                yield return null;
            }
            transform.position = enemyPosition;
            targetPosition = enemyPosition;
        }

        enemyPosition = targetPosition;
    }
    public void TakeDamage()
    {
        StartCoroutine(TakeDamageCoroutine());
    }
    public IEnumerator TakeDamageCoroutine()
    {
        takingDamage = true;
        health--;
        audioSource.clip = hitSound;
        Reactional.Playback.MusicSystem.ScheduleAudio(audioSource, 1f);
        Vector3Int direction = playerPosition - enemyPosition;
        knockedBack = true;
        StartCoroutine(Knockback(direction * -1));
        if (health <= 0)
        {
            audioSource.PlayOneShot(deathSound);
            Destroy(gameObject, 0.25f);
        }
        // Flash the enemy red
        int repeats = 1;
        float t = 0f;
        while (t < repeats)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            t += 1;
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        takingDamage = false;
    }

    IEnumerator Knockback(Vector3Int direction)
    {
        Reactional.Playback.Theme.TriggerStinger(StingerMapping.AttackStinger, 0.125f);
        if (!hasKnockBack)
            yield break;
        float duration = 0.1f;
        float t = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + direction;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, t / duration);
            yield return null;
        }
        enemyPosition = Vector3Int.RoundToInt(endPosition);
        transform.position = endPosition;
    }
}
