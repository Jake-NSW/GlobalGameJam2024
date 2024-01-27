using UnityEditor;
using UnityEngine;

public class RenameAssetsWindow : EditorWindow
{
    private string _searchText = "";
    private string _replaceText = "";

    [MenuItem("Window/Rename Assets")]
    public static void ShowWindow()
    {
        var window = GetWindow<RenameAssetsWindow>("Rename Assets");
        if (Selection.assetGUIDs.Length > 0)
        {
            var firstSelectedAssetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            var firstSelectedAssetName = System.IO.Path.GetFileNameWithoutExtension(firstSelectedAssetPath);
            window._searchText = firstSelectedAssetName;
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Replace asset names", EditorStyles.boldLabel);
        _searchText = EditorGUILayout.TextField("Search Text", _searchText);
        _replaceText = EditorGUILayout.TextField("Replace Text", _replaceText);

        if (GUILayout.Button("Replace"))
        {
            RenameAssets();
        }
    }

    void RenameAssets()
    {
        foreach (var guid in Selection.assetGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var assetName = System.IO.Path.GetFileNameWithoutExtension(path);
            var newAssetName = assetName.Replace(_searchText, _replaceText);
            if (newAssetName != assetName)
            {
                AssetDatabase.RenameAsset(path, newAssetName);
            }
        }
    }
}

public class RenameAssetsContextMenu
{
    [MenuItem("Assets/Rename Assets")]
    private static void RenameAssets()
    {
        RenameAssetsWindow.ShowWindow();
    }
}