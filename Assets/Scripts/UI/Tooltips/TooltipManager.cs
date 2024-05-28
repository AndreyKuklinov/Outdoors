using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Manager;

    [SerializeField] Tooltip _tooltip;
    [SerializeField] float _delayBeforeShown;

    private bool _isPointerOnTrigger;

    void Start()
    {
        Manager = this;
    }

    public void Show(string header, string body)
    {
        _isPointerOnTrigger = true;
        _tooltip.SetText(header, body);
        _tooltip.Update();
        StartCoroutine(ShowCoroutine());
    }

    public void Hide()
    {
        _isPointerOnTrigger = false;
        _tooltip.gameObject.SetActive(false);
    }

    IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeShown);
        if(_isPointerOnTrigger)
            _tooltip.gameObject.SetActive(true);
    }
}
