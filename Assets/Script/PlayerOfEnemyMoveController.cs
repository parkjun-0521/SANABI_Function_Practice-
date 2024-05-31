using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameKeyboard;

/**
 * 적을 조종하는 스크립트 
 * 
 * 플레이어가 공격 시 적에게 돌진하는데 이후 적의 방향을 조종할 수 있다. ( 좌, 우만 가능 ) 
**/

public class PlayerOfEnemyMoveController : MonoBehaviour
{
    public int id;

    GameKeyboard Keyboard;
    SpriteRenderer spriteRenderer;

    void Start() {
        Keyboard = GameKeyboard.instance.GetComponent<GameKeyboard>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); 
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

                spriteRenderer.flipX = true;
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove))) {
                Player playerLogic = GameManager.instance.player.GetComponent<Player>();
                transform.position += Vector3.right * playerLogic.speed * Time.deltaTime;
                playerLogic.spriteRenderer.flipX = false;
                playerLogic.wallCheck.transform.localPosition = new Vector2(0.3f, 0f);
                playerLogic.wallCheck.gameObject.SetActive(false);

                spriteRenderer.flipX = false;
            }
        }
    }
}
