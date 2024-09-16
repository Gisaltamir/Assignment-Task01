using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioControl : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    private Animator animator;
    private Rigidbody2D rb2D;
    
    public bool grounded;

    public float fallMultiplies = 2f;
    public float lowJumpMultiplier = 2f;

    Rigidbody2D rb;

    void Awake()
    {
       rb = GetComponent<Rigidbody2D>();
  
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        GroundDetect();

        transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, 0);
        //rb2D.velocityX = moveSpeed * Input.GetAxis("Horizontal");

        if(Input.GetAxisRaw("Horizontal") != 0)
        {

            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }

        if(Input.GetButtonDown("Jump") && grounded)
        {
            
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            animator.SetBool("jump", true);

        }

        if(rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplies - 1) * Time.deltaTime;


        } else if (rb2D.velocity.y > 0 && !Input.GetButton("Jump")) 
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        }
    }

    public void MarioDie()
    {
        Debug.Log("Mario is dead!");
        // Display dead image of mario
        // stop time 
        // Bounce mario up
        // stop all comtrol from player 
        //stop camera to move 
        // coroutine not recommend to do that much it wiil required a lot 

        animator.SetTrigger("die");
        rb2D.velocity = new Vector2(0, 10);// bounce
        Destroy(GetComponent<BoxCollider2D>());
        moveSpeed = 0; // user cannot move mario
        Camera.main.transform.parent = null; //camera does not follow the Mario anymore
        Destroy(gameObject, 6);


        StartCoroutine("ContinueTime"); // start corutine
        Time.timeScale = 0;  // stop the game 

    }

    IEnumerator ContinueTime() //make the screen stop before restart
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1; // back to normal running 
        yield return new WaitForSecondsRealtime(4);
        //run here method that restart the level
        Debug.Log("Level restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }



    void GroundDetect()
    {
        Vector3 boxPosition = transform.position;
        RaycastHit2D rayHit = Physics2D.BoxCast(boxPosition,
            new Vector2(1.2f, 0.2f), 0, Vector2.zero, 0, LayerMask.GetMask("Ground", "Enemy")); 
        if(rayHit == true)
        {
            grounded = true;
            animator.SetBool("jump", false);
        }
        else
        {
            grounded= false; // mario in the air
            animator.SetBool("jump", true);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 boxPosition = transform.position;
        Gizmos.DrawWireCube(boxPosition, new Vector2(1.2f, 0.2f));
    }
}



