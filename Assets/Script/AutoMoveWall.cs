using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 자동으로 움직이는 지형 스크립트 
 *
 * 일정 간격을 자동으로 이동하는 지형을 구현하기 위해 작성 
**/

public class AutoMoveWall : MonoBehaviour
{
    public float speed = 2f;         // 이동 속도
    public float leftBoundary = -5f; // 왼쪽 경계
    public float rightBoundary = 5f; // 오른쪽 경계

    private bool movingRight = true; // 이동 방향

    void Update() {
        if (movingRight) {
            // 오른쪽 이동 
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // 오른쪽 경계에 도달하면 방향 전환
            if (transform.position.x >= rightBoundary) {
                movingRight = false;
            }
        }
        else {
            // 왼쪽 이동 
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // 왼쪽 경계에 도달하면 방향 전환
            if (transform.position.x <= leftBoundary) {
                movingRight = true;
            }
        }
    }
}
