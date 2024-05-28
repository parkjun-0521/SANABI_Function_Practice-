using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 1.0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.angularVelocity = speed;
    }
}
