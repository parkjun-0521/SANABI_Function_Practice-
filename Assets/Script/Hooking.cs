using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ���� �� �� ��ũ��Ʈ 
 * 
 * ������ ������ �� �浹�ϸ� ���� �� �� ���� 
**/

public class Hooking : MonoBehaviour {
    GrapplingHook grappling;

    public DistanceJoint2D distanceJoint2D;     // Ư�� ��Ȳ�� ���� ����Ʈ ( ���߿� �� ���� �� ���� ���� ���� ) 
    public SpringJoint2D joint2D;               // �⺻ ������ ���� ����Ʈ 

    public Transform originalParent;            // Hook�� ���� �θ� ����
    public GameObject hookedObject;             // ���� �ɸ� ������Ʈ ����
    public Vector2 hookVelocity;                // ���� �̵� ����� �ӵ�

    void Start() {
        grappling = GameObject.Find("Player").GetComponent<GrapplingHook>();
        // ����Ʈ �ʱ�ȭ 
        joint2D = GetComponent<SpringJoint2D>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        // �ʱ� �θ� ����
        originalParent = transform.parent; 
    }

    void Update() {
        // ������ ���ư��� 
        if (!grappling.isAttach) {
            // ����Ʈ�� �Ÿ� 
            joint2D.distance = Vector3.Distance(grappling.gameObject.transform.position, transform.position);
            distanceJoint2D.distance = Vector3.Distance(grappling.gameObject.transform.position, transform.position);

            // Raycast�� �̿��� �浹 ����
            Vector2 position = transform.position;
            // ���� �̵� ���⿡ �°� ����
            Vector2 direction = hookVelocity.normalized;
            // �̵� �������� �Ÿ� ���� 
            float distance = hookVelocity.magnitude * Time.deltaTime;
            // �浹�� Layer �Ǵ� 
            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, LayerMask.GetMask("Ring", "Ground", "Enemy", "Movable"));

            // �浹 �Ǿ��� �� ���� ���� 
            if (hit.collider != null) {
                HandleCollision(hit.collider);
            }
        }
    }

    void HandleCollision(Collider2D collision) {
        if (collision.CompareTag("Ring") || collision.CompareTag("Ground")) {   // Ring �� Ground Tag�� �浹 �� �� 
            joint2D.enabled = true;
            // ���߿� ������ �� SpringJoint �� ���̰� �̻������� ������ �ذ��ϱ� ���� 
            // Distancejoint�� �Ѳ� ����Ͽ� ���̸� ���� 
            if (!grappling.isGround) {
                distanceJoint2D.enabled = true;
            }
            grappling.isAttach = true;
            grappling.isHookAction = false;
        } 
        else if (collision.CompareTag("Enemy")) {                              // Enemy Tag�� �浹 �� �� 
            joint2D.enabled = true;
            grappling.isAttach = true;
            grappling.isHookAction = false;

            // ���� �̺�Ʈ ���� 
            grappling.GetComponent<Player>().isAttack = true;
        } 
        else if (collision.CompareTag("Movable")) {                            // Movable Tag�� �浹 �� �� 
            joint2D.enabled = true;
            distanceJoint2D.enabled = true;
            grappling.isAttach = true;
            grappling.isHookAction = false;
        }

        // ���� �浹�� ������Ʈ�� �����ϰ� �θ�� ���� ( �̵��ϴ� ������Ʈ���� ���� �̵��ϵ��� �ϱ� ���� )
        hookedObject = collision.gameObject;
        transform.SetParent(hookedObject.transform);
    }
}