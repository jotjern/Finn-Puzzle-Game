using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {

    public float speed = 5.0f;
    public float jumpForce = 500f;
    public float pawSize = 16f;
    public float pawStrength = 15f;
    public float pawHitCooldown;

    public GameObject paw;

    private float pawHitCooldownTimer = 0f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    bool CanJump()
    {
        return true;
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce);
    }

	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed);
        pawHitCooldownTimer -= Time.deltaTime;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            transform.localScale = new Vector2((Input.GetAxis("Horizontal") < 0) ? 1 : -1, 1f);
        }
        if (Input.GetKeyDown("space"))
        {
            if (CanJump())
            {
                Jump();
            }
        }
        if (Input.GetKeyDown("f"))
        {
            if (pawHitCooldownTimer <= 0f)
            {
                pawHitCooldownTimer = pawHitCooldown;
                foreach (Collider2D other in Physics2D.OverlapCircleAll(paw.transform.position, pawSize))
                {
                    if (other.transform.tag == "Movable")
                    {
                        print("Found movable object");
                        Rigidbody2D other_rb = other.GetComponent<Rigidbody2D>();
                        if (other_rb != null)
                        {
                            Vector2 direction;
                            if (other.transform.position.x > paw.transform.position.x)
                            {
                                direction = Vector2.right;
                            }
                            else
                            {
                                direction = Vector2.left;
                            }
                            print("Adding force");
                            other_rb.AddForce(direction * pawStrength);
                            other_rb.AddForce(Vector2.up * pawStrength);
                        }
                        else
                        {
                            print("Warning: Object tagged as movable doesn't have rigidbody2d");
                        }
                    }
                }
            }
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(paw.transform.position, pawSize);
    }
}
