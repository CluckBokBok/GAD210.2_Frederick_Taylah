using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GAD210_Pickup_Base
{

    public class PickupMain : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 1f;
        [SerializeField] protected float detectionDistance = 3f;
        public Transform playerLocation;
        [SerializeField] protected Rigidbody2D pickupItemRB;
        [SerializeField] protected string playerTag = "Player";
        [SerializeField] protected LayerMask PlayerDetection;
        //public TF_TextDisplay textDisplay;
        [SerializeField] private UnityEvent onItemPickedUp;
        //public TF_SoundEffect soundEffectPlayer;

        private bool itemPickedUp = false;

        // Start is called before the first frame update
        public void Start()
        {
            if (playerLocation == null)
            {
                Debug.LogError("Player location is not set.");
            }
            if (pickupItemRB == null)
            {
                Debug.LogError("Rigidbody2D is not set.");
            }

            //onItemPickedUp.AddListener(soundEffectPlayer.PlaySound);

        }

        // Update is called once per frame
        public void Update()
        {
            if (playerLocation != null && pickupItemRB != null)
            {
                DetectPlayerAndMove();
            }
        }

        //Uses Physics Overlap to detect if the player has collided before calling on MoveTowardPlayer functions and PlaySound functions. 
        protected virtual void DetectPlayerAndMove()
        {
            Vector2 detectionCenter = transform.position;
            float detectionRadius = detectionDistance;


            DebugDrawCircle(detectionCenter, detectionRadius, Color.green);


            Collider2D[] hits = Physics2D.OverlapCircleAll(detectionCenter, detectionRadius, PlayerDetection);

            bool playerDetected = false;

            foreach (var hit in hits)
            {
                if (hit.CompareTag(playerTag))
                {
                    playerDetected = true;
                    MoveTowardPlayer(hit.transform.position);
                    //soundEffectPlayer.PlaySound();
                    break;
                }
            }

        }
        //Creates a circle visual. 
        void DebugDrawCircle(Vector2 center, float radius, Color color)
        {
            int segments = 36;
            float angleIncrement = 360f / segments;
            Vector3 prevPoint = center + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = angleIncrement * i * Mathf.Deg2Rad;
                Vector3 newPoint = center + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                Debug.DrawLine(prevPoint, newPoint, color);
                prevPoint = newPoint;
            }
        }

        //Uses force to move the prefab towards the player. 
        protected virtual void MoveTowardPlayer(Vector2 playerPosition)
        {
            Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
            pickupItemRB.AddForce(direction * moveSpeed);

            if (Vector2.Distance(transform.position, playerPosition) < 0.5f)
            {
                onItemPickedUp?.Invoke();
            }
        }
    }


}