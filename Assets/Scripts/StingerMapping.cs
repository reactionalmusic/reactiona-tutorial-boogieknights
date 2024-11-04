using UnityEngine;

public class StingerMapping : MonoBehaviour
{
    public static StingerMapping Instance { get; private set; }

    [SerializeField]
    private string moveStinger;
    [SerializeField]
    private string attackStinger;
    [SerializeField]
    private string damageStinger;

    public static string MoveStinger { get; private set; }
    public static string AttackStinger { get; private set; }
    public static string DamageStinger { get; private set; }

    void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the singleton alive across scenes

            // Initialize static fields with serialized values
            MoveStinger = moveStinger;
            AttackStinger = attackStinger;
            DamageStinger = damageStinger;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
