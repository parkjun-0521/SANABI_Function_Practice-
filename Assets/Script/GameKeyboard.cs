using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Input Manager 의 키 값을 관리하는 스크립트 
 * 
 * 키보드의 키값을 한번에 관리하기 위해 구현 
 * 추후 설정에서 키변경을 만들기 위해 미리 관리하는 것 
**/

public class GameKeyboard : MonoBehaviour
{
    // 키보드매니저 싱글톤 
    public static GameKeyboard instance;

    // 열거형 변수 선언 
    public enum KeyCodeTypes {
        LeftMove,
        RightMove,
        UpMove,
        DownMove,
        Jump,
        Hook,
        Attack,
        HookAcceleration
    }

    // 딕셔너리로 키 관리 
    private Dictionary<KeyCodeTypes, KeyCode> keyMappings;

    void Awake() {
        instance = this;
        // 딕셔너리 초기화 
        keyMappings = new Dictionary<KeyCodeTypes, KeyCode>();
        
        // 각 디셔너리 키에 맞는 키보드 값을 추가 
        keyMappings[KeyCodeTypes.LeftMove] = KeyCode.A;
        keyMappings[KeyCodeTypes.RightMove] = KeyCode.D;
        keyMappings[KeyCodeTypes.UpMove] = KeyCode.W;
        keyMappings[KeyCodeTypes.DownMove] = KeyCode.S;
        keyMappings[KeyCodeTypes.Jump] = KeyCode.Space;
        keyMappings[KeyCodeTypes.Hook] = KeyCode.Mouse0;
        keyMappings[KeyCodeTypes.Attack] = KeyCode.Mouse1;
        keyMappings[KeyCodeTypes.HookAcceleration] = KeyCode.LeftShift;        
    }

    public KeyCode GetKeyCode( KeyCodeTypes action ) {
        // 키값 반환 
        return keyMappings[action];
    }

    public void SetKeyCode( KeyCodeTypes action, KeyCode keyCode ) {
        // 키값 설정 
        keyMappings[action] = keyCode;
    }

    void Update() {
        // 설정 창 띄웠을 때 TextInput 창의 값을 반영시키는 로직 추가 SetKeyCode() 사용 
    }
}
