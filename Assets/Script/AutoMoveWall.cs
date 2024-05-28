using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveWall : MonoBehaviour
{
    public float speed = 2f; // 이동 속도
    public float leftBoundary = -5f; // 왼쪽 경계
    public float rightBoundary = 5f; // 오른쪽 경계

    private bool movingRight = true; // 이동 방향

    void Update() {
        if (movingRight) {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // 오른쪽 경계에 도달하면 방향 전환
            if (transform.position.x >= rightBoundary) {
                movingRight = false;
            }
        }
        else {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // 왼쪽 경계에 도달하면 방향 전환
            if (transform.position.x <= leftBoundary) {
                movingRight = true;
            }
        }
    }
}
