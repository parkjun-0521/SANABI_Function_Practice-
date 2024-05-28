using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public ParticleSystem particle;
    SpriteRenderer spriteRenderer;
    ParticleSystemRenderer particleRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
    }

    void Update() {
        if (particle != null) {
            if (spriteRenderer.flipX) {
                // ParticleSystemRenderer의 flip을 사용하여 X축 반전
                particle.transform.localPosition = new Vector3(0.05f, -0.175f, 0);
                particleRenderer.flip = new Vector3(1, 0, 0); // X축 반전
            }
            else {
                // 기본 방향으로 설정
                particle.transform.localPosition = new Vector3(-0.07f, -0.175f, 0);
                particleRenderer.flip = Vector3.zero;
            }
        }
    }
}
