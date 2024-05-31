using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * �ڵ����� �����̴� ���� ��ũ��Ʈ 
 *
 * ���� ������ �ڵ����� �̵��ϴ� ������ �����ϱ� ���� �ۼ� 
**/

public class AutoMoveWall : MonoBehaviour
{
    public float speed = 2f;         // �̵� �ӵ�
    public float leftBoundary = -5f; // ���� ���
    public float rightBoundary = 5f; // ������ ���

    private bool movingRight = true; // �̵� ����

    void Update() {
        if (movingRight) {
            // ������ �̵� 
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // ������ ��迡 �����ϸ� ���� ��ȯ
            if (transform.position.x >= rightBoundary) {
                movingRight = false;
            }
        }
        else {
            // ���� �̵� 
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // ���� ��迡 �����ϸ� ���� ��ȯ
            if (transform.position.x <= leftBoundary) {
                movingRight = true;
            }
        }
    }
}
