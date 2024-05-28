using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooking : MonoBehaviour {
    GrapplingHook grappling;
    public DistanceJoint2D distanceJoint2D;
    public SpringJoint2D joint2D;

    public Transform originalParent; // Hook�� ���� �θ� ����
    public GameObject hookedObject; // ���� �ɸ� ������Ʈ ����

    void Start() {
        grappling = GameObject.Find("Player").GetComponent<GrapplingHook>();
        joint2D = GetComponent<SpringJoint2D>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        originalParent = transform.parent; // �ʱ� �θ� ����
    }

    void Update() {
        if (!grappling.isAttach) {
            joint2D.distance = Vector3.Distance(grappling.gameObject.transform.position, transform.position);
            distanceJoint2D.distance = Vector3.Distance(grappling.gameObject.transform.position, transform.position);
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Ring") || collision.CompareTag("Ground")) {
            joint2D.enabled = true;
            if (!grappling.isGround) {
                distanceJoint2D.enabled = true;
            }
            grappling.isAttach = true;
            grappling.isHookAction = false;
        }
        else if (collision.CompareTag("Enemy")) {
            joint2D.enabled = true;
            grappling.isAttach = true;
            grappling.isHookAction = false;
            grappling.GetComponent<Player>().isAttack = true;
        }
        else if (collision.CompareTag("Movable")) {
            joint2D.enabled = true;
            distanceJoint2D.enabled = true;
            grappling.isAttach = true;
            grappling.isHookAction = false;

        }
        // ���� �浹�� ������Ʈ�� �����ϰ� �θ�� ����
        hookedObject = collision.gameObject;
        transform.SetParent(hookedObject.transform);
    }
}