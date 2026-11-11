using UnityEngine;
using UnityEditor;

public class OfficeAutoBuilder : EditorWindow
{
    [Header("Mô-đun Kiến Trúc")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject glassWallPrefab;

    [Header("Mô-đun Nội Thất")]
    public GameObject receptionDeskPrefab;
    public GameObject sofaPrefab;
    public GameObject officeDeskPrefab;
    public GameObject chairPrefab;
    public GameObject meetingTablePrefab;
    public GameObject whiteboardPrefab;
    public GameObject coffeeMachinePrefab;
    public GameObject plantPrefab;

    [MenuItem("Tools/Office Auto Builder")]
    public static void ShowWindow()
    {
        GetWindow<OfficeAutoBuilder>("Office Auto Builder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Công Cụ Dựng Tự Động Sơ Đồ Văn Phòng (Hoàn Chỉnh)", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        floorPrefab = (GameObject)EditorGUILayout.ObjectField("Sàn (Floor):", floorPrefab, typeof(GameObject), false);
        wallPrefab = (GameObject)EditorGUILayout.ObjectField("Tường bao (Wall):", wallPrefab, typeof(GameObject), false);
        glassWallPrefab = (GameObject)EditorGUILayout.ObjectField("Vách kính:", glassWallPrefab, typeof(GameObject), false);

        EditorGUILayout.Space(5);
        receptionDeskPrefab = (GameObject)EditorGUILayout.ObjectField("Bàn lễ tân:", receptionDeskPrefab, typeof(GameObject), false);
        sofaPrefab = (GameObject)EditorGUILayout.ObjectField("Sofa tiếp khách:", sofaPrefab, typeof(GameObject), false);
        officeDeskPrefab = (GameObject)EditorGUILayout.ObjectField("Bàn làm việc:", officeDeskPrefab, typeof(GameObject), false);
        chairPrefab = (GameObject)EditorGUILayout.ObjectField("Ghế làm việc / Ghế họp:", chairPrefab, typeof(GameObject), false);
        meetingTablePrefab = (GameObject)EditorGUILayout.ObjectField("Bàn họp dài:", meetingTablePrefab, typeof(GameObject), false);
        whiteboardPrefab = (GameObject)EditorGUILayout.ObjectField("Bảng trắng:", whiteboardPrefab, typeof(GameObject), false);
        coffeeMachinePrefab = (GameObject)EditorGUILayout.ObjectField("Máy pha cà phê:", coffeeMachinePrefab, typeof(GameObject), false);
        plantPrefab = (GameObject)EditorGUILayout.ObjectField("Cây cảnh:", plantPrefab, typeof(GameObject), false);

        EditorGUILayout.Space(15);
        if (GUILayout.Button("DỰNG HOÀN CHỈNH VĂN PHÒNG", GUILayout.Height(40)))
        {
            BuildCompleteOffice();
        }
    }

    private void BuildCompleteOffice()
    {
        GameObject root = new GameObject("Full_Office_Layout");
        Undo.RegisterCreatedObjectUndo(root, "Build Full Office");

        float tileSize = 2.0f; // Kích thước mỗi ô sàn (2m x 2m)
        int width = 10;        // Rộng 20m
        int depth = 10;        // Dài 20m

        // 1. Trải sàn toàn bộ văn phòng
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int z = -depth / 2; z < depth / 2; z++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                SpawnItem(floorPrefab, pos, Quaternion.identity, root.transform);
            }
        }

        // 2. Dựng tường bao xung quanh
        for (int x = -width / 2; x < width / 2; x++)
        {
            SpawnItem(wallPrefab, new Vector3(x * tileSize, 0, (depth / 2) * tileSize), Quaternion.identity, root.transform);
            SpawnItem(wallPrefab, new Vector3(x * tileSize, 0, (-depth / 2) * tileSize), Quaternion.identity, root.transform);
        }
        for (int z = -depth / 2; z < depth / 2; z++)
        {
            SpawnItem(wallPrefab, new Vector3((-width / 2) * tileSize, 0, z * tileSize), Quaternion.Euler(0, 90, 0), root.transform);
            SpawnItem(wallPrefab, new Vector3((width / 2) * tileSize, 0, z * tileSize), Quaternion.Euler(0, 90, 0), root.transform);
        }

        // 3. Khu 1: Reception (Góc trên bên trái)
        SpawnItem(receptionDeskPrefab, new Vector3(-5, 0, 5), Quaternion.identity, root.transform);
        SpawnItem(sofaPrefab, new Vector3(-8, 0, 5), Quaternion.Euler(0, 90, 0), root.transform);

        // 4. Khu 4: Pantry (Góc trên bên phải)
        SpawnItem(coffeeMachinePrefab, new Vector3(6, 0, 6), Quaternion.identity, root.transform);
        SpawnItem(plantPrefab, new Vector3(8, 0, 7), Quaternion.identity, root.transform);

        // 5. Khu 2: Open Office (Trung tâm)
        // Bàn Junior, Senior, Dev, Tester, PM
        Vector3[] deskPositions = { new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0), new Vector3(0, 0, -2) };
        foreach (Vector3 pos in deskPositions)
        {
            SpawnItem(officeDeskPrefab, pos, Quaternion.identity, root.transform);
            SpawnItem(chairPrefab, pos + new Vector3(0, 0, -0.8f), Quaternion.identity, root.transform);
        }

        // 6. Khu 3: Meeting Room (Góc dưới bên trái + Vách kính)
        SpawnItem(meetingTablePrefab, new Vector3(-5, 0, -6), Quaternion.identity, root.transform);
        SpawnItem(whiteboardPrefab, new Vector3(-8, 0, -6), Quaternion.Euler(0, 90, 0), root.transform);

        // Ghế phòng họp
        SpawnItem(chairPrefab, new Vector3(-5, 0, -5), Quaternion.Euler(0, 180, 0), root.transform);
        SpawnItem(chairPrefab, new Vector3(-5, 0, -7), Quaternion.identity, root.transform);

        // Vách kính ngăn phòng họp
        for (float z = -4; z >= -8; z -= 2)
        {
            SpawnItem(glassWallPrefab, new Vector3(-1, 0, z), Quaternion.Euler(0, 90, 0), root.transform);
        }

        Debug.Log("Đã tạo thành công không gian văn phòng hoàn chỉnh!");
    }

    private void SpawnItem(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (prefab == null) return;
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.SetParent(parent);
    }
}