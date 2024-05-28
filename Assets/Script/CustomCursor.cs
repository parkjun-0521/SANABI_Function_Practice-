using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour {
    // 커서 이미지
    public Texture2D cursorTexture;

    void Start() {
        // 커서의 중심점 설정 (핫스팟을 이미지의 중심으로 설정)
        Vector2 hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);

        // 커서를 변경하고 핫스팟을 설정
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}