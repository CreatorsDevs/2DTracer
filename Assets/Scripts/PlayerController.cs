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
    private int currentHealth = 3; // Player's current health
    private Vector2 currentVelocity;
    private SpriteRenderer spriteRenderer;
    private const int MaxHitEnemies = 10;
    private Collider2D[] hitEnemies = new Collider2D[MaxHitEnemies];
    private Animator animator;
    private Coroutine takeDamageCoroutine;

    private bool decreaseHealth = false;
    private float timerForEnemyOverlap = 0.0f;
    private float waitTimeForDecreasingHealth = 0.5f;
    public bool IsDashing { get; private set; }
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() 
    {
        for(int i = 0; i < MaxHitEnemies; i++)
            hitEnemies[i] = null;

        // Adjust the size based on the maximum number of enemies you expect
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, 0.5f, hitEnemies, enemyLayer);
            
        for (int i = 0; i < count; i++)
        {
            Collider2D enemy = hitEnemies[i];

            if(IsDashing)
            {
                // Destroy the enemy
                Destroy(enemy.gameObject);
            }
            else
            {
                // Decrease Health.
                if(!decreaseHealth)
                {
                    // Decrease Health
                    DecreaseHealth();

                    // Tell Update() that certain time should pass 
                    // after we are able to damage the player again!
                    decreaseHealth = true;
                    // Reset the timer.
                    timerForEnemyOverlap = 0.0f;
                }
            }
        }
    }

    void Update()
    {
        if(decreaseHealth)
        {
            timerForEnemyOverlap += Time.deltaTime;

            if(timerForEnemyOverlap >= waitTimeForDecreasingHealth)
            {
                // Reset Timer
                timerForEnemyOverlap = 0.0f;
                decreaseHealth = false;
            }
        }


        if (IsDashing)
            return;
        
        //Player Movement
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z += Camera.main.nearClipPlane;

        // Calculate the distance between the player and the cursor
        float distanceToCursor = Vector2.Distance(transform.position, mousePosition);

        if (Input.GetMouseButtonDown(0) && distanceToCursor >= 2.5f)
        {
            StartCoroutine(DashTowardsMousePosition(mousePosition));
        }
        else
        {
            MoveTowardsPosition(mousePosition);
        }

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
        // Get the camera's viewport boundaries
        Vector3 minViewport = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        Vector3 maxViewport = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        // Add padding to the viewport boundaries
        minViewport += new Vector3(1, 1, 0);
        maxViewport -= new Vector3(1, 1, 0);

        // Clamp the target position within the viewport boundaries
        targetPosition.x = Mathf.Clamp(targetPosition.x, minViewport.x, maxViewport.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minViewport.y, maxViewport.y);

        transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxMoveSpeed);
    }

    private IEnumerator DashTowardsMousePosition(Vector3 targetPosition)
    {
        IsDashing = true;
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

            yield return null;
        }

        IsDashing = false;
        tr.emitting = false;
        maxMoveSpeed = originalMaxMoveSpeed;
    }

    private void DecreaseHealth()
    {
        if (currentHealth > 0)
        {
            currentHealth--;

            // TODO: Update health UI or perform other actions when the player's health decreases
            GameManager.instance.HandleHealthUI(currentHealth);

            Debug.Log("Player's health decreased. Current health: " + currentHealth);

            if (currentHealth == 0)
            {
                // TODO: Implement player death logic
                Debug.Log("Player died.");
                Destroy(gameObject);
                GameManager.instance.GameOver();
            }
        }
    }
}
