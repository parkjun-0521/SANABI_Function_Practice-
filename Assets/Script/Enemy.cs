using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;                              // ���� ü�� 
    [SerializeField] protected float speed;         // ���� �̵� �ӵ� 
    [SerializeField] protected float curDelay;      // ���� ���� ���� ������ 
    [SerializeField] protected float maxDelay;      // ���� max ������

}
