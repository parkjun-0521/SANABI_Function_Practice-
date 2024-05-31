using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * �ܻ� ȿ���� ����� ��ũ��Ʈ
 * 
 * ĳ���Ͱ� �������� ������ �̵��� �� �ܻ��� ����� ȿ���� �����ϱ� ���� �ۼ� 
**/

public class AfterImage : MonoBehaviour
{
    public ParticleSystem particle;             // �ܻ��� ��Ÿ�� ��ƼŬ 
    SpriteRenderer spriteRenderer;              // �ܻ��� ��Ÿ�� ������Ʈ�� ��������Ʈ 
    ParticleSystemRenderer particleRenderer;    // �ܻ��� ��Ÿ�� ������Ʈ�� ��ƼŬ�̺�Ʈ

    void Start() {
        // ������Ʈ �Ҵ�
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
    }

    void Update() {
        if (particle != null) {
            // ��, �찡 ������ �ɶ� ���� �ٸ��� ���� 
            if (spriteRenderer.flipX) {
                // ��ƼŬ�� ��Ÿ���Ե� ��ġ ��ǥ 
                particle.transform.localPosition = new Vector3(0.05f, -0.175f, 0);
                // ParticleSystemRenderer�� flip�� ����Ͽ� X�� ����
                particleRenderer.flip = new Vector3(1, 0, 0); 
            }
            else {
                // ��ƼŬ�� ��Ÿ���Ե� ��ġ ��ǥ 
                particle.transform.localPosition = new Vector3(-0.07f, -0.175f, 0);
                particleRenderer.flip = Vector3.zero;
            }
        }
    }
}
