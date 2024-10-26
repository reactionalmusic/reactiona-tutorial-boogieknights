using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<SpriteRenderer> hearts;
    
    public void UpdateHealth(int health)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < health)
            {
                hearts[i].color = Color.white;
            }
            else
            {
                hearts[i].color = Color.black;
            }
        }
    }
}
