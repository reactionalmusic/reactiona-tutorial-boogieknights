using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{

    [HideInInspector]
    public Vector3Int gridPosition; // Position on the grid

    [Header("Weapon Settings")]
    public GameObject equippedWeapon;
    public List<GameObject> weapons;
    public GameObject weaponHolder;
    private Quaternion swordStartRotation;
    private bool isAttacking = false;

    [Header("Audio Settings")]
    public List<AudioClip> swordSounds;
    public List<AudioClip> hitSounds;
    public List<AudioClip> walkSounds;
    private AudioSource audioSource;

    [Header("Health Settings")]
    public int health = 4;
    private bool invulnerable = false;
    private bool isDead = false;

    [Header("Game Settings")]
    public GameObject gameOverScreen;
    [HideInInspector]
    public bool blockInput = false;

    private float nextBeat = 1f;

    private void Start()
    {
        // Initialize the knight's position on the grid (rounding to grid)
        gridPosition = Vector3Int.RoundToInt(transform.position);
        transform.position = gridPosition;
        swordStartRotation = equippedWeapon.transform.rotation;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        var currentBeat = Reactional.Playback.MusicSystem.GetCurrentBeat();
        if (currentBeat < 1f)
        {
            nextBeat = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Swing sword
        {
            if (isDead)
            {
                // Restart the game
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }

            StartCoroutine(SwingSword());
        }

        if (blockInput)
            return;
        // Check direction to rotate sword
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  // Move Left
        {
            weaponHolder.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // Move Right
        {
            weaponHolder.transform.localScale = new Vector3(1, 1, 1);
        }

        // Check if it's time to move
        if (currentBeat >= nextBeat)
        {
            nextBeat = Reactional.Playback.MusicSystem.GetNextBeat(1f, -0.25f);
            Vector3Int direction = Vector3Int.zero;

            // Get player input for movement direction
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))     // Move Up
            {
                direction = new Vector3Int(0, 1, 0);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  // Move Down
            {
                direction = new Vector3Int(0, -1, 0);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  // Move Left
            {
                direction = new Vector3Int(-1, 0, 0);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // Move Right
            {
                direction = new Vector3Int(1, 0, 0);
            }

            // If there's a valid input direction, move the knight
            if (direction != Vector3Int.zero)
            {
                MoveKnight(direction);
            }
        }
    }

    public void SwitchWeapon()
    {
        // Switch to the next weapon in the list
        int currentIndex = weapons.IndexOf(equippedWeapon);
        int nextIndex = (currentIndex + 1) % weapons.Count;
        equippedWeapon = weapons[nextIndex];
        equippedWeapon.SetActive(true);
        foreach (GameObject weapon in weapons)
        {
            if (weapon != equippedWeapon)
            {
                weapon.SetActive(false);
            }
        }
    }
    void MoveKnight(Vector3Int direction)
    {
        // Calculate the new grid position
        Vector3Int newGridPosition = gridPosition + direction;
        // Define the size of the box for collision detection
        Vector2 boxSize = new Vector2(0.8f, 0.8f); // Adjust size as needed to match the grid size

        // Check if the new position collides with any colliders on the "Wall" layer
        Vector3 boxCenter = new Vector3(newGridPosition.x + 0.5f, newGridPosition.y - 0.5f, 0f);

        if (Physics2D.OverlapBox(boxCenter, boxSize, 0f, LayerMask.GetMask("Wall")))
        {
            return;
        }
        // Update the knight's grid position
        gridPosition = newGridPosition;
        StartCoroutine(SmoothMove(gridPosition));
        Reactional.Playback.Theme.TriggerStinger(StingerMapping.MoveStinger, 0.125f);
        audioSource.clip = walkSounds[Random.Range(0, walkSounds.Count)];
        Reactional.Playback.MusicSystem.ScheduleAudio(audioSource, 1f);

    }

    IEnumerator SmoothMove(Vector3Int endPosition)
    {
        float duration = 0.05f;
        float t = 0f;
        Vector3 startPosition = transform.position;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, t / duration);
            yield return null;
        }

        transform.position = endPosition;
    }

    IEnumerator SwingSword()
    {
        if (isAttacking)
            yield break;
        isAttacking = true;
        yield return new Reactional.Playback.MusicSystem.WaitForNextBeat(0.5f, -0.1f);
        CheckAttack();
        // rotate Z axis 90 degrees and back
        float duration = 0.1f;
        float t = 0f;
        // start rotation 
        equippedWeapon.transform.rotation = swordStartRotation;
        Reactional.Playback.MusicSystem.ScheduleAudio(equippedWeapon.GetComponent<AudioSource>(), 0.25f);
        while (t < duration)
        {
            t += Time.deltaTime;
            equippedWeapon.transform.Rotate(0, 0, -90 * Time.deltaTime / duration);
            yield return null;
        }
        // reset rotation
        equippedWeapon.transform.rotation = swordStartRotation;
        isAttacking = false;
    }

    void CheckAttack()
    {
        // Check if the sword is colliding with an enemy
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(equippedWeapon.transform.position, 1f);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null && enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<BasicEnemy>().TakeDamage();
            }
        }
    }

    public void TakeDamage()
    {
        if (invulnerable)
            return;
        StartCoroutine(TakeDamageCoroutine());
    }
    public IEnumerator TakeDamageCoroutine()
    {
        invulnerable = true;
        health--;
        GetComponent<Healthbar>().UpdateHealth(health);
        if (health <= 0)
        {
            // Game over
            Debug.Log("Game Over");
            Reactional.Playback.Theme.TriggerStinger(StingerMapping.DamageStinger, 0.125f);
            StartCoroutine(GameOver());
            yield break;
        }
        audioSource.clip = hitSounds[Random.Range(0, hitSounds.Count)];
        Reactional.Playback.MusicSystem.ScheduleAudio(audioSource, 0.5f);
        Reactional.Playback.Theme.TriggerStinger(StingerMapping.DamageStinger, 0.125f);
        // Flash the knight red
        int repeats = 4;
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
        invulnerable = false;
    }

    IEnumerator GameOver()
    {
        // fade in game over screen
        float duration = 1f;
        float t = 0f;
        Color startColor = gameOverScreen.GetComponent<SpriteRenderer>().color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        while (t < duration)
        {
            t += Time.deltaTime;
            gameOverScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, endColor, t / duration);
            yield return null;
        }

        blockInput = true;
        yield return new WaitForSeconds(4f);
        isDead = true;
    }


}
