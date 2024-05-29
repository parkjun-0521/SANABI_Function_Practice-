using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    [SerializeField] protected float speed;
    [SerializeField] protected float curDelay;
    [SerializeField] protected float maxDelay;

}
