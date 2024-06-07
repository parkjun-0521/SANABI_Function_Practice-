using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongDistance : Enemy
{
    bool isMove;

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
            // 회전 
            Vector3 dir = target.position - transform.position;
            if (dir.x > 0) {
                spriteRenderer.flipX = false;
            }
            else {
                spriteRenderer.flipX = true;
            }
            // 공격 
            if (curDelay > maxDelay) {
                StartCoroutine(FireBulletsWithDelay());
                curDelay = 0;
            }
        }
    }
    IEnumerator FireBulletsWithDelay() {
        for (int i = 0; i < 5; i++) {
            GameObject bullet = GameManager.instance.poolManager.GetObject(1);
            bullet.transform.position = transform.position;
            Vector3 direction = (target.transform.position - transform.position).normalized;
            bullet.GetComponent<EnemyBullet>().trailRenderer.enabled = true;

            // 랜덤 각도를 계산 (여기서는 -15도에서 +15도 사이의 각도)
            float randomAngle = Random.Range(-20f, 20f);

            // 방향 벡터를 회전시킴
            direction = Quaternion.Euler(0, 0, randomAngle) * direction;

            bullet.GetComponent<EnemyBullet>().Initialize(direction);
            yield return new WaitForSeconds(0.1f);
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
