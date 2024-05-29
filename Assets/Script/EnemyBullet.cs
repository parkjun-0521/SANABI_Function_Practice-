using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public float speed;
    private Rigidbody2D rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void Initialize( Vector2 direction ) {
        rigid.velocity = direction * speed;
    }

    void Update() {
        Vector2 position = transform.position;
        Vector2 nextPosition = position + rigid.velocity * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(position, nextPosition - position, (nextPosition - position).magnitude, LayerMask.GetMask("Player"));

        if (hit.collider != null) {
            if (hit.collider.CompareTag("Player")) {
                GameManager.instance.player.health -= 1;
                gameObject.SetActive(false);
            }
        }

        transform.position = nextPosition;
    }
}
