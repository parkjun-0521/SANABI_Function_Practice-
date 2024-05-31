using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameKeyboard;

/**
 * 로프 발사 구현 스크립트 
 * 
 * 로프 액션을 위해 로프를 발사를 구현
 * 현재 마우스의 위치 방향으로 로프가 발사되도록 함 
**/

public class GrapplingHook : MonoBehaviour {
    public float speed;             // 로프 발사 속도 
    public float backSpeed= 200;    // 로프 돌아오는 속도 
    public float maxDistance;       // 로프 최대 거리 

    public bool isHookAction;       // 로프가 날아가는지 판단 
    public bool isLineMax;          // 로프가 최대거리인지 판단 
    public bool isAttach;           // 로프가 붙어있는지 판단 

    public bool isGround = false;   // 바닥인지 확인 변수 ( 로프를 걸고 이동을 하려는 로직 ) 

    Vector2 mousedir;               // 현재 마우스의 방향을 저장하기 위한 변수

    // 컴포넌트 및 스크립트 
    public LineRenderer line;      
    public Transform hook;
    GameKeyboard Keyboard;
    Player player;

    void Awake() {
        Keyboard = GameKeyboard.instance.GetComponent<GameKeyboard>();
        player = GetComponent<Player>();    
    }

    void Start() {
        // LinRenderer의 초기값 설정 
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        // 처음 위치, 마지막 위치 설정 
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
    }

    void Update() {
        // 적을 공격하여 조종 할 때는 로프 발사를 못하게 제한 
        if (GameManager.instance.player.GetComponent<Player>().isEnemyControll) {
            return;
        }

        // 시작 위치와 마지막 위치를 지속적으로 갱신 
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        // 로프 발사 ( 마우스 왼클릭 ) 
        if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Hook)) && !isHookAction) {
            // 발사 초기에 변수 초기화 
            isAttach = false;
            isLineMax = false;
            hook.GetComponent<Hooking>().joint2D.enabled = false;
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
            hook.gameObject.SetActive(false);

            // 훅의 위치 조정 
            hook.position = transform.position;
            // 현재 마우스 포인터와 Player의 위치 사이의 방향을 구함 
            mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            isHookAction = true;
            isLineMax = false;
            hook.gameObject.SetActive(true);
        }

        // 로프 액션 처리 
        if (isHookAction && !isLineMax && !isAttach) {      // 로프가 날아가는데 최대거리에 도달했을 경우 
            hook.Translate(mousedir.normalized * Time.deltaTime * speed);
            // Player의 위치와 hook의 위치의 길이가 최대가 넘을 때 
            if (Vector2.Distance(transform.position, hook.position) > maxDistance) {
                isLineMax = true;
            }
        }
        else if (isHookAction && isLineMax) {               // 최대거리에 도달했을 때 돌아오는 경우 
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * backSpeed);
            // 로프의 길이가 Player와 거의 근접했을 때 변수 초기화 
            if (Vector2.Distance(transform.position, hook.position) < 0.1f) {
                isHookAction = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
        }       
        else if (isAttach) {                                // 로프가 특정 오브젝트에 붙었을 경우 
            // 로프가 붙었을 때 마우스 버튼을 땠을 경우 ( 즉, 로프가 떨어질 때 ) 
            if (!Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.Hook)) && !player.isAttack) { 
                // 플레이어 변수 초기화 
                player.isJumpCheck = false;
                player.jumpCount = 1;
                player.isAcceleration = false;
                player.rigid.gravityScale = 2;

                // 변수 초기화 
                isAttach = false;
                isHookAction = false;
                isLineMax = false;
                
                // 로프 비활성화  
                hook.GetComponent<Hooking>().joint2D.enabled = false;
                hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
                hook.gameObject.SetActive(false);

                // 최대 거리 제한 
                if(Vector2.Distance(transform.position, hook.position) > maxDistance) {
                    hook.GetComponent<Hooking>().joint2D.distance = maxDistance;
                }

                // 로프가 붙어있는 부모 오브젝트 초기화 
                hook.SetParent(hook.gameObject.GetComponent<Hooking>().originalParent);
                hook.gameObject.GetComponent<Hooking>().hookedObject = null;
            }

            // 플레이어가 땅에 있고 후크가 걸려있는지 확인
            if (isGround) {
                AdjustHookLength();
            }
        }
    }

    // 깔끔하게 동작하지 않음 ( 문제점을 찾아봐야 될 꺼 같음 ) 
    void AdjustHookLength() {
        // 현재 플레이어 위치와 후크 위치 간의 거리 계산
        float distanceChange = speed * Time.deltaTime;

        Hooking hookScript = hook.gameObject.GetComponent<Hooking>();
        float targetDistance = hookScript.joint2D.distance;             // 목표 거리

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
        // 로프가 걸려있고 지면에 닿은 상태일 때 
        if (collision.gameObject.CompareTag("Ground") && isAttach) {
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = false;
            isGround = true;
        }
    }

    private void OnCollisionExit2D( Collision2D collision ) {
        // 로프가 걸려있고 지면에 닿은 상태일 때 
        if (collision.gameObject.CompareTag("Ground") && isAttach) {
            hook.GetComponent<Hooking>().distanceJoint2D.enabled = true;
            // 로프의 거리를 현재 거리로 변경 
            hook.GetComponent<Hooking>().distanceJoint2D.distance = hook.GetComponent<Hooking>().joint2D.distance;
            isGround = false;
        }
    }
}