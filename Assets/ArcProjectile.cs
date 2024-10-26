using UnityEngine;
using System.Collections;

public class ArcProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;    // The object to spawn (e.g., a bomb or arrow)
    public Transform spawnPoint;           // The point where the projectile is spawned
    public Vector2 targetPosition;         // The target position in 2D space where the projectile will go
    public float speed = 5f;               // The speed at which the projectile moves
    public float arcHeight = 2f;           // The height of the fake arc
    float nextBeat = 1f;
    public GameObject player;
    private bool isFired = false;

    public int scanRange = 5;
    public float beatWait = 3f;

    public AudioClip fireSound;
    public AudioClip landSound;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // check distance to player
        if (Vector2.Distance(player.transform.position, transform.position) > scanRange)
            return;
        // Fire the projectile when the space key is pressed

        var currBeat = Reactional.Playback.MusicSystem.GetCurrentBeat();
        if (currBeat >= nextBeat && !isFired)
            FireProjectile();

        if (Input.GetKeyDown(KeyCode.Space) && !isFired)
        {
        
        }
    }

    // Method to spawn and fire the projectile
    void FireProjectile()
    {
        
        // Spawn the projectile at the spawn point's position
        targetPosition = player.transform.position + new Vector3(0.5f, -0.5f, 0);
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity, null);
        isFired = true;

        // Start the coroutine to move the projectile in an arc
        StartCoroutine(MoveProjectileInArc(projectile, spawnPoint.position, targetPosition));
    }

    // Coroutine to move the projectile in a fake arc over time
    IEnumerator MoveProjectileInArc(GameObject projectile, Vector2 startPosition, Vector2 endPosition)
    {
        audioSource.clip = fireSound;
        Reactional.Playback.MusicSystem.ScheduleAudio(audioSource,1f);
        yield return new Reactional.Playback.MusicSystem.WaitForNextBeat(1f);
        nextBeat = Reactional.Playback.MusicSystem.GetNextBeat(beatWait);
        float journeyProgress = 0f;  // Track the progress of the projectile's movement (0 to 1)
        float duration = Vector2.Distance(startPosition, endPosition) / speed;  // Total time for the projectile to reach the target

        var startBeat = Reactional.Playback.MusicSystem.GetCurrentBeat();
        while (journeyProgress <= 1.0f)
        {
            journeyProgress = Reactional.Playback.MusicSystem.GetCurrentBeat() - 0.5f - startBeat;            
            // Lerp the X and Y positions between the start and end positions
            Vector2 currentPosition = Vector2.Lerp(startPosition, endPosition, journeyProgress);

            // Apply a fake arc using a sine wave for the Y-axis
            float arc = Mathf.Sin(Mathf.PI * Mathf.Clamp(journeyProgress,0f,1f)) * arcHeight;
            currentPosition.y += arc;

            // Update the projectile's position
            projectile.transform.position = currentPosition;

            yield return null;  // Wait for the next frame
        }
        audioSource.clip = landSound;
        Reactional.Playback.MusicSystem.ScheduleAudio(audioSource, 1f);
        // Ensure the projectile reaches the exact target position at the end
        projectile.transform.position = endPosition;

        if ((Vector2)player.transform.position + new Vector2(0.5f, -0.5f) == endPosition)
        {
            if (player.GetComponent<Knight>() != null)
                player.GetComponent<Knight>().TakeDamage();
        }

        // Destroy or handle the projectile when it reaches the target
        isFired = false;

        //fade out color
        SpriteRenderer sr = projectile.GetComponent<SpriteRenderer>();
        Color c = sr.color;
        while (c.a > 0)
        {
            c.a -= 0.1f;
            sr.color = c;
            yield return new WaitForSeconds(0.1f);
        }

        // Destroy the projectile after a delay
        Destroy(projectile);
    }
}
