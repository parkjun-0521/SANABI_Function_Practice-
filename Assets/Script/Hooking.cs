using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 고정 될 훅 스크립트 
 * 
 * 로프가 나갔을 때 충돌하면 고정 될 훅 구현 
**/

public class Hooking : MonoBehaviour {
    GrapplingHook grappling;

    public DistanceJoint2D distanceJoint2D;     // 특정 상황에 사용될 조인트 ( 공중에 떠 있을 때 로프 길이 고정 ) 
    public SpringJoint2D joint2D;               // 기본 로프에 사용될 조인트 

    public Transform originalParent;            // Hook의 원래 부모 저장
    public GameObject hookedObject;             // 훅이 걸린 오브젝트 저장
    public Vector2 hookVelocity;                // 훅의 이동 방향과 속도

    void Start() {
        grappling = GameObject.Find("Player").GetComponent<GrapplingHook>();
        // 조인트 초기화 
        joint2D = GetComponent<SpringJoint2D>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        // 초기 부모 설정
        originalParent = transform.parent; 
    }

    void Update() {
        // 로프가 날아갈때 
        if (!grappling.isAttach) {
            // 조인트의 거리 
            joint2D.distance = Vector3.Distance(grappling.gameObject.transform.position, transform.position);
            distanceJoint2D.distance = Vector3.Distance(grappling.gameObject.transform.position, transform.position);

            // Raycast를 이용한 충돌 감지
            Vector2 position = transform.position;
            // 훅의 이동 방향에 맞게 설정
            Vector2 direction = hookVelocity.normalized;
            // 이동 방향으로 거리 저장 
            float distance = hookVelocity.magnitude * Time.deltaTime;
            // 충돌할 Layer 판단 
            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, LayerMask.GetMask("Ring", "Ground", "Enemy", "Movable"));

            // 충돌 되었을 때 로프 고정 
            if (hit.collider != null) {
                HandleCollision(hit.collider);
            }
        }
    }

    void HandleCollision(Collider2D collision) {
        if (collision.CompareTag("Ring") || collision.CompareTag("Ground")) {   // Ring 과 Ground Tag에 충돌 할 때 
            joint2D.enabled = true;
            // 공중에 떠있을 떄 SpringJoint 의 길이가 이상해지는 현상을 해결하기 위해 
            // Distancejoint도 한께 사용하여 길이를 고정 
            if (!grappling.isGround) {
                distanceJoint2D.enabled = true;
            }
            grappling.isAttach = true;
            grappling.isHookAction = false;
        } 
        else if (collision.CompareTag("Enemy")) {                              // Enemy Tag에 충돌 할 때 
            joint2D.enabled = true;
            grappling.isAttach = true;
            grappling.isHookAction = false;

            // 공격 이벤트 동작 
            grappling.GetComponent<Player>().isAttack = true;
        } 
        else if (collision.CompareTag("Movable")) {                            // Movable Tag에 충돌 할 때 
            joint2D.enabled = true;
            distanceJoint2D.enabled = true;
            grappling.isAttach = true;
            grappling.isHookAction = false;
        }

        // 훅이 충돌한 오브젝트를 저장하고 부모로 설정 ( 이동하는 오브젝트에서 같이 이동하도록 하기 위해 )
        hookedObject = collision.gameObject;
        transform.SetParent(hookedObject.transform);
    }
}