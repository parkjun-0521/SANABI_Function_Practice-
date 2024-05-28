using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class GameKeyboard : MonoBehaviour
{
    public static GameKeyboard instance;

    public enum KeyCodeTypes {
        LeftMove,
        RightMove,
        UpMove,
        DownMove,
        Jump,
        Hook,
        HookAcceleration
    }

    private Dictionary<KeyCodeTypes, KeyCode> keyMappings;

    void Awake() {
        instance = this;

        keyMappings = new Dictionary<KeyCodeTypes, KeyCode>();
        
        keyMappings[KeyCodeTypes.LeftMove] = KeyCode.A;
        keyMappings[KeyCodeTypes.RightMove] = KeyCode.D;
        keyMappings[KeyCodeTypes.UpMove] = KeyCode.W;
        keyMappings[KeyCodeTypes.DownMove] = KeyCode.S;
        keyMappings[KeyCodeTypes.Jump] = KeyCode.Space;
        keyMappings[KeyCodeTypes.Hook] = KeyCode.Mouse0;
        keyMappings[KeyCodeTypes.HookAcceleration] = KeyCode.LeftShift;        
    }

    public KeyCode GetKeyCode( KeyCodeTypes action ) {
        return keyMappings[action];
    }

    public void SetKeyCode( KeyCodeTypes action, KeyCode keyCode ) {
        keyMappings[action] = keyCode;
    }

    void Update() {
        // 설정 창 띄웠을 때 TextInput 창의 값을 반영시키는 로직 추가 SetKeyCode() 사용 
    }
}
