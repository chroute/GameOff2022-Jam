using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GO22
{
    public class LetterMovement : MonoBehaviour
    {
        Rigidbody2D myRigidBody;
        CapsuleCollider2D myBoundDetectorCollider;
        [SerializeField] float speed = 1f;

        void Start()
        {
            myRigidBody = GetComponent<Rigidbody2D>();
            myBoundDetectorCollider = GetComponent<CapsuleCollider2D>();
        }

        void Update()
        {
            Move();
        }
        void Move()
        {
            Vector2 letterVelocity = new Vector2(myRigidBody.velocity.x, speed);
            myRigidBody.velocity = letterVelocity;
        }

        void OnTriggerEnter2D(Collider2D myBoundDetectorCollider)
        {
            FlipDirection();
        }

        private void FlipDirection()
        {
            speed = -speed;
        }

    }
}
