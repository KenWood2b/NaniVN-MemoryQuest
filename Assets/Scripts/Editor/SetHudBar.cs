#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class SetHudBar
{
    [MenuItem("Tools/Pairs/Set HUD Top Bar (80px)")]
    static void Apply()
    {
        var rt = Selection.activeTransform as RectTransform;
        if (!rt) { Debug.LogWarning("������ RectTransform (HUD)."); return; }

        // Stretch �� X, ������ � �����, ������ 80
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);

        // ������� ������� �����/������/������, ������ 80 �� ���� Bottom = -80
        rt.offsetMin = new Vector2(0f, -80f); // Left=0, Bottom=-80
        rt.offsetMax = new Vector2(0f, 0f);   // Right=0, Top=0

        Debug.Log("HUD ��������: ������� ������ 80 px (Stretch X, Top).");
    }
}
#endif
