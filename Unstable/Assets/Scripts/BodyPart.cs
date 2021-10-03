using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D[] rigidbodies2D;

    // force to apply to each rigidbody
    [SerializeField]
    private float runForce;
    [SerializeField]
    private float bounceForce;
    [SerializeField]
    private float jumpForce;

    // torque to apply to each rigidbody
    [SerializeField]
    private float runTorque;
    [SerializeField]
    private float bounceTorque;
    [SerializeField]
    private float jumpTorque;

    [SerializeField]
    private bool wiggle = false;

    public void Update()
    {
        if (wiggle)
        {
            foreach (Rigidbody2D rb2d in rigidbodies2D)
            {
                rb2d.AddForce(jumpForce * Vector2.one);
                rb2d.AddTorque(jumpTorque);
            }
        }
    }

    public void Run()
    {
        foreach (Rigidbody2D rb2d in rigidbodies2D)
        {
            rb2d.AddForce(runTorque * transform.forward);
            rb2d.AddTorque(runTorque);
        }
    }

    public void Bounce()
    {
        foreach (Rigidbody2D rb2d in rigidbodies2D)
        {
            rb2d.AddForce(bounceForce * transform.up, ForceMode2D.Impulse);
            rb2d.AddTorque(bounceTorque);
        }
    }

    public void Jump()
    {
        foreach (Rigidbody2D rb2d in rigidbodies2D)
        {
            rb2d.AddForce(jumpForce * transform.up);
            rb2d.AddTorque(jumpTorque);
        }
    }
}