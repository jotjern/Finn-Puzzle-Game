using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {

    public float speed = 5.0f;
    public float jumpForce = 500f;
    public float pawSize = 16f;
    public float pawStrength = 15f;
    public float pawHitCooldown;
    public bool active = true;
    public float timeSinceLevelLoad;

    public GameObject paw;
    public Rigidbody2D rb;

    private float pawHitCooldownTimer = 0f;
    private Collider2D coll;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
	}

    bool CanJump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit.collider != null)
        {
            if (hit.distance < coll.bounds.extents.y + 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

	// Update is called once per frame
	void Update () {
        if (!active)
            return;
        transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed);
        pawHitCooldownTimer -= Time.deltaTime;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            transform.localScale = new Vector3((Input.GetAxis("Horizontal") < 0) ? 1 : -1, 1f, 1f);
        }
        if (Input.GetKey("space"))
        {
            if (CanJump())
            {
                Jump();
            }
        }
        if (Input.GetKey("f"))
        {
            if (pawHitCooldownTimer <= 0f)
            {
                pawHitCooldownTimer = pawHitCooldown;
                foreach (Collider2D other in Physics2D.OverlapCircleAll(paw.transform.position, pawSize))
                {
                    if (other.transform.tag == "Movable")
                    {
                        TilemapManager tmm = other.transform.parent.GetComponent<TilemapManager>();
                        Vector2Int boxPos = tmm.GetTilePosFromTransformPos(other.transform.position);
						tmm.Map.PushBox(boxPos.x, boxPos.y, other.transform.position.x > transform.position.x);
                        break;
                    }
                }
            }
        }
	}
}
