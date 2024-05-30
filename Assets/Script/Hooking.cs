using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooking : MonoBehaviour {
    GrapplingHook grappling;
    public DistanceJoint2D distanceJoint2D;
    public SpringJoint2D joint2D;

    public Transform originalParent; // Hook의 원래 부모 저장
    public GameObject hookedObject; // 훅이 걸린 오브젝트 저장
    public Vector2 hookVelocity; // 훅의 이동 방향과 속도

    void Start() {
        grappling = GameObject.Find("Player").GetComponent<GrapplingHook>();
        joint2D = GetComponent<SpringJoint2D>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        originalParent = transform.parent; // 초기 부모 설정
    }

    void Update() {
        if (!grappling.isAttach) {
            joint2D.distance = Vector3.Distance(grappling.gameObject.transform.position, transform.position);
            distanceJoint2D.distance = Vector3.Distance(grappling.gameObject.transform.position, transform.position);

            // Raycast를 이용한 충돌 감지
            Vector2 position = transform.position;
            Vector2 direction = hookVelocity.normalized; // 훅의 이동 방향에 맞게 설정
            float distance = hookVelocity.magnitude * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, LayerMask.GetMask("Ring", "Ground", "Enemy", "Movable"));

            if (hit.collider != null) {
                HandleCollision(hit.collider);
            }
        }
    }

    void HandleCollision(Collider2D collision) {
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

        // 훅이 충돌한 오브젝트를 저장하고 부모로 설정
        hookedObject = collision.gameObject;
        transform.SetParent(hookedObject.transform);
    }
}