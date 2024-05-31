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
        // ��������Ʈ�� ������ ���� ���
        string folderPath = "Assets/Image/NearEnemy";
        // ���յ� �ؽ�ó�� ũ�� (320x320 �ȼ� ��������Ʈ 99���� ���� ũ�� ����)
        int textureWidth = 700 * 10; // ���η� 10���� ��������Ʈ (320 * 10)
        int textureHeight = 450 * 6; // ���η� 10���� ��������Ʈ (320 * 10, 99���� �̹��� ���� ����)

        // �������� ��� ��������Ʈ �ε�
        string[] spritePaths = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
        List<Sprite> sprites = new List<Sprite>();

        // ���ϸ��� �������� ��� ����
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

            // �ؽ�ó �б� ���� ����
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
        int yOffset = textureHeight - 450; // �ʱ� yOffset�� �� ���� ����
        //int maxHeight = 320; // �̹��� ����

        // ��������Ʈ�� ���� ��ܿ��� ������ �ϴ����� ��ġ
        for (int i = 0; i < 57; i++) {
            if (i < sprites.Count) {
                Sprite sprite = sprites[i];
                Texture2D texture = sprite.texture;
                Rect rect = sprite.rect;
                int width = (int)rect.width;
                int height = (int)rect.height;

                // ���� ��ġ�� ��������Ʈ ����
                if (texture != null) {
                    Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, width, height);
                    combinedTexture.SetPixels(xOffset, yOffset, width, height, pixels);
                }
                else {
                    Debug.LogError("Texture is null for sprite: " + sprite.name);
                }
            }

            // ������ ������Ʈ
            xOffset += 700;
            if (xOffset + 700 > textureWidth) {
                xOffset = 0;
                yOffset -= 450; // yOffset�� ���ҽ��� �Ʒ��� �̵�
            }
        }

        // �ؽ�ó ����
        combinedTexture.Apply();

        // ���յ� �ؽ�ó ����
        byte[] bytes = combinedTexture.EncodeToPNG();
        string combinedTexturePath = "Assets/NearEnemy.png";
        File.WriteAllBytes(combinedTexturePath, bytes);

        // ������ ������ ��� ���
        Debug.Log("Combined texture saved to: " + combinedTexturePath);

        // ������ �������� ����Ʈ
        AssetDatabase.ImportAsset(combinedTexturePath);
        AssetDatabase.Refresh();

        // ����Ʈ�� �ؽ�ó ��������
        Texture2D importedTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(combinedTexturePath);
        Debug.Log("Imported combined texture: " + importedTexture.name);
    }
}
