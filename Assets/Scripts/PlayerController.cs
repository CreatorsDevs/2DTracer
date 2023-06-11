using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 10;
    [SerializeField] private float smoothTime = 0.3f;
    private Vector2 currentVelocity;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Player Movement
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z += Camera.main.nearClipPlane;
        transform.position = Vector2.SmoothDamp(transform.position, mousePosition, ref currentVelocity, smoothTime, maxMoveSpeed);

        //Player Rotation
        Vector2 playerToMouseDirection = mousePosition - transform.position;

            // Flip the sprite based on the player-to-mouse direction
            if (playerToMouseDirection.x > 0f)
            {
                spriteRenderer.flipX = false;
                
            }
            else if (playerToMouseDirection.x < 0f)
            {
                spriteRenderer.flipX = true;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Set the animator parameters based on player movement
        bool isMoving = playerToMouseDirection.sqrMagnitude > 0.3f;
        animator.SetBool("IsMoving", isMoving);
    }
}
