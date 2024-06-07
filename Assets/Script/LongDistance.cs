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
        // ����
        curDelay += Time.deltaTime;
        LongEnemyAttack();
    }

    void FixedUpdate() {
        // �� �̵� 
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
            // �̵� ���� 
            isMove = false;
            // ȸ�� 
            Vector3 dir = target.position - transform.position;
            if (dir.x > 0) {
                spriteRenderer.flipX = false;
            }
            else {
                spriteRenderer.flipX = true;
            }
            // ���� 
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

            // ���� ������ ��� (���⼭�� -15������ +15�� ������ ����)
            float randomAngle = Random.Range(-20f, 20f);

            // ���� ���͸� ȸ����Ŵ
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
        // ��ü�� ��� �׸���
        Gizmos.color = Color.red;  // ���� ����
        Gizmos.DrawRay(transform.position, transform.forward);  // ���� �׸���
        Gizmos.DrawWireSphere(transform.position + transform.forward ,radius);  // ������������ ��ü �׸���
    }
}
