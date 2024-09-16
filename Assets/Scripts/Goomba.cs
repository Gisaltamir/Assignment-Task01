using UnityEngine;

public class Goomba : MonoBehaviour
{
    public float moveSpeed;
    public GameObject detectionPoint;
    public Animator  animator;

    public float direction; //-1 when go left, 1 when go right
    public LayerMask groundLayer;
    public bool changeDirection;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * direction, 0, 0);
        transform.localScale = new Vector3(direction, 1,1);
       
    }

    private void LateUpdate()
    {
       Debug.DrawRay(detectionPoint.transform.position, Vector3.down, Color.green);

        // Raycast is important thing to know hot to do it
        RaycastHit2D hit = Physics2D.Raycast(detectionPoint.transform.position, Vector2.down, 1, groundLayer);
        if( hit.collider == null)
        {
            //Ray didn't hit anything so we must change direction 
            changeDirection = true;

        }

        // Raycast is important thing to know hot to do it // f - mean a float num
        RaycastHit2D hit2 = Physics2D.Raycast(detectionPoint.transform.position, Vector2.right * direction, 0.2f, groundLayer);
        if (hit2.collider != null)
        {
            //Ray hit somthing like a wall so we must change direction 
            //ChangeDirection();
            changeDirection = true;

        }
        if (changeDirection == true)
        {
            ChangeDirection();
           
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.transform.position.y > transform.position.y + collision.collider.bounds.size.y / 2)
            {
                // Let's bounce Mario little bit from Goomba
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = 
                    new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, 5);
                Die();
            }
            else
            {
                //Maro dies 
                collision.gameObject.GetComponent<MarioControl>().MarioDie();

            }

        }

    }



    void ChangeDirection()
    {
        direction *= -1;
        changeDirection = false;

    }


    public void Die()
    {
        animator.SetTrigger("Die");
        moveSpeed = 0;
        // Destroy component 
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(gameObject, 2);
    }


}

