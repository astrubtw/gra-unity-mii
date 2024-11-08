using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Movement parameters" )]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    [Range(0.01f, 20.0f)] [SerializeField] private float jumpSpeed = 6.0f;

    private Rigidbody2D rigidBody;

    private Animator animator;

    public LayerMask GroundLayer;

    const float RayLength = 1.1f;

    [SerializeField] private bool rayCastVisable = false;

    private bool isFacingRight = true;
    private bool isClimbing = false;
    private bool isLadder = false;

    private int score = 0;

    private float vertical = 0;

    void Start()
    {
        
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = false;
        vertical = Input.GetAxis("Vertical");

        if (isLadder && System.Math.Abs(vertical) > 0)
        {
            isClimbing = true;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;

            if (!isFacingRight)
                Flip();
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;

            if (isFacingRight)
                Flip();
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }



        if (rayCastVisable)
            Debug.DrawRay(transform.position, RayLength * Vector3.down, Color.white, 1, false);

        animator.SetBool("isGrounded", isGrounded());
        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("verticalVelocity", rigidBody.velocity.y);
        animator.SetBool("isClimbing", isClimbing);
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * moveSpeed);
        }
        else
        {
            rigidBody.gravityScale = 1.2f;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "FallLevel")
        {
            Debug.Log("Player fell from The level");
        }

        if (col.CompareTag("Bonus"))
        {
            score += 100;
            Debug.Log("Score: " + score);
            col.gameObject.SetActive(false);
        }

        if (col.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;

        theScale.x *= -1;

        transform.localScale = theScale;
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, RayLength, GroundLayer.value);
    }

    void jump()
    {
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            Debug.Log("jumping");
        }
    }

}
