using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearEnemy : Enemy 
{
    bool isMove = false;

    Transform target;
    Rigidbody2D rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        health = 1;
        target = GameManager.instance.player.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // °ø°Ý µô·¹ÀÌ 
        // curDelay += Time.deltaTime;

        if(isMove && !GameManager.instance.player.GetComponent<Player>().isAttack) {
            Vector3 dir = target.position - transform.position;
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isMove = true;
        }
    }

    void OnTriggerExit2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isMove = false;
        }
    }
}
