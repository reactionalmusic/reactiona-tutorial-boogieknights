using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {            
            other.gameObject.GetComponent<Knight>().SwitchWeapon();
            Destroy(gameObject);
        }
    }
}
