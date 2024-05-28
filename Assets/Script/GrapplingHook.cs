using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameKeyboard;

public class GrapplingHook : MonoBehaviour {
    public LineRenderer line;       // 각각 변수는 assinged 를 해줘야 한다. 
    public Transform hook;

    public float speed = 10;
    public float backSpeed= 200;
    public float maxDistance;

    Vector2 mousedir;           // 현재 마우스의 방향을 저장하기 위한 변수 

    public bool isHookAction;
    public bool isLineMax;

    public bool isAttach;
    public bool isGround = false;

    GameKeyboard Keyboard;
    Player player;

    void Awake() {
        Keyboard = GameKeyboard.instance.GetComponent<GameKeyboard>();
        player = GetComponent<Player>();    
    }

    void Start() {
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    void Update() {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Hook)) && !isHookAction) {

            isAttach = false;
            isLineMax = false;
            hook.GetComponent<Hooking>().joint2D.enabled = false;
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
            hook.gameObject.SetActive(false);

            // 현재 마우스 포인터와 Player의 위치 사이의 방향을 구함 
            hook.position = transform.position;
            mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            isHookAction = true;
            isLineMax = false;
            hook.gameObject.SetActive(true);
        }

        if (isHookAction && !isLineMax && !isAttach) {
            hook.Translate(mousedir.normalized * Time.deltaTime * speed);
            // Player의 위치와 hook의 위치의 길이가 5가 넘을 때 
            if (Vector2.Distance(transform.position, hook.position) > maxDistance) {
                isLineMax = true;
            }
        }
        else if (isHookAction && isLineMax) {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * backSpeed);
            if (Vector2.Distance(transform.position, hook.position) < 0.1f) {
                isHookAction = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
        }
        else if (isAttach) {
            if (!Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.Hook)) && !player.isAttack) { 
                // 플레이어 변수 초기화 
                player.isJumpCheck = false;
                player.jumpCount = 1;
                player.isAcceleration = false;
                player.rigid.gravityScale = 2;

                isAttach = false;
                isHookAction = false;
                isLineMax = false;
                hook.GetComponent<Hooking>().joint2D.enabled = false;
                hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
                hook.gameObject.SetActive(false);

                if(Vector2.Distance(transform.position, hook.position) > maxDistance) {
                    hook.GetComponent<Hooking>().joint2D.distance = maxDistance;
                }

                hook.SetParent(hook.gameObject.GetComponent<Hooking>().originalParent);
                hook.gameObject.GetComponent<Hooking>().hookedObject = null;
            }

            // 플레이어가 땅에 있고 후크가 걸려있는지 확인
            if (isGround) {
                AdjustHookLength();
            }
        }
    }

    void AdjustHookLength() {
        // 현재 플레이어 위치와 후크 위치 간의 거리 계산
        float distanceChange = speed * Time.deltaTime;

        Hooking hookScript = hook.gameObject.GetComponent<Hooking>();

        float targetDistance = hookScript.joint2D.distance; // 목표 거리

        // 후크가 플레이어보다 오른쪽에 있는 경우
        if (hook.position.x > transform.position.x) {
            if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove))) {
                // 플레이어가 왼쪽으로 이동하면 로프 길이 증가
                targetDistance = Mathf.Min(hookScript.joint2D.distance + distanceChange, maxDistance);
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove))) {
                // 플레이어가 오른쪽으로 이동하면 로프 길이 감소
                targetDistance = Mathf.Max(hookScript.joint2D.distance - distanceChange, 0);
            }
        }
        // 후크가 플레이어보다 왼쪽에 있는 경우
        else if (hook.position.x < transform.position.x) {
            if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove))) {
                // 플레이어가 왼쪽으로 이동하면 로프 길이 감소
                targetDistance = Mathf.Max(hookScript.joint2D.distance - distanceChange, 0);
            }
            else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove))) {
                // 플레이어가 오른쪽으로 이동하면 로프 길이 증가
                targetDistance = Mathf.Min(hookScript.joint2D.distance + distanceChange, maxDistance);
            }
        }
        // 후크와 플레이어의 위치가 같은 경우
        else {
            // 플레이어가 이동해도 로프 길이를 유지
            targetDistance = hookScript.joint2D.distance;
        }

        // 선형 보간을 사용하여 목표 거리로 부드럽게 이동
        hookScript.joint2D.distance = Mathf.Lerp(hookScript.joint2D.distance, targetDistance, Time.deltaTime * player.speed * 10);
    }
    void OnCollisionStay2D( Collision2D collision ) {
        if (collision.gameObject.CompareTag("Ground") && isAttach) {
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
            isGround = true;
        }
    }

    private void OnCollisionExit2D( Collision2D collision ) {
        if (collision.gameObject.CompareTag("Ground") && isAttach) {
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = true;
            hook.GetComponent<Hooking>().distanceJoint2D.distance = hook.GetComponent<Hooking>().distanceJoint2D.distance;
            isGround = false;
        }
    }
}
