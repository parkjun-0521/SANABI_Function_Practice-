using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseLineDrawer : MonoBehaviour {
    public float lineSpeed;
    public float lineEnemySpeed;

    public float lineMaxDistance;
    public float lineEnemyMaxDistance;

    public LineRenderer lineRenderer;
    public LayerMask collisionMask; // 충돌을 감지할 레이어 설정
    public Material lineMaterial;

    public Color ringColor;  
    public Color enemyColor;  

    GrapplingHook grappling;

    void Start() {
        if (lineRenderer == null) {
            lineRenderer = GetComponent<LineRenderer>();
        }

        grappling = GetComponent<GrapplingHook>();

        // LineRenderer 설정 (선의 너비, 색상 등)
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2; // 선의 두 점
    }

    void Update() {
        if (!grappling.isAttach) {
            lineRenderer.enabled = true;
            DrawLineToCollision();
        }
        else {
            lineRenderer.enabled = false;
        }
    }

    void DrawLineToCollision() {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // z 좌표를 0으로 설정하여 2D 평면에 그리기

        // 시작점과 방향을 설정
        Vector3 startPoint = transform.position;
        Vector3 direction = (mousePosition - startPoint).normalized;

        // Ray를 발사하여 충돌 지점을 찾음
        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, Mathf.Infinity, collisionMask);

        if (hit.collider != null) {
            // 충돌 지점까지 선을 그림
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, hit.point);

            // 충돌한 오브젝트에 따라 색을 변경 
            if (hit.collider.gameObject.layer.Equals(11)) {
                lineMaterial.SetColor("_Color", enemyColor);
                grappling.speed = lineEnemySpeed;
                grappling.maxDistance = lineEnemyMaxDistance;
            }
            else {
                lineMaterial.SetColor("_Color", ringColor);
                grappling.speed = lineSpeed;
                grappling.maxDistance = lineMaxDistance;
            }
        }
        else {
            // 충돌 지점이 없으면 마우스 위치까지 선을 그림
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, mousePosition);
            lineMaterial.SetColor("_Color", ringColor);
            grappling.speed = lineSpeed;
            grappling.maxDistance = lineMaxDistance;
        }
    }
}
