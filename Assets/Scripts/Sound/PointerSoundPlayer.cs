using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerSoundPlayer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] AudioClip _pointerEnterClip;
    [SerializeField] AudioClip _pointerClickClip;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_pointerClickClip == null)
            return;

        SoundManager.Singleton.PlayClip(_pointerClickClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_pointerEnterClip == null)
            return;

        SoundManager.Singleton.PlayClip(_pointerEnterClip);
    }
}
