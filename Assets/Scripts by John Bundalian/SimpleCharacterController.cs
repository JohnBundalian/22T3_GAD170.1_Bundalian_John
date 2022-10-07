using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JohnBundalian
{
    public class SimpleCharacterController : MonoBehaviour
    {
        private Rigidbody2D rbody2D;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private LayerMask groundLayerMask;
        private float horizontalMovementValue = 0f;
        private bool isGrounded = false;
        private bool isJumpButtonHeld = false;

        [Header("Character Stats")]
        [SerializeField] private float runSpeed = 3f;
        [SerializeField] private float jumpStrength = 5f;

        private void Awake()
        {
            rbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // Initialise Character.
            Initialise();
        }
        
            private void Update()
        {
            // Horizontal movement value to Playetr 
            rbody2D.velocity = new Vector2(Input.GetAxis("Horizontal"), rbody2D.velocity.y);

            if (Input.GetKey(KeyCode.Space))
                rbody2D.velocity = new Vector2(rbody2D.velocity.x, runSpeed);

                // Flip our sprite along a vertical 'mirror line' so that it 'faces' the correct direction.
                // To the left.
                if (horizontalMovementValue < 0f && spriteRenderer.flipX == true)
                {
                    spriteRenderer.flipX = false;
                }
                // To the right.
                else if (horizontalMovementValue > 0f && spriteRenderer.flipX == false)
                {
                    spriteRenderer.flipX = true;
                }

                // Set our animator's Speed parameter so that Alan's run animation plays.
                animator.SetFloat("Speed", Mathf.Abs(horizontalMovementValue));

                // Transform this object's position on the X axis (horizontally)
                // at a rate of "moveSpeed" units every second (by default, 3 units per second).
                transform.position += Time.deltaTime * runSpeed * new Vector3(horizontalMovementValue, 0);

                // If the player presses "Jump" (by default, the space bar on the keyboard)...
                if (isGrounded && Input.GetButtonDown("Jump"))
                {
                    // ...set some animation and control parameters...
                    isJumpButtonHeld = true;
                    animator.SetBool("IsJumping", true);
                    isGrounded = false;

                    // ...then add vertical velocity to make their character "jump"!
                    rbody2D.velocity = new Vector3(rbody2D.velocity.x, jumpStrength);
                }

                // If the player releases the "Jump" button...
                if (Input.GetButtonUp("Jump"))
                {
                    // ...tell the game that the player is not trying to jump any more.
                    isJumpButtonHeld = false;
                }
            }

            // Our physics update.
            private void FixedUpdate()
            {
                // Check if this object is colliding with any other object at its feet.
                // If it is...
                if (Physics2D.OverlapCircle(transform.position, 0.1f, groundLayerMask))
                {
                    // ...then tell the game that it should be treated as being "grounded".
                    isGrounded = true;

                    // If the player is not holding the jump button...
                    if (!isJumpButtonHeld)
                    {
                        // ...then tell the animator to stop playing the jump animation.
                        animator.SetBool("IsJumping", false);
                    }
                }
                // If we are not colliding...
                else
                {
                    // ...then tell the game that this object is airborne.
                    isGrounded = false;
                }
            }

            /// <summary>
            /// Set up some initial on-spawn values.
            /// </summary>
            private void Initialise()
            {
                // Set up some references to our Rigidbody2D, SpriteRenderer, and Animator,
                // and tell this character what the ground is.
                if (rbody2D == null) rbody2D = GetComponent<Rigidbody2D>();
                if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                if (animator == null) animator = GetComponentInChildren<Animator>();
                if (groundLayerMask != 8) groundLayerMask = 8;

            }
    }
}
