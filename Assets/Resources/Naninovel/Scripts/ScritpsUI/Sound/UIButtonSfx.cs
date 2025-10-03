using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public sealed class UIButtonSfx : MonoBehaviour
{
    [SerializeField] private string clipId = "ui-click";
    [Range(0, 1)] public float volume = 1f;

    void Awake()
    {
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(() => AudioHub.I?.PlayUi(clipId, volume));
    }
}
