using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] bool _isBlockedByUI;
    [SerializeField] Tooltip _tooltip;
    [SerializeField] float _delayBeforeShown;

    private IEnumerator _currentCoroutine;

    public void Show(string header, string body)
    {
        _tooltip.SetText(header, body);
        _currentCoroutine = DelayShowCoroutine();
        StartCoroutine(_currentCoroutine);
    }

    public void Hide()
    {
        _tooltip.gameObject.SetActive(false);

        if(_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }

    IEnumerator DelayShowCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeShown);
        
        if(!(_isBlockedByUI && EventSystem.current.IsPointerOverGameObject()))
        {
            _tooltip.gameObject.SetActive(true);
            _tooltip.Update();
        }
    }
}
