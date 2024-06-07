using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : Enemy
{
    public GameObject target;

    public LayerMask collisionMask;
    public float laserRange = 30.0f; // 레이저 사정거리

    public LineRenderer lineRenderer; // 레이저를 그리기 위한 Line Renderer 컴포넌트

    private Color laserColor;
    void Start()
    {
        health = 1;
        speed = 0;
        laserColor = new Color(1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(curDelay > maxDelay ) {
            ShootBulletInLaserDirection();
            curDelay = 0;
        }
    }

    void OnTriggerStay2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            curDelay += Time.deltaTime;
            lineRenderer.enabled = true;
            ShootLaser();
        }
        else {
            float gValue = Mathf.Clamp01(1 - (curDelay / maxDelay));
            laserColor.g = gValue;
            lineRenderer.startColor = laserColor;
            lineRenderer.endColor = laserColor;
        }
    }

    void OnTriggerExit2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            curDelay = 0;
            lineRenderer.enabled = false;
        }
    }

    void ShootLaser() {
        Vector3 start = transform.position;
        Vector3 direction = target.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(start, direction, Mathf.Infinity, collisionMask); ;

        if (hit.collider != null) {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, hit.point);
        }
        else {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, start + direction * laserRange);
        }
    }
    void ShootBulletInLaserDirection() {
        GameObject bullet = GameManager.instance.poolManager.GetObject(0);
        bullet.transform.position = transform.position;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        bullet.GetComponent<EnemyBullet>().trailRenderer.enabled = true;
        bullet.GetComponent<EnemyBullet>().Initialize(direction);
    }
}
