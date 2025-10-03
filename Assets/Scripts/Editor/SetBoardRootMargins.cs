#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class SetBoardRootMargins
{
    [MenuItem("Tools/Pairs/Set BoardRoot Offsets 40/40/100/40")]
    static void Apply()
    {
        var rt = Selection.activeTransform as RectTransform;
        if (!rt) { Debug.LogWarning("Выдели RectTransform (BoardRoot)."); return; }
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = new Vector2(40f, 40f);   // Left, Bottom
        rt.offsetMax = new Vector2(-40f, -100f); // Right, Top (минус!)
        Debug.Log("Offsets применены: L40 R40 T100 B40");
    }
}
#endif
