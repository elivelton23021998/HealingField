using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
[ExecuteInEditMode]
public static class HierarchyIconEditor
{
    static HierarchyIconEditor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject != null)
        {
            var icon = EditorGUIUtility.ObjectContent(gameObject, typeof(GameObject)).image;
            var iconRect = new Rect(selectionRect.x, selectionRect.y, 16f, 16f);
            var backgroundColor = EditorGUIUtility.isProSkin ? new Color(0.22f, 0.22f, 0.22f) : new Color(0.76f, 0.76f, 0.76f);

            EditorGUI.DrawRect(iconRect, backgroundColor);
            GUI.DrawTexture(iconRect, icon);

            if (Selection.activeGameObject == gameObject)
            {
                EditorGUI.DrawRect(iconRect, new Color(0.24f, 0.48f, 0.90f, 0.5f));
            }
        }
    }

    private static void OnHierarchyChanged()
    {
        EditorApplication.RepaintHierarchyWindow();
    }

    //Asset developed by Noctys_Holding
}