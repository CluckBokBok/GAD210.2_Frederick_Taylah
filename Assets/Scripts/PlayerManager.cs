using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private float playerHealth = 100f;
    [SerializeField] private float maxHealth = 100f;

    public void TakeDamage(float amount)
    {
        playerHealth -= amount;
        Debug.Log($"Player took {amount} damage. Current health: {playerHealth}");

        if (playerHealth <= 0)
        {
            Debug.Log("Player has been defeated!");
            // Handle game over logic here
        }
    }
    public void ApplyHealing(float amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Min(playerHealth, 100f); // Cap at max health
        Debug.Log($"Player healed by {amount}. Current health: {playerHealth}");
    }

    public float GetHealth()
    {
        return playerHealth;
    }

}
