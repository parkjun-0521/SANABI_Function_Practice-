using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ���� �߻� �Ÿ� ǥ�� ��ũ��Ʈ  
 * 
 * ������ �߻�� �Ÿ��� �̸� �����ֱ� ���� ���� 
**/


public class MouseLineDrawer : MonoBehaviour {
    GrapplingHook grappling;  

    // �� ������Ʈ�� �浹 �� ������ �ӵ� �� �Ÿ� ���� 
    public float lineSpeed;
    public float lineEnemySpeed;
    public float lineMaxDistance;
    public float lineEnemyMaxDistance;

    public LineRenderer lineRenderer;   // LinRenderer ������Ʈ 
    public LayerMask collisionMask;     // �浹�� ������ ���̾� ����
    public Material lineMaterial;       // Line �� ��Ƽ������ ���� 

    public Color ringColor;             // ���� �ɼ� �ִ� �κ��� ���̾� �� 
    public Color enemyColor;            // ���� ����ų ���� �� 


    void Start() {
        // ������Ʈ �ʱ�ȭ 
        lineRenderer = GetComponent<LineRenderer>();
        grappling = GetComponent<GrapplingHook>();

        // LineRenderer �ʱ� ���� 
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
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // z ��ǥ�� 0���� �����Ͽ� 2D ��鿡 �׸���
        mousePosition.z = 0; 

        // �������� ������ ����
        Vector3 startPoint = transform.position;
        Vector3 direction = (mousePosition - startPoint).normalized;

        // Ray�� �߻��Ͽ� �浹 ������ ã��
        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, Mathf.Infinity, collisionMask);

        if (hit.collider != null) {
            // �浹 �������� ���� �׸�
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, hit.point);

            // �浹�� ������Ʈ�� ���� ���� ���� �� ������ �ӵ�, �ִ� ���� ���� 
            // 11�� Layer : Enemy
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
            // �浹 ������ ������ ���콺 ��ġ���� ���� �׸�
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, mousePosition);
            lineMaterial.SetColor("_Color", ringColor);
            grappling.speed = lineSpeed;
            grappling.maxDistance = lineMaxDistance;
        }
    }
}
