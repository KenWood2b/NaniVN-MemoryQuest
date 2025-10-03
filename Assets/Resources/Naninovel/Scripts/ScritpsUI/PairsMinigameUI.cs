using System.Threading.Tasks;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public sealed class PairsMinigameUI : CustomUI
{
    [Header("Refs")]
    [SerializeField] private PairsGameController controller;

    [Header("HUD")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button cancelButton;

    private TaskCompletionSource<string> tcs;
    private CanvasGroup cg;

    protected override void Awake()
    {
        base.Awake();
        cg = GetComponent<CanvasGroup>();
        if (cg) { cg.blocksRaycasts = false; cg.interactable = false; }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (controller)
        {
            controller.OnWin += HandleWin;
            controller.OnLose += HandleLose;
            controller.OnTimeChanged += HandleTimeChanged;
        }

        if (cancelButton)
        {
            cancelButton.onClick.RemoveListener(OnCancelClicked);
            cancelButton.onClick.AddListener(OnCancelClicked);
            cancelButton.interactable = true;
        }
    }

    protected override void OnDisable()
    {
        if (controller)
        {
            controller.OnWin -= HandleWin;
            controller.OnLose -= HandleLose;
            controller.OnTimeChanged -= HandleTimeChanged;
        }

        if (cancelButton)
            cancelButton.onClick.RemoveListener(OnCancelClicked);

        base.OnDisable();
    }

    public void Begin()
    {
        tcs = new TaskCompletionSource<string>();

        if (cg) { cg.blocksRaycasts = true; cg.interactable = true; }

        if (timerText) timerText.text = FormatTime(controller ? controller.TimeLimitSeconds : 0);

        if (!controller || !controller.BuildBoard())
            Complete("lose");
    }

    public Task<string> WaitForResultAsync() => tcs?.Task ?? Task.FromResult("lose");

    private void HandleWin() => Complete("win");
    private void HandleLose() => Complete("lose");

    private void OnCancelClicked()
    {
        if (controller) controller.ForceCancel();
        Complete("cancel");                       
    }

    private void HandleTimeChanged(float seconds)
    {
        if (timerText) timerText.text = FormatTime(Mathf.CeilToInt(seconds));
    }

    private static string FormatTime(int totalSeconds)
    {
        int m = Mathf.Max(0, totalSeconds) / 60;
        int s = Mathf.Max(0, totalSeconds) % 60;
        return $"{m:00}:{s:00}";
    }

    private void Complete(string result)
    {
        if (controller)
        {
            controller.OnWin -= HandleWin;
            controller.OnLose -= HandleLose;
            controller.OnTimeChanged -= HandleTimeChanged;
        }

        var v = string.IsNullOrEmpty(result) ? "lose" : result;
        tcs?.TrySetResult(v);

        if (cg) { cg.blocksRaycasts = false; cg.interactable = false; }
    }
}
