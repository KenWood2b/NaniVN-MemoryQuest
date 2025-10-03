using Naninovel;
using Naninovel.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class QuestUI : CustomUI
{
    [SerializeField] private RectTransform content;         
    [SerializeField] private TextMeshProUGUI linePrefab;  

    private readonly List<TextMeshProUGUI> lines = new();

    public void AddLine(string text)
    {
        var inst = Instantiate(linePrefab, content);
        inst.text = "• " + text;
        inst.gameObject.SetActive(true);
        lines.Add(inst);
    }

    public void ClearAll()
    {
        foreach (var l in lines) if (l) Destroy(l.gameObject);
        lines.Clear();
    }

    internal Task ShowAsync()
    {
        throw new NotImplementedException();
    }
}