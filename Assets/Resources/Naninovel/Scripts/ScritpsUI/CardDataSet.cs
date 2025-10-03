using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSet", menuName = "Pairs/Card Data Set")]
public class CardDataSet : ScriptableObject
{
    [Tooltip("Нужно минимум 6 для сетки 3×4.")]
    public List<Sprite> Faces = new List<Sprite>();

    public List<Sprite> GetRandomFaces(int count)
    {
        var result = new List<Sprite>(count);

        if (Faces == null || Faces.Count == 0)
        {
            Debug.LogError("[CardDataSet] Faces пуст.");
            return result;
        }

        var idx = new List<int>(Faces.Count);
        for (int i = 0; i < Faces.Count; i++) idx.Add(i);
        for (int i = idx.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (idx[i], idx[j]) = (idx[j], idx[i]);
        }

        int take = Mathf.Min(count, idx.Count);
        for (int k = 0; k < take; k++)
            result.Add(Faces[idx[k]]);

        return result;
    }
}