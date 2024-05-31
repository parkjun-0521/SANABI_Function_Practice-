using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 로프 발사 거리 표기 스크립트  
 * 
 * 로프가 발사될 거리를 미리 보여주기 위해 구현 
**/


public class MouseLineDrawer : MonoBehaviour {
    GrapplingHook grappling;  

    // 각 오브젝트의 충돌 시 로프의 속도 및 거리 변경 
    public float lineSpeed;
    public float lineEnemySpeed;
    public float lineMaxDistance;
    public float lineEnemyMaxDistance;

    public LineRenderer lineRenderer;   // LinRenderer 컴포넌트 
    public LayerMask collisionMask;     // 충돌을 감지할 레이어 설정
    public Material lineMaterial;       // Line 의 머티리얼을 지정 

    public Color ringColor;             // 로프 걸수 있는 부분의 레이어 색 
    public Color enemyColor;            // 적을 가리킬 때의 색 


    void Start() {
        // 컴포넌트 초기화 
        lineRenderer = GetComponent<LineRenderer>();
        grappling = GetComponent<GrapplingHook>();

        // LineRenderer 초기 설정 
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2; 
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
        // z 좌표를 0으로 설정하여 2D 평면에 그리기
        mousePosition.z = 0; 

        // 시작점과 방향을 설정
        Vector3 startPoint = transform.position;
        Vector3 direction = (mousePosition - startPoint).normalized;

        // Ray를 발사하여 충돌 지점을 찾음
        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, Mathf.Infinity, collisionMask);

        if (hit.collider != null) {
            // 충돌 지점까지 선을 그림
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, hit.point);

            // 충돌한 오브젝트에 따라 색을 변경 및 로프의 속도, 최대 길이 변경 
            // 11번 Layer : Enemy
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
