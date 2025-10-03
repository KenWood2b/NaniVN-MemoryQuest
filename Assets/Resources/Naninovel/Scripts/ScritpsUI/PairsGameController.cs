using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PairsGameController : MonoBehaviour
{
    [Header("Board")]
    [SerializeField] private RectTransform boardRoot;
    [SerializeField] private PairsCard cardPrefab;
    [SerializeField] private int pairs = 6;

    [Header("Visuals")]
    [SerializeField] private CardDataSet cardData;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private float mismatchDelay = 1.2f;

    [Header("Timer")]
    [SerializeField] private int timeLimitSeconds = 60;
    public int TimeLimitSeconds => timeLimitSeconds;

    public System.Action OnWin;
    public System.Action OnLose;
    public System.Action<float> OnTimeChanged;

    private readonly List<PairsCard> cards = new();
    private PairsCard first, second;
    private int matched;
    private bool inputLocked;
    private Coroutine timerRoutine;

    public bool BuildBoard()
    {
        if (!boardRoot || !cardPrefab || !cardData || cardData.Faces == null || cardData.Faces.Count == 0)
        {
            Debug.LogError("[PairsGameController] Assign boardRoot/cardPrefab/cardData (Faces).");
            return false;
        }

        for (int i = boardRoot.childCount - 1; i >= 0; i--)
            Destroy(boardRoot.GetChild(i).gameObject);

        var take = Mathf.Min(pairs, cardData.Faces.Count);
        if (take <= 0) { Debug.LogError("[PairsGameController] pairs <= 0"); return false; }

        var faces = cardData.Faces.OrderBy(_ => Random.value).Take(take).ToList();
        var deck = faces.Concat(faces).OrderBy(_ => Random.value).ToList();

        cards.Clear();
        matched = 0;
        first = second = null;
        inputLocked = false;

        foreach (var face in deck)
        {
            var card = Instantiate(cardPrefab, boardRoot);
            card.Init(face, backSprite);
            card.Clicked += OnCardClicked;
            cards.Add(card);
        }

        if (timerRoutine != null) StopCoroutine(timerRoutine);
        timerRoutine = StartCoroutine(Timer());

        Debug.Log($"Pairs game started: {pairs} pairs, {timeLimitSeconds} seconds");
        return true;
    }

    public void ForceCancel()
    {
        StopTimer();
    }

    private IEnumerator Timer()
    {
        float t = timeLimitSeconds;
        OnTimeChanged?.Invoke(t);
        while (t > 0f && matched < pairs)
        {
            t -= Time.deltaTime;
            OnTimeChanged?.Invoke(Mathf.Max(0f, t));
            yield return null;
        }
        timerRoutine = null;

        if (matched < pairs)
            OnLose?.Invoke();
    }

    private void StopTimer()
    {
        if (timerRoutine != null) StopCoroutine(timerRoutine);
        timerRoutine = null;
    }

    private void OnDestroy()
    {
        foreach (var c in cards) if (c) c.Clicked -= OnCardClicked;
        StopTimer();
    }

    private void OnCardClicked(PairsCard card)
    {
        if (inputLocked || card.IsMatched || card.IsFaceUp) return;

        card.FlipUp();

        if (!first) { first = card; return; }

        second = card;
        inputLocked = true;

        if (first.FaceSprite == second.FaceSprite)
        {
            first.LockOpen();
            second.LockOpen();
            matched++;
            ResetTurn();

            if (matched >= pairs)
            {
                StopTimer();
                OnWin?.Invoke();
            }
        }
        else
            StartCoroutine(MismatchThenFlipDown());
    }

    private IEnumerator MismatchThenFlipDown()
    {
        yield return new WaitForSeconds(mismatchDelay);
        first.FlipDown();
        second.FlipDown();
        ResetTurn();
    }

    private void ResetTurn()
    {
        first = null;
        second = null;
        inputLocked = false;
    }
}
