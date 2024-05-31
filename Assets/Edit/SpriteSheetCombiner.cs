using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpriteSheetCombiner : MonoBehaviour
{
    [MenuItem("Tools/Combine Sprites")]
    public static void CombineSprites() {
        // 스프라이트를 가져올 폴더 경로
        string folderPath = "Assets/Image/NearEnemy";
        // 결합될 텍스처의 크기 (320x320 픽셀 스프라이트 99개를 위한 크기 설정)
        int textureWidth = 700 * 10; // 가로로 10개의 스프라이트 (320 * 10)
        int textureHeight = 450 * 6; // 세로로 10개의 스프라이트 (320 * 10, 99개의 이미지 포함 가능)

        // 폴더에서 모든 스프라이트 로드
        string[] spritePaths = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
        List<Sprite> sprites = new List<Sprite>();

        // 파일명을 기준으로 경로 정렬
        spritePaths = spritePaths.OrderBy(path => {
            string assetPath = AssetDatabase.GUIDToAssetPath(path);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            int fileNumber;
            int.TryParse(fileName, out fileNumber);
            return fileNumber;
        }).ToArray();

        foreach (var spritePath in spritePaths) {
            string path = AssetDatabase.GUIDToAssetPath(spritePath);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            // 텍스처 읽기 가능 설정
            if (textureImporter != null) {
                textureImporter.isReadable = true;
                textureImporter.SaveAndReimport();
            }

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null) {
                sprites.Add(sprite);
                Debug.Log("Loaded sprite: " + sprite.name);
            }
            else {
                Debug.LogError("Failed to load sprite at path: " + path);
            }
        }

        Texture2D combinedTexture = new Texture2D(textureWidth, textureHeight);

        int xOffset = 0;
        int yOffset = textureHeight - 450; // 초기 yOffset을 맨 위로 설정
        //int maxHeight = 320; // 이미지 높이

        // 스프라이트를 왼쪽 상단에서 오른쪽 하단으로 배치
        for (int i = 0; i < 57; i++) {
            if (i < sprites.Count) {
                Sprite sprite = sprites[i];
                Texture2D texture = sprite.texture;
                Rect rect = sprite.rect;
                int width = (int)rect.width;
                int height = (int)rect.height;

                // 현재 위치에 스프라이트 복사
                if (texture != null) {
                    Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, width, height);
                    combinedTexture.SetPixels(xOffset, yOffset, width, height, pixels);
                }
                else {
                    Debug.LogError("Texture is null for sprite: " + sprite.name);
                }
            }

            // 오프셋 업데이트
            xOffset += 700;
            if (xOffset + 700 > textureWidth) {
                xOffset = 0;
                yOffset -= 450; // yOffset을 감소시켜 아래로 이동
            }
        }

        // 텍스처 적용
        combinedTexture.Apply();

        // 결합된 텍스처 저장
        byte[] bytes = combinedTexture.EncodeToPNG();
        string combinedTexturePath = "Assets/NearEnemy.png";
        File.WriteAllBytes(combinedTexturePath, bytes);

        // 파일이 생성된 경로 출력
        Debug.Log("Combined texture saved to: " + combinedTexturePath);

        // 파일을 에셋으로 임포트
        AssetDatabase.ImportAsset(combinedTexturePath);
        AssetDatabase.Refresh();

        // 임포트된 텍스처 가져오기
        Texture2D importedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(combinedTexturePath);
        Debug.Log("Imported combined texture: " + importedTexture.name);
    }
}
