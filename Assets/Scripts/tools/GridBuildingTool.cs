using UnityEngine;
using UnityEditor;

public class GridBuildingTool : EditorWindow
{
    private GameObject prefabToPlace;
    private float gridSize = 1.0f;
    private float currentRotation = 0.0f;
    private bool isBuildingMode = false;

    [MenuItem("Tools/Grid Building Tool")]
    public static void ShowWindow()
    {
        GetWindow<GridBuildingTool>("Grid Builder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Cấu Hình Công Cụ Xây Lưới 3D", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        prefabToPlace = (GameObject)EditorGUILayout.ObjectField("Prefab Cần Đặt:", prefabToPlace, typeof(GameObject), false);
        gridSize = EditorGUILayout.FloatField("Kích Thước Lưới (Grid):", gridSize);

        EditorGUILayout.Space(10);
        isBuildingMode = GUILayout.Toggle(isBuildingMode, "BẬT / TẮT CHẾ ĐỘ XÂY", "Button", GUILayout.Height(35));

        if (isBuildingMode)
        {
            EditorGUILayout.HelpBox("Thao tác trong Scene View:\n• Click chuột trái: Đặt khối\n• Phím R: Xoay 90 độ", MessageType.Info);
        }
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!isBuildingMode || prefabToPlace == null) return;

        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(controlID);

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 snapPos = hit.point;
            snapPos.x = Mathf.Round(snapPos.x / gridSize) * gridSize;
            snapPos.y = Mathf.Round(snapPos.y / gridSize) * gridSize;
            snapPos.z = Mathf.Round(snapPos.z / gridSize) * gridSize;

            Quaternion rotation = Quaternion.Euler(0, currentRotation, 0);

            // Vẽ khung màu xanh xem trước vị trí snap
            Handles.color = Color.green;
            Handles.DrawWireCube(snapPos + Vector3.up * (gridSize * 0.5f), new Vector3(gridSize, gridSize, gridSize));

            // Nhấn phím R để xoay 90 độ
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
            {
                currentRotation = (currentRotation + 90f) % 360f;
                e.Use();
            }

            // Click chuột trái để đặt Prefab vào Scene
            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                GameObject spawnedObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabToPlace);
                spawnedObj.transform.position = snapPos;
                spawnedObj.transform.rotation = rotation;

                Undo.RegisterCreatedObjectUndo(spawnedObj, "Grid Place Object");
                e.Use();
            }
        }

        sceneView.Repaint();
    }
}