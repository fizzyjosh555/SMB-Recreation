using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    public float moveSpeed;
    public float jumpSpeed;
    private Animator myAnim;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;

    public int curHealth;
    public int maxHealth = 3;

    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    private LevelManager theLevelManager;
    public int coinValue;

    public AudioClip JumpSound;
    public AudioClip CoinSound;
    public AudioClip InjuredSound;
    private AudioSource source;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        theLevelManager = FindObjectOfType<LevelManager>();
        myAnim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        curHealth = maxHealth;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpSpeed);
            source.PlayOneShot(JumpSound, 1f);
        }
    }

    void FixedUpdate()
    {
        myAnim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
        myAnim.SetBool("Grounded", isGrounded);
        {
            if (knockbackCount <= 0)
            {


                if (Input.GetAxisRaw("Horizontal") > 0f)
                {
                    myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
                    transform.localScale = new Vector2(5, 5);
                }

                else if (Input.GetAxisRaw("Horizontal") < 0f)
                {
                    myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
                    transform.localScale = new Vector2(-5, 5);
                }

                else
                {
                    myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
                }
            }

            else
            {
                if (knockFromRight)
                    myRigidBody.velocity = new Vector2(-knockback, knockback);
                if (!knockFromRight)
                    myRigidBody.velocity = new Vector2(knockback, knockback);
                knockbackCount -= Time.deltaTime;
            }
        }

        if (curHealth > maxHealth)
            curHealth = maxHealth;
        if (curHealth <= 0)
            Die();
    }

    void Die()
    {
        SceneManager.LoadScene(3);
        curHealth = maxHealth;
    }

    public void Damage(int dmg)
    {
        curHealth -= dmg;
        gameObject.GetComponent<Animation>().Play("Injured");
        source.PlayOneShot(InjuredSound, 1f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "MovingPlatform")
            transform.parent = other.transform;
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "MovingPlatform")
            transform.parent = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillPlane")
        {
            SceneManager.LoadScene(3);
        }
        else if (other.tag == "Coin")
        {
            theLevelManager.AddCoins(coinValue);
            source.PlayOneShot(CoinSound, 1f);
            Destroy(other.gameObject);
        }
    }
}
