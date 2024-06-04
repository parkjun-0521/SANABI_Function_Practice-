using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongDistance : MonoBehaviour
{
    bool isMove;

    public float speed;
    public float curDelay;
    public float maxDelay;

    public float radius = 10.0f;
    public LayerMask layerMask;

    public Transform target;
    public CircleCollider2D circleCollider;
    SpriteRenderer spriteRenderer;

    void Awake() {
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() {
        target = GameManager.instance.player.transform;
    }

    void Update() {
        // 공격
        curDelay += Time.deltaTime;
        LongEnemyAttack();
    }

    void FixedUpdate() {
        // 적 이동 
        LongEnemyMove();
    }

    void LongEnemyMove() {
        if (isMove) {

            Vector3 dir = target.position - transform.position;

            if (dir.x > 0) {
                spriteRenderer.flipX = false;
            }
            else {
                spriteRenderer.flipX = true;
            }

            transform.position += dir * speed * Time.fixedDeltaTime;
        }
    }

    void LongEnemyAttack() {
        if (Physics2D.CircleCast(transform.position, radius, Vector2.up, radius, layerMask)) {
            // 이동 정지 
            isMove = false;
            // 공격 
            if (curDelay > maxDelay) {
                GameObject bullet = GameManager.instance.poolManager.GetObject(1);
                bullet.transform.position = transform.position;
                Vector3 direction = (target.transform.position - transform.position).normalized;
                bullet.GetComponent<EnemyBullet>().Initialize(direction);
                curDelay = 0;
            }
        }
    }

    void OnTriggerStay2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isMove = true;
        }
    }

    void OnTriggerExit2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isMove = false;
        }
    }

    void OnDrawGizmos() {
        // 구체의 경로 그리기
        Gizmos.color = Color.red;  // 색상 설정
        Gizmos.DrawRay(transform.position, transform.forward);  // 레이 그리기
        Gizmos.DrawWireSphere(transform.position + transform.forward ,radius);  // 목적지에서의 구체 그리기
    }
}
