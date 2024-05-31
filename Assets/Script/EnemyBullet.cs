using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ���Ÿ� ������ �Ѿ� ��ũ��Ʈ
 * 
**/

public class EnemyBullet : MonoBehaviour {
    public float speed;             // Bullet �ӵ� 
    Rigidbody2D rigid;   

    void Awake() {
        //������Ʈ �Ҵ� ( �ʱ�ȭ ) 
        rigid = GetComponent<Rigidbody2D>();

        // �� �� ��Ȯ�� �浹�� �ϱ� ���� Continuous�� ���� 
        rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void Initialize( Vector2 direction ) {
        // Bullet�� �߻� �ӵ� 
        rigid.velocity = direction * speed;
    }

    void Update() {
        // �浹 üũ 
        Vector2 position = transform.position;
        // ���� ��ġ�� ���� ��ġ���� ���ư��� ��ġ�� ���� ������ ��ġ�� ���� �� 
        // ��, ���� �������� �̸� ����� �Ѵ�. 
        Vector2 nextPosition = position + rigid.velocity * Time.deltaTime;
        // hit ������ ���� Layer�� Player���� �Ǵ�
        RaycastHit2D hit = Physics2D.Raycast(position, nextPosition - position, (nextPosition - position).magnitude, LayerMask.GetMask("Player"));

        if (hit.collider != null) {
            // ���� ������Ʈ�� tag�� Player �� �� 
            if (hit.collider.CompareTag("Player")) {
                // ������Ʈ ��Ȱ��ȭ 
                Player playerLogic = GameManager.instance.player;
                playerLogic.health -= 1;
                playerLogic.healthUI.SetActive(true);
                for (int i = 0; i < playerLogic.healthBar.Length; i++) {
                    playerLogic.healthBar[i].SetActive(false);
                }
                for (int i = 0; i < playerLogic.health; i++) {
                    playerLogic.healthBar[i].SetActive(true);
                }

                if (transform.position.x - hit.transform.position.x > 0) {
                    playerLogic.rigid.AddForce(new Vector2(-1.5f, 1) * 2f, ForceMode2D.Impulse);
                }
                else {
                    playerLogic.rigid.AddForce(new Vector2(1.5f, 1) * 2f, ForceMode2D.Impulse);
                }

                // �Ѿ��� �¾��� �� ������Ʈ ��Ȱ��ȭ 
                gameObject.SetActive(false);
            }
        }

        // �� ������ ���� ��ġ�� ���� ��ġ�� �ٲ���� ���������� �̸� ���� ����� �Ѵ�. 
        transform.position = nextPosition;
    }
}
