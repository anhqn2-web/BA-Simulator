using UnityEngine;
using UnityEditor;

public class OfficeLayoutBuilder : EditorWindow
{
    [Header("1. Kích Thước Lưới Mô-Đun")]
    public float tileSize = 1.0f;

    [Header("2. Kiến Trúc & Vách Ngăn")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject glassWallPrefab;
    public GameObject doorPrefab;

    [Header("3. Dãy Sau - Khu Lễ Tân & Bếp")]
    public GameObject receptionCounter;
    public GameObject sofaSet;
    public GameObject turnstileGate;
    public GameObject kitchenLShape;
    public GameObject diningTable;
    public GameObject fridge;

    [Header("4. Dãy Trước - Phòng Đào Tạo & Làm Việc")]
    public GameObject deskRowTraining;
    public GameObject deskClusterOffice;
    public GameObject projectorScreen;
    public GameObject whiteboard;
    public GameObject tvLoungeSet;
    public GameObject plantPot;

    private Vector2 scrollPos;

    [MenuItem("Tools/Office Layout Builder")]
    public static void ShowWindow()
    {
        GetWindow<OfficeLayoutBuilder>("Office Layout Builder");
    }

    private void OnGUI()
    {
        GUILayout.Label("DỰNG VĂN PHÒNG CHUẨN KẾT CẤU SƠ ĐỒ", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // 1. Kích thước
        tileSize = EditorGUILayout.FloatField("Kích thước Tile (m):", tileSize);
        EditorGUILayout.Space(10);

        // 2. Kiến trúc
        GUILayout.Label("2. Kiến Trúc & Vách Ngăn", EditorStyles.boldLabel);
        floorPrefab = (GameObject)EditorGUILayout.ObjectField("Sàn (Floor):", floorPrefab, typeof(GameObject), false);
        wallPrefab = (GameObject)EditorGUILayout.ObjectField("Tường (Wall):", wallPrefab, typeof(GameObject), false);
        glassWallPrefab = (GameObject)EditorGUILayout.ObjectField("Vách kính:", glassWallPrefab, typeof(GameObject), false);
        doorPrefab = (GameObject)EditorGUILayout.ObjectField("Cửa (Door):", doorPrefab, typeof(GameObject), false);
        EditorGUILayout.Space(10);

        // 3. Dãy sau
        GUILayout.Label("3. Dãy Sau - Khu Lễ Tân & Bếp", EditorStyles.boldLabel);
        receptionCounter = (GameObject)EditorGUILayout.ObjectField("Bàn lễ tân:", receptionCounter, typeof(GameObject), false);
        sofaSet = (GameObject)EditorGUILayout.ObjectField("Sofa tiếp khách:", sofaSet, typeof(GameObject), false);
        turnstileGate = (GameObject)EditorGUILayout.ObjectField("Cổng xoay:", turnstileGate, typeof(GameObject), false);
        kitchenLShape = (GameObject)EditorGUILayout.ObjectField("Tủ bếp chữ L:", kitchenLShape, typeof(GameObject), false);
        diningTable = (GameObject)EditorGUILayout.ObjectField("Bàn ăn Pantry:", diningTable, typeof(GameObject), false);
        fridge = (GameObject)EditorGUILayout.ObjectField("Tủ lạnh:", fridge, typeof(GameObject), false);
        EditorGUILayout.Space(10);

        // 4. Dãy trước
        GUILayout.Label("4. Dãy Trước - Phòng Đào Tạo & Làm Việc", EditorStyles.boldLabel);
        deskRowTraining = (GameObject)EditorGUILayout.ObjectField("Bàn đào tạo:", deskRowTraining, typeof(GameObject), false);
        deskClusterOffice = (GameObject)EditorGUILayout.ObjectField("Cụm bàn nhân viên:", deskClusterOffice, typeof(GameObject), false);
        projectorScreen = (GameObject)EditorGUILayout.ObjectField("Màn chiếu lớn:", projectorScreen, typeof(GameObject), false);
        whiteboard = (GameObject)EditorGUILayout.ObjectField("Bảng trắng:", whiteboard, typeof(GameObject), false);
        tvLoungeSet = (GameObject)EditorGUILayout.ObjectField("Cụm TV Lounge:", tvLoungeSet, typeof(GameObject), false);
        plantPot = (GameObject)EditorGUILayout.ObjectField("Cây cảnh:", plantPot, typeof(GameObject), false);

        EditorGUILayout.Space(15);
        if (GUILayout.Button("DỰNG MẶT BẰNG VĂN PHÒNG", GUILayout.Height(40)))
        {
            BuildOfficeByImageStructure();
        }

        EditorGUILayout.EndScrollView();
    }

    private void BuildOfficeByImageStructure()
    {
        GameObject root = new GameObject("Office_Structure_Root");
        Undo.RegisterCreatedObjectUndo(root, "Build Office Structure");

        Transform archGroup = new GameObject("01_Architecture").transform;
        Transform furnitureGroup = new GameObject("02_Furniture").transform;

        archGroup.SetParent(root.transform);
        furnitureGroup.SetParent(root.transform);

        int width = 32;
        int depth = 18;

        // 1. Sàn
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                Spawn(floorPrefab, pos, Quaternion.identity, archGroup);
            }
        }

        // 2. Vách tường giữa (Z = 9)
        for (int x = 0; x < width; x++)
        {
            if (x >= 14 && x <= 18) continue; // Lối đi
            Vector3 pos = new Vector3(x * tileSize, 0, 9 * tileSize);
            Spawn(wallPrefab, pos, Quaternion.identity, archGroup);
        }

        // 3. Tường kính mặt sau (Z = 18)
        for (int x = 0; x < width; x++)
        {
            Vector3 pos = new Vector3(x * tileSize, 0, depth * tileSize);
            Spawn(glassWallPrefab != null ? glassWallPrefab : wallPrefab, pos, Quaternion.identity, archGroup);
        }

        // --- KHU VỰC DÃY SAU (Z > 9) ---
        Vector3 receptionPos = new Vector3(8 * tileSize, 0, 14 * tileSize);
        Spawn(receptionCounter, receptionPos, Quaternion.identity, furnitureGroup);
        Spawn(sofaSet, new Vector3(4 * tileSize, 0, 14 * tileSize), Quaternion.Euler(0, 90, 0), furnitureGroup);
        Spawn(turnstileGate, new Vector3(12 * tileSize, 0, 10 * tileSize), Quaternion.identity, furnitureGroup);

        Vector3 kitchenPos = new Vector3(24 * tileSize, 0, 16 * tileSize);
        Spawn(kitchenLShape, kitchenPos, Quaternion.identity, furnitureGroup);
        Spawn(fridge, kitchenPos + new Vector3(6 * tileSize, 0, 0), Quaternion.identity, furnitureGroup);
        Spawn(diningTable, new Vector3(22 * tileSize, 0, 12 * tileSize), Quaternion.identity, furnitureGroup);
        Spawn(diningTable, new Vector3(27 * tileSize, 0, 12 * tileSize), Quaternion.identity, furnitureGroup);

        // --- KHU VỰC DÃY TRƯỚC (Z < 9) ---
        for (int z = 0; z < 7; z++)
        {
            Spawn(wallPrefab, new Vector3(12 * tileSize, 0, z * tileSize), Quaternion.Euler(0, 90, 0), archGroup);
        }
        for (int row = 1; row <= 4; row++)
        {
            for (int col = 1; col <= 3; col++)
            {
                Vector3 deskPos = new Vector3(col * 3.5f * tileSize, 0, row * 1.5f * tileSize);
                Spawn(deskRowTraining, deskPos, Quaternion.identity, furnitureGroup);
            }
        }
        Spawn(whiteboard, new Vector3(10 * tileSize, 0, 5 * tileSize), Quaternion.Euler(0, 270, 0), furnitureGroup);

        Vector3 officeCenter = new Vector3(18 * tileSize, 0, 4 * tileSize);
        Spawn(deskClusterOffice, officeCenter, Quaternion.identity, furnitureGroup);
        Spawn(deskClusterOffice, officeCenter + new Vector3(5 * tileSize, 0, 0), Quaternion.identity, furnitureGroup);
        Spawn(projectorScreen, new Vector3(15 * tileSize, 0, 8 * tileSize), Quaternion.identity, furnitureGroup);

        Vector3 loungePos = new Vector3(28 * tileSize, 0, 4 * tileSize);
        Spawn(tvLoungeSet, loungePos, Quaternion.Euler(0, 270, 0), furnitureGroup);

        Spawn(plantPot, new Vector3(13 * tileSize, 0, 1 * tileSize), Quaternion.identity, furnitureGroup);
        Spawn(plantPot, new Vector3(25 * tileSize, 0, 7 * tileSize), Quaternion.identity, furnitureGroup);

        Debug.Log("Đã dựng xong văn phòng chuẩn kết cấu!");
    }

    private void Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
    {
        if (prefab == null) return;
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.transform.SetParent(parent);
    }
}