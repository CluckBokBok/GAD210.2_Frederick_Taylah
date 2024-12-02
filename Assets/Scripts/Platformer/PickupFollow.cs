using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


namespace GAD210_Pickup_Base
{
    public class TF_ItemPickupFollow : PickupMain
    {

        [SerializeField] private Vector2 followOffset = new Vector2(1.2f, 1.2f);
        private bool isFollowingPlayer = false;
        [SerializeField] private float bounceAmplitude = 0.3f;
        [SerializeField] private float bounceFrequency = 10f;
        private float bounceTimer = 0f;


        // Start is called before the first frame update
        void Start()
        {
            base.Start();

        }

        // Update is called once per frame
        public void Update()
        {

            base.Update();

            if (!isFollowingPlayer && playerLocation != null && pickupItemRB != null)
            {
                DetectPlayerAndMove();
            }
            else if (isFollowingPlayer && playerLocation != null)
            {
                FollowPlayer();
            }
        }
        //Adds a bouncing animation once the item has been collected and begins following. 
        private void FollowPlayer()
        {
            // Determine the direction based on the player's scale
            float playerDirection = Mathf.Sign(playerLocation.localScale.x);

            // Adjust the offset to be on the left or right side
            Vector2 dynamicOffset = followOffset;
            dynamicOffset.x = Mathf.Abs(followOffset.x) * (playerDirection < 0 ? 1 : -1);

            // Calculate the target position with the dynamic offset
            Vector2 targetPosition = (Vector2)playerLocation.position + dynamicOffset;

            // Ensure the pickup stays at the same height as the player
            targetPosition.y = playerLocation.position.y;

            // Update the position of the pickup
            transform.position = targetPosition;

            // Debugging logs (optional)
            Debug.Log($"Player Direction: {playerDirection}, Offset: {dynamicOffset}, Target Position: {targetPosition}");
        }


        //Collider that detects the player and checks that the prefab is 'following'. Also destroys RB to ensure the prefab doesn't affect player movement. 
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(playerTag))
            {

                isFollowingPlayer = true;
                transform.SetParent(playerLocation);
                Destroy(pickupItemRB);
                Debug.Log("You have a follower!");
            }
        }

        


    }
}