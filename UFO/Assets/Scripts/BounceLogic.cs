/*
Author: Sunny Dinh
Date: 9/16/24
Description: This script is made to: 
    1. Start the pickup object moving in a random direction. 
    2. Bounce off walls at an angle that they hit
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BounceLogic : MonoBehaviour
{
    // set initial speed the object will bounce
    public float bounce = 5f;
    // reference game object RigidBody2D
    private Rigidbody2D rb;
    // Create Vector2 to store current velocity of 2D object
    private Vector2 bounceVelocity;



    // Start is called before the first frame update
    void Start()
    {
        // set gameObject to rigidbody
        rb = GetComponent<Rigidbody2D>();
        /*
            Create a random direction for this game Object
            Using Random.insideUnitCircle method to have a random point chosen within a unitCircle radius
            Using .normalized to a unit vector to ensure the object will move in one direction
        */
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // Determine the gameObject speed
        rb.velocity = randomDirection * bounce;

    }

    // Update is called once per frame
    void Update()
    {
        // This controls the speed the gameObject will bounce with a collider
        bounceVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // calculate bounceVelocity speed using .magnitude
        var speed = bounceVelocity.magnitude;

        // Vector2.Refelct will calculate the direction the gameObject will bounce
        var direction = Vector2.Reflect(bounceVelocity.normalized, collision.contacts[0].normal);

        // set the speed of after the gameObject collides with another collider
        rb.velocity = direction * speed;
    }


}
