using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
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
