using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GAD210_Enemy_Base
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager; // Assign in the Inspector
        [SerializeField] private float detectionDistance = 5f;
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private LayerMask playerDetection;
        [SerializeField] public float enemyHealth = 100f;
        [SerializeField] private float retaliationDamage = 10f;

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

        public void TakeDamage(float amount)
        {
            enemyHealth -= amount;
            Debug.Log($"Enemy took {amount} damage. Current health: {enemyHealth}");

            if (enemyHealth <= 0)
            {
                Debug.Log("Enemy has been defeated!");
                // Handle enemy defeat logic here
            }
        }

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

        public void Retaliate(PlayerManager player)
        {
            if (player == null)
            {
                Debug.LogWarning("Player reference is null. Cannot retaliate.");
                return;
            }

            Debug.Log($"Enemy retaliates with {retaliationDamage} damage.");
            player.TakeDamage(retaliationDamage);
        }

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
