using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class NaniExprScanner : MonoBehaviour
{
    static readonly Regex RxCurly = new Regex(@"\{\s*(lose|win|cancel|abort)\s*\}", RegexOptions.IgnoreCase);
    static readonly Regex RxEqWord = new Regex(@"==\s*(lose|win|cancel|abort)\b(?!\s*[""'])", RegexOptions.IgnoreCase);

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.logMessageReceived += OnLog;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= OnLog;
    }

    void Start()
    {
        StartCoroutine(ScanAfterOneFrame());
    }

    IEnumerator ScanAfterOneFrame()
    {
        yield return null;
        ScanAll();
    }

    void OnLog(string condition, string stackTrace, LogType type)
    {
        if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert && type != LogType.Warning)
            return;

        if (condition.IndexOf("Custom variable 'lose' doesn't exist", StringComparison.OrdinalIgnoreCase) >= 0 ||
            condition.IndexOf("Custom variable 'win' doesn't exist", StringComparison.OrdinalIgnoreCase) >= 0 ||
            condition.IndexOf("Custom variable 'cancel' doesn't exist", StringComparison.OrdinalIgnoreCase) >= 0 ||
            condition.IndexOf("Custom variable 'abort' doesn't exist", StringComparison.OrdinalIgnoreCase) >= 0 ||
            condition.IndexOf("String was not recognized as a valid Boolean", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            Debug.Log("[NaniExprScanner] Got runtime error; rescanning objects to find offenders…");
            ScanAll();
        }
    }

    void ScanAll()
    {
        var allBehaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        int hits = 0;

        foreach (var mb in allBehaviours)
        {
            if (!mb) continue;

         
#if UNITY_EDITOR
            if (!UnityEditor.EditorUtility.IsPersistent(mb) && mb.hideFlags != HideFlags.None);
#endif
            var t = mb.GetType();
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo[] fields;

            try { fields = t.GetFields(flags); }
            catch { continue; }

            foreach (var f in fields)
            {
                if (f.FieldType == typeof(string))
                {
                    var s = f.GetValue(mb) as string;
                    if (string.IsNullOrEmpty(s)) continue;
                    if (RxCurly.IsMatch(s) || RxEqWord.IsMatch(s))
                    {
                        hits++;
                        Debug.LogWarning(FormatHit("STRING", mb, f.Name, s));
                    }
                }

                else if (f.FieldType == typeof(string[]))
                {
                    var arr = f.GetValue(mb) as string[];
                    if (arr == null) continue;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var s = arr[i];
                        if (string.IsNullOrEmpty(s)) continue;
                        if (RxCurly.IsMatch(s) || RxEqWord.IsMatch(s))
                        {
                            hits++;
                            Debug.LogWarning(FormatHit($"STRING[{i}]", mb, f.Name, s));
                        }
                    }
                }

                else if (f.FieldType.IsGenericType &&
                         f.FieldType.GetGenericTypeDefinition() == typeof(List<>) &&
                         f.FieldType.GetGenericArguments()[0] == typeof(string))
                {
                    var list = f.GetValue(mb) as IList;
                    if (list == null) continue;
                    for (int i = 0; i < list.Count; i++)
                    {
                        var s = list[i] as string;
                        if (string.IsNullOrEmpty(s)) continue;
                        if (RxCurly.IsMatch(s) || RxEqWord.IsMatch(s))
                        {
                            hits++;
                            Debug.LogWarning(FormatHit($"LIST[{i}]", mb, f.Name, s));
                        }
                    }
                }
            }
        }

        Debug.Log($"[NaniExprScanner] Scan complete. Hits: {hits}.");
    }

    string FormatHit(string kind, MonoBehaviour mb, string fieldName, string value)
    {

        string path = "(no scene object)";
        try
        {
            if (mb && mb.gameObject)
                path = GetPath(mb.gameObject.transform);
        }
        catch { }

        return $"[NaniExprScanner] Found suspicious {kind} on " +
               $"Object='{path}', Component='{mb.GetType().Name}', Field='{fieldName}', " +
               $"Value='{value}'";
    }

    string GetPath(Transform t)
    {
        var stack = new Stack<string>();
        while (t != null)
        {
            stack.Push(t.name);
            t = t.parent;
        }
        return string.Join("/", stack.ToArray());
    }
}
