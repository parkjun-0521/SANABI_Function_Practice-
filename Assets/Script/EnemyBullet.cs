using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 원거리 몬스터의 총알 스크립트
 * 
**/

public class EnemyBullet : MonoBehaviour {
    public float speed;             // Bullet 속도 
    Rigidbody2D rigid;   

    void Awake() {
        //컴포넌트 할당 ( 초기화 ) 
        rigid = GetComponent<Rigidbody2D>();

        // 좀 더 정확한 충돌을 하기 위해 Continuous로 변경 
        rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void Initialize( Vector2 direction ) {
        // Bullet의 발사 속도 
        rigid.velocity = direction * speed;
    }

    void Update() {
        // 충돌 체크 
        Vector2 position = transform.position;
        // 다음 위치는 현재 위치에서 날아가는 위치의 다음 프레임 위치를 더한 값 
        // 즉, 다음 프레임을 미리 계산을 한다. 
        Vector2 nextPosition = position + rigid.velocity * Time.deltaTime;
        // hit 변수로 맞은 Layer가 Player인지 판단
        RaycastHit2D hit = Physics2D.Raycast(position, nextPosition - position, (nextPosition - position).magnitude, LayerMask.GetMask("Player"));

        if (hit.collider != null) {
            // 맞은 오브젝트의 tag가 Player 일 때 
            if (hit.collider.CompareTag("Player")) {
                // 오브젝트 비활성화 
                gameObject.SetActive(false);
            }
        }

        // 매 프레임 마다 위치를 다음 위치로 바꿔줘야 지속적으로 미리 물리 계산을 한다. 
        transform.position = nextPosition;
    }
}
