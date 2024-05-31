using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;                              // 몬스터 체력 
    [SerializeField] protected float speed;         // 몬스터 이동 속도 
    [SerializeField] protected float curDelay;      // 몬스터 현재 공격 딜레이 
    [SerializeField] protected float maxDelay;      // 몬스터 max 딜레이

}
