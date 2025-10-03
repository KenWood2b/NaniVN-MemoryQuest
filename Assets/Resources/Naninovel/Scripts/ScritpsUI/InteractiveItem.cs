using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveItem : MonoBehaviour, IPointerClickHandler
{
    public string Id = "amulet";
    public static event Action<string> Clicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(Id);
        gameObject.SetActive(false);
    }
}