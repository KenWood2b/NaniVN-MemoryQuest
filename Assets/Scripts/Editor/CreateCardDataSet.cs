#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class CreateCardDataSet
{
    [MenuItem("Tools/Pairs/Create CardDataSet (empty)")]
    static void Create()
    {
        var asset = ScriptableObject.CreateInstance<CardDataSet>();
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/CardDataSet.asset");
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        Selection.activeObject = asset;
        Debug.Log("CardDataSet создан: " + path + ". Заполни Faces иконками.");
    }
}
#endif
