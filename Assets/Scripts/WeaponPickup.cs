using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {            
            other.gameObject.GetComponent<Knight>().SwitchWeapon();
            Destroy(gameObject);
        }
    }
}
