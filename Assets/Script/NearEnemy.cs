using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NearEnemy : Enemy 
{
    bool isMove = false;
    bool isAttack = false;
    bool isDamage = false;

    public float curAttackDelay;
    public float maxAttackDelay;

    Transform target;
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    Animator animator;

    public Vector2 hookVelocity;                // ���� �̵� ����� �ӵ�

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        health = 1;
        target = GameManager.instance.player.gameObject.transform;
        boxCollider.enabled = false;
    }

    void Update() {
        // ���� �ݶ��̴� ���� 
        if (curAttackDelay >= maxAttackDelay && !isAttack) {
            StartCoroutine(AutoAttack());
        }
    }

    IEnumerator AutoAttack() {
        animator.SetBool("isMove", false);
        animator.SetBool("isAttack", true);
        isAttack = true;
        yield return new WaitForSeconds(1.25f);
        boxCollider.enabled = true;

        curAttackDelay = 0;
        yield return new WaitForSeconds(0.1f);
        boxCollider.enabled = false;
        isAttack = false;
        isMove = true;

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttack", false);
    }

    void FixedUpdate() {
        EnemyMove();
    }

    public void EnemyMove() {
        if (isMove && !isAttack && !isDamage) {
            animator.SetBool("isIdle", false);
            animator.SetBool("isMove", true);
            // ���� ������ ( ĳ���͸� �������� �� ���� �غ� ) 
            curAttackDelay += Time.deltaTime;

            Vector3 dir = target.position - transform.position;

            if(dir.x > 0) {
                spriteRenderer.flipX = false;
            }
            else {
                spriteRenderer.flipX = true;
            }

            transform.position += dir * speed * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            // ���� ( �÷��̾ �ǰ� �Ǿ��� �� ) 
            if (gameObject.GetComponentInChildren<BoxCollider2D>().enabled) {
                isAttack = false;
                isMove = true;
                curAttackDelay = 0;
                boxCollider.enabled = false;
            }
        }

        if (collision.gameObject.layer == 12) {
            isDamage = true;

            animator.SetBool("isMove", false);
            animator.SetBool("isAttack", false);
            animator.SetBool("isIdle", true);
            curAttackDelay = 0;
            isMove = false;
            isAttack = false;
            boxCollider.enabled = false;
        }
    }

    void OnTriggerStay2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            // �÷��̾� ���󰡱� 
            if (gameObject.GetComponentInChildren<CircleCollider2D>()) {
                isMove = true;
            }
        }
    }

    void OnTriggerExit2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            if (gameObject.GetComponentInChildren<CircleCollider2D>()) {
                animator.SetBool("isIdle", true);
                animator.SetBool("isMove", false);
                animator.SetBool("isAttack", false);
                isMove = false;
                isAttack = false;
                isDamage = false;
                curAttackDelay = 0;
            }
        }
    }
}
