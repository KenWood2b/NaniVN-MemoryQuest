using System.Threading;
using System.Threading.Tasks;
using Naninovel;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : CustomUI
{
    [Header("Buttons")]
    [SerializeField] private Button loc1Btn, loc2Btn, loc3Btn;

    [Header("Thumbnails (always visible)")]
    [SerializeField] private Image thumb1, thumb2, thumb3;
    [SerializeField] private Sprite loc1Sprite, loc2Sprite, loc3Sprite;

    [Header("Sorting")]
    [SerializeField] private int topSortOrder = 1000;

    private ICustomVariableManager vars;
    private Canvas canvas;
    private CanvasGroup cg;
    private string picked;
    private TaskCompletionSource<string> tcs;
    private CancellationTokenRegistration ctr;

    protected override void Awake()
    {
        base.Awake();
        vars = Engine.GetService<ICustomVariableManager>();
        canvas = GetComponent<Canvas>();
        cg = GetComponent<CanvasGroup>();

        if (canvas) { canvas.overrideSorting = true; canvas.sortingOrder = topSortOrder; }

        if (thumb1) { thumb1.sprite = loc1Sprite; thumb1.preserveAspect = true; thumb1.raycastTarget = false; }
        if (thumb2) { thumb2.sprite = loc2Sprite; thumb2.preserveAspect = true; thumb2.raycastTarget = false; }
        if (thumb3) { thumb3.sprite = loc3Sprite; thumb3.preserveAspect = true; thumb3.raycastTarget = false; }

        if (loc1Btn) loc1Btn.onClick.AddListener(() => TryPick("Loc1"));
        if (loc2Btn) loc2Btn.onClick.AddListener(() => TryPick("Loc2"));
        if (loc3Btn) loc3Btn.onClick.AddListener(() => TryPick("Loc3"));

        OnVisibilityChanged += visible =>
        {
            if (visible)
            {
                transform.SetAsLastSibling();
                if (canvas) { canvas.overrideSorting = true; canvas.sortingOrder = topSortOrder; }
                if (cg) { cg.alpha = 1f; cg.interactable = true; cg.blocksRaycasts = true; }

                picked = null;
                ApplyLocksFromVars();

                if (tcs == null || tcs.Task.IsCompleted)
                    tcs = new TaskCompletionSource<string>();
            }
            else
            {
                if (cg) { cg.interactable = false; cg.blocksRaycasts = false; }
            }
        };
    }

    public void ApplyLocksFromVars()
    {
        vars ??= Engine.GetService<ICustomVariableManager>();
        bool l1 = AsBool(vars.GetVariableValue("loc1Locked"));
        bool l2 = AsBool(vars.GetVariableValue("loc2Locked"));
        bool l3 = AsBool(vars.GetVariableValue("loc3Locked"));
        if (loc1Btn) loc1Btn.interactable = !l1;
        if (loc2Btn) loc2Btn.interactable = !l2;
        if (loc3Btn) loc3Btn.interactable = !l3;
    }

    static bool AsBool(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        s = s.Trim().Trim('"');
        return s.Equals("true", System.StringComparison.OrdinalIgnoreCase);
    }

    private void TryPick(string id)
    {
        if (id == "Loc1" && loc1Btn && !loc1Btn.interactable) return;
        if (id == "Loc2" && loc2Btn && !loc2Btn.interactable) return;
        if (id == "Loc3" && loc3Btn && !loc3Btn.interactable) return;

        picked = id;
        tcs?.TrySetResult(picked);
    }

    public Task<string> WaitForChoiceAsync(CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(picked))
            return Task.FromResult(picked);

        if (tcs == null || tcs.Task.IsCompleted)
            tcs = new TaskCompletionSource<string>();

        if (ct.CanBeCanceled)
        {
            ctr.Dispose();
            ctr = ct.Register(() => tcs.TrySetCanceled());
        }

        return tcs.Task;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ctr.Dispose();
        tcs?.TrySetCanceled();
        if (cg) { cg.interactable = false; cg.blocksRaycasts = false; }
    }
}
