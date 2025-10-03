#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class NaniFindUnquotedComparisons
{
    [MenuItem("Tools/Naninovel/Find unquoted comparisons")]
    public static void Find()
    {
        var root = Application.dataPath;
        var files = Directory.GetFiles(root, "*.nani", SearchOption.AllDirectories);

        var rxAbortBare = new Regex(@"\{\s*abort\s*\}", RegexOptions.IgnoreCase);
        var rxEqAbort = new Regex(@"==\s*abort\b", RegexOptions.IgnoreCase);
        var rxEqCancel = new Regex(@"==\s*cancel\b", RegexOptions.IgnoreCase);
        var rxEqWin = new Regex(@"==\s*win\b", RegexOptions.IgnoreCase);
        var rxEqLose = new Regex(@"==\s*lose\b", RegexOptions.IgnoreCase);

        int hits = 0;
        foreach (var path in files)
        {
            var lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (rxAbortBare.IsMatch(line) || rxEqAbort.IsMatch(line) ||
                    rxEqCancel.IsMatch(line) || rxEqWin.IsMatch(line) || rxEqLose.IsMatch(line))
                {
                    hits++;
                    Debug.Log($"Unquoted at {path}:{i + 1}  -->  {line}");
                }
            }
        }

        Debug.Log($"Search done. Found {hits} issues.");
    }
}
#endif
