using System.Collections;
using UnityEngine;

namespace PlayerMovement.Base
{
    public class BaseMovement : MonoBehaviour
    {
        protected Rigidbody2D body;
        private float move = 0f;
        [SerializeField] private float speed = 3f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private LayerMask groundLayer;
        protected bool touchingGround;
        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }


        // Update is called once per frame
        protected void Update()
        {
            InitiateMovement();
            InitiateJumping();
            UpdateAnimations();
        }

        //Enables forward + backward movement, stops movement if no input is detected. 
        protected void InitiateMovement()
        {
            move = 0f;

            // Check input for movement
            if (Input.GetKey(KeyCode.A))
            {
                move = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                move = 1f;
            }

            // Apply movement
            body.velocity = new Vector2(move * speed, body.velocity.y);

            // Adjust speed when sprinting
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 6f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 3f;
            }

            // Flip the player's scale based on movement direction
            if (move != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (move < 0 ? -1 : 1); // Flip on X-axis
                transform.localScale = scale;
            }
        }


        //Checks if player is touching the ground before initiating a jump
        protected void InitiateJumping()
        {
            if (Input.GetKeyDown(KeyCode.Space) && touchingGround)
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                Debug.Log("You are jumping!");
            }

            CheckGrounded();
        }

        //A physics overlap that searches for the ground layer mask to ensure player is touching the ground 
        protected void CheckGrounded()
        {
            touchingGround = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 1f),
                                                   new Vector2(transform.position.x + 0.5f, transform.position.y - 1.1f),
                                                   groundLayer);
        }


        private void UpdateAnimations()
        {
            float absVelocityX = Mathf.Abs(body.velocity.x);

            // Update speed parameter
            animator.SetFloat("Speed", absVelocityX);

            // Update jumping parameter
            animator.SetBool("IsJumping", !touchingGround);
        }


    }
}
