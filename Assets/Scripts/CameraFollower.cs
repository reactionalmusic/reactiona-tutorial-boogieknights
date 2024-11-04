using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform player;      // Reference to the player's transform
    public float gridSize = 1f;   // The size of each grid cell in world units
    public int cameraOffset = 10; // Number of grid spaces the camera moves when the player goes too far

    private Vector3 cameraTargetPosition; // The target position for the camera

    void Start()
    {
        // Initialize the camera's target position to the player's current position (rounded to the grid)
        cameraTargetPosition = GetGridPosition(player.position);
        transform.position = cameraTargetPosition;
    }

    void Update()
    {
        // Check if the player has moved outside of the current camera view (centered zone)
        Vector3 playerGridPosition = GetGridPosition(player.position);

        if (playerGridPosition.x > cameraTargetPosition.x + (cameraOffset / 2) * gridSize)
        {
            MoveCamera(Vector3.right); // Move camera 10 grids to the right
        }
        else if (playerGridPosition.x < cameraTargetPosition.x - (cameraOffset / 2) * gridSize)
        {
            MoveCamera(Vector3.left);  // Move camera 10 grids to the left
        }

        if (playerGridPosition.y > cameraTargetPosition.y + (cameraOffset / 2) * gridSize)
        {
            MoveCamera(Vector3.up);    // Move camera 10 grids up
        }
        else if (playerGridPosition.y < cameraTargetPosition.y - (cameraOffset / 2) * gridSize)
        {
            MoveCamera(Vector3.down);  // Move camera 10 grids down
        }
    }

    // Moves the camera by 10 grid spaces in the given direction
    void MoveCamera(Vector3 direction)
    {
        cameraTargetPosition += direction * cameraOffset * gridSize;
        transform.position = cameraTargetPosition;
    }

    // Helper function to get the grid-aligned position based on the grid size
    Vector3 GetGridPosition(Vector3 worldPosition)
    {
        return new Vector3(
            Mathf.Floor(worldPosition.x / gridSize) * gridSize,
            Mathf.Floor(worldPosition.y / gridSize) * gridSize,
            transform.position.z // Keep the camera's original Z position
        );
    }
}
