using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace GAD210_Enemy_Base
{
    public class EnemyBase : MonoBehaviour
    {

        public TMP_Text enemyDamageOutput;

        // Calls on GameManager script
        [SerializeField] private GameManager gameManager; // Assign in the Inspector
        
        // Variables for creating the physics overlap fields
        [SerializeField] private float detectionDistance = 5f;
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private LayerMask playerDetection;
        
        // Enemies health variable
        [SerializeField] public float enemyHealth = 100f;
        
        // Enemy damage output variables
        [SerializeField] private float enemyMinDamage = 10f;
        [SerializeField] private float enemyMaxDamage = 25f;
        private float retaliationDamage;

        void Start()
        {
            // Attempt to find GameManager if not manually assigned
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }
        }

        void Update()
        {
            DetectPlayerAndBattle();
        }

        // Applies damage to the enemy based on card outcome (Amount)
        public void ApplyDamage(float amount)
        {
            enemyHealth -= amount;
            if (enemyHealth <= 0)
            {
                Debug.Log("Enemy defeated!");
                enemyHealth = 0; // Ensure it doesn't go negative
            }
            Debug.Log($"Enemy took {amount} damage. Remaining health: {enemyHealth}");
        }

        // Uses a randomr range to determine enemy attack output and applies it to the player
        public void Retaliate(PlayerManager player)
        {
            if (player == null)
            {
                Debug.LogWarning("Player reference is null. Cannot retaliate.");
                return;
            }


            retaliationDamage = Random.Range(enemyMinDamage, enemyMaxDamage);

            Debug.Log($"Enemy retaliates with {retaliationDamage} damage.");
            player.TakeDamage(retaliationDamage);
        }

        // A physics overlap that detects the player, once detected it loads the card game scene
        protected virtual void DetectPlayerAndBattle()
        {
            // Define detection parameters
            Vector2 detectionCenter = transform.position;
            float detectionRadius = detectionDistance;

            // Debug circle for visualization
            DebugDrawCircle(detectionCenter, detectionRadius, Color.green);

            // Detect objects in the defined radius
            Collider2D[] hits = Physics2D.OverlapCircleAll(detectionCenter, detectionRadius, playerDetection);

            foreach (var hit in hits)
            {
                if (hit.CompareTag(playerTag))
                {
                    Debug.Log("Player detected! Loading next scene...");
                    gameManager?.LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1);
                    return; // Stop further processing after detecting the player
                }
            }
        }

        // Visual aspect of the physics overlap
        private void DebugDrawCircle(Vector2 center, float radius, Color color)
        {
            int segments = 50;
            float angle = 2 * Mathf.PI / segments;

            for (int i = 0; i < segments; i++)
            {
                Vector3 start = center + new Vector2(Mathf.Cos(angle * i), Mathf.Sin(angle * i)) * radius;
                Vector3 end = center + new Vector2(Mathf.Cos(angle * (i + 1)), Mathf.Sin(angle * (i + 1))) * radius;

                Debug.DrawLine(start, end, color);
            }
        }
    }
}
