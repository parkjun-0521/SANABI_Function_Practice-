using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 잔상 효과를 만드는 스크립트
 * 
 * 캐릭터가 순간적인 힘으로 이동할 때 잔상이 생기는 효과를 구현하기 위해 작성 
**/

public class AfterImage : MonoBehaviour
{
    public ParticleSystem particle;             // 잔상을 나타낼 파티클 
    SpriteRenderer spriteRenderer;              // 잔상을 나타낼 오브젝트의 스프라이트 
    ParticleSystemRenderer particleRenderer;    // 잔상을 나타낼 오브젝트의 파티클이벤트

    void Start() {
        // 컴포넌트 할당
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
    }

    void Update() {
        if (particle != null) {
            // 좌, 우가 변경이 될때 서로 다르게 적용 
            if (spriteRenderer.flipX) {
                // 파티클이 나타나게될 위치 좌표 
                particle.transform.localPosition = new Vector3(0.05f, -0.175f, 0);
                // ParticleSystemRenderer의 flip을 사용하여 X축 반전
                particleRenderer.flip = new Vector3(1, 0, 0); 
            }
            else {
                // 파티클이 나타나게될 위치 좌표 
                particle.transform.localPosition = new Vector3(-0.07f, -0.175f, 0);
                particleRenderer.flip = Vector3.zero;
            }
        }
    }
}
