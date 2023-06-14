using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 2;
    [SerializeField] private float smoothTime = 0.1f;
    private Vector2 currentVelocity;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.player != null)
        {
            // Calculate the direction from the enemy to the player
            Vector2 direction = GameManager.instance.player.transform.position - transform.position;
            direction.Normalize(); // Normalize the direction to have a magnitude of 1

            // Flip the sprite based on the enemy-to-player direction
                if (direction.x > 0f)
                {
                    spriteRenderer.flipX = false;
                    
                }
                else if (direction.x < 0f)
                {
                    spriteRenderer.flipX = true;
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Move the enemy towards the player
            transform.position = Vector2.SmoothDamp(transform.position, GameManager.instance.player.transform.position, ref currentVelocity, smoothTime, maxMoveSpeed);
        }
    }
}
