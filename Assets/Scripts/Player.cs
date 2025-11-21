using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float maxJumpTime = 0.2f;
    public float holdForce = 7f;
    public float fastFallSpeed = 7f;
    public float health = 10;
    public float damage = 2;
    public float wallSlide = 1f; // Geschwindigkeit beim Runterrutschen

    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool canJump = true;
    public bool canMove = true;
    private float jumpTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = 0f;

        if (canMove)
        {
            if (Input.GetKey(KeyCode.A))
                move = -1f;
            else if (Input.GetKey(KeyCode.D))
                move = 1f;

            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed = 10f;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed = 5f;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            isJumping = true;
            canJump = false;
            jumpTimeCounter = maxJumpTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, holdForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
            isJumping = false;

        if (Input.GetKey(KeyCode.S) && !canJump)
        {
            if (rb.linearVelocity.y > -fastFallSpeed)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fastFallSpeed);
        }

        if (Input.GetKey(KeyCode.J))
            playerAttack();

        if (health <= 0)
            gameOver();

        void playerAttack() { }

        void gameOver()
        {
            Application.Quit();

            EditorApplication.ExitPlaymode();

            Debug.Log("Game Over!!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boden"))
        {
            canJump = true;
            isJumping = false;
            canMove = true;
        }

        if (collision.gameObject.CompareTag("Wand"))
        {
            canMove = true; //test
            canJump = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 2;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wand"))
        {
            // Spieler rutscht langsam runter
            if (rb.linearVelocity.y < -wallSlide)
                rb.linearVelocity = new Vector2(0, -wallSlide);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wand"))
        {
            // Bewegung wieder aktivieren wenn Wand verlassen test
            canMove = true;
        }
    }
}
