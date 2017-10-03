using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [HideInInspector] public bool facingRight;

    public int scoreValue = 1;
    public float maxSpeed = 5f;
    public bool jumping;
    public float jumpForce = 1000f;
    public Transform groundCheck;

    private Rigidbody2D rb2d;
    private CircleCollider2D myCollider;
    private PlayerController playerController;
    private bool grounded = false;
    private bool jump = false;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
        facingRight = Random.value > 0.5f;

        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }
        if (playerController == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }

        // Flip the sprite if not facing right
        if (!facingRight)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
	
	void Update () {

        int h = (facingRight ? 1 : -1);

        rb2d.velocity = new Vector2(Mathf.Sign(h) * maxSpeed, rb2d.velocity.y);

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 11 << LayerMask.NameToLayer("Arena"));

        if (jumping && grounded)
        {
            jump = true;
        }

        if (jump)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Border") || collision.collider.CompareTag("Enemy"))
        {
            Flip();
        }
        else if(collision.collider.CompareTag("Player"))
        {
            if(collision.transform.position.y > transform.position.y + myCollider.radius/2 * transform.localScale.y)
            {
                playerController.AddScore(scoreValue);
                collision.rigidbody.AddForce(new Vector2(0f, 300f));
                Destroy(gameObject);
            }
            else
            {
                playerController.TakeDamage();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            playerController.TakeDamage();
        }
        else if(collision.collider.CompareTag("Border"))
        {
            if(transform.position.x < 0)
            {
                if(!facingRight)
                {
                    Flip();
                }
            }
            else
            {
                if(facingRight)
                {
                    Flip();
                }
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void AddScore(int value)
    {
        scoreValue += value;
        transform.localScale = new Vector3(transform.localScale.x * 1.2f, transform.localScale.y * 1.2f, transform.localScale.z);
    }
}
