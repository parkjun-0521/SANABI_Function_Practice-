using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �̱��� ��� 
    public static GameManager instance;

    public Player player;       // Player ��ũ��Ʈ 
    public Pooling poolManager; // Pooling ��ũ��Ʈ 

    void Awake() {
        instance = this;
    }
}
