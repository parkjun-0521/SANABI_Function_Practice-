using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 사용 
    public static GameManager instance;

    public Player player;       // Player 스크립트 
    public Pooling poolManager; // Pooling 스크립트 

    void Awake() {
        instance = this;
    }
}
