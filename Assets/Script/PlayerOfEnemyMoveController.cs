using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameKeyboard;

public class PlayerOfEnemyMoveController : MonoBehaviour
{
    public int id;

    GameKeyboard Keyboard;

    void Start() {
        Keyboard = GameKeyboard.instance.GetComponent<GameKeyboard>();
    }

    void Update() {
        if (GameManager.instance.player.GetComponent<Player>().isEnemyControll && 
            id == GameManager.instance.player.GetComponent<Player>().collierEnemyId) {
            if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove))) {
                Player playerLogic = GameManager.instance.player.GetComponent<Player>();
                transform.position += Vector3.left * playerLogic.speed * Time.deltaTime;
                playerLogic.spriteRenderer.flipX = true;
                playerLogic.wallCheck.transform.localPosition = new Vector2(-0.3f, 0f);
                playerLogic.wallCheck.gameObject.SetActive(false);
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove))) {
                Player playerLogic = GameManager.instance.player.GetComponent<Player>();
                transform.position += Vector3.right * playerLogic.speed * Time.deltaTime;
                playerLogic.spriteRenderer.flipX = false;
                playerLogic.wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
                playerLogic.wallCheck.gameObject.SetActive(false);
            }
        }
    }
}
