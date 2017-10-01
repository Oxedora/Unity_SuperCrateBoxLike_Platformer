using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public int life = 3;
    public float invincibleTime = 1.5f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public float timeToSleep = 5f;
    public Transform groundCheck;
    public AudioClip hit;
    public AudioClip die;
    public MovementController movementController;
    public JumpController jumpController;


    private bool grounded = false;
    private bool dead = false;
    private Animator anim;
    private AudioSource audioSource;
    private Rigidbody2D rb2d;
    private float lastTimeMoved;
    private float lastTimeHited;
    private GameController gameController;


    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        lastTimeHited = -invincibleTime;

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 11 << LayerMask.NameToLayer("Arena"));

        if (jumpController.CanJump() && grounded)
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        if(!dead)
        {
            float h = movementController.GetDirection();

            // If h is 0, then the player moved, the lastTimeMoved is now
            if (h != 0)
                lastTimeMoved = Time.time;

            // Setting to true isInactive if it's been timeToSleep seconds since last moved
            anim.SetBool("IsInactive", Time.time - lastTimeMoved > timeToSleep);

            if (h == 0)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            else
            {
                rb2d.velocity = new Vector2(Mathf.Sign(h) * maxSpeed, rb2d.velocity.y);
            }

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();

            if (jump)
            {
                rb2d.AddForce(new Vector2(0f, jumpForce));
                jump = false;
                lastTimeMoved = Time.time;
            }

            anim.SetBool("Fly", !grounded);
        }
    }


    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void PlaySound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void Respawn()
    {
        TakeDamage();

        //if(life > 0)
        //{
            transform.position = new Vector2(0f, 0f);
        //}
        
    }

    public void TakeDamage()
    {
        if(Time.time - Mathf.Abs(lastTimeHited) > invincibleTime && !dead)
        {
            anim.SetTrigger("Hurt");
            life--;
            lastTimeHited = lastTimeMoved = Time.time;
            PlaySound(hit);
        }

        if (life < 1)
        {
            Die();
        }
    }

    public void AddScore(int value)
    {
        gameController.AddScore(value);
    }

    public void Die()
    {
        if(!dead)
        {
            anim.SetBool("Die", true);
            dead = true;
            PlaySound(die);
            gameController.GameOver();
        }
    }
}