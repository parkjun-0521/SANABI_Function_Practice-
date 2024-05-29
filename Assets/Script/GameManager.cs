using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public Pooling poolManager;

    void Awake() {
        instance = this;
    }
}
