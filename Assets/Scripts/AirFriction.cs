using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFriction : MonoBehaviour {

    public float amountPerSecond;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update () {
        float deltaX = rb.velocity.x - rb.velocity.x * amountPerSecond;
        deltaX *= Time.deltaTime;
        rb.velocity -= Vector2.right * deltaX;
    }

}
