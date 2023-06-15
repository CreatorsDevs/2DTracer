using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 10;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float dashSpeedMultiplier = 2f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private TrailRenderer tr;
    private Vector2 currentVelocity;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isDashing = false;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDashing)
            return;
        
        //Player Movement
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z += Camera.main.nearClipPlane;

        // Calculate the distance between the player and the cursor
        float distanceToCursor = Vector2.Distance(transform.position, mousePosition);

        if (Input.GetMouseButtonDown(0) && distanceToCursor >= 5f)
        {
            StartCoroutine(DashTowardsMousePosition(mousePosition));
        }
        else
        {
            MoveTowardsPosition(mousePosition);
        }

        //transform.position = Vector2.SmoothDamp(transform.position, mousePosition, ref currentVelocity, smoothTime, maxMoveSpeed);

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

    private void MoveTowardsPosition(Vector3 targetPosition)
    {
        transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxMoveSpeed);
    }

    private IEnumerator DashTowardsMousePosition(Vector3 targetPosition)
    {
        isDashing = true;
        float dashTimer = 0f;
        float originalMaxMoveSpeed = maxMoveSpeed;

        AudioManager.instance.Play(SoundNames.DashSound);
        tr.emitting= true;

        while (dashTimer < dashDuration)
        {
            dashTimer += Time.deltaTime;

            // Calculate the dash direction
            Vector2 dashDirection = (targetPosition - transform.position).normalized;

            // Move the player with increased speed
            transform.Translate(dashDirection * (maxMoveSpeed * dashSpeedMultiplier * Time.deltaTime));

            // Check for enemy collisions
            CheckEnemyCollisions();

            yield return null;
        }

        isDashing = false;
        tr.emitting = false;
        maxMoveSpeed = originalMaxMoveSpeed;
    }

    private void CheckEnemyCollisions()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.5f, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            // Destroy the enemy
            Destroy(enemy.gameObject);
        }
    }
}
