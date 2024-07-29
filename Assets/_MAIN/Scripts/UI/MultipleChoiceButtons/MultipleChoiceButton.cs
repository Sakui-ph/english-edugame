using System;
using AUDIO_SYSTEM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MultipleChoiceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public MultipleChoiceGroup multipleChoiceGroup;
    public Image background;
    private bool isDisabled;
    public event Action OnClickAction;
    public AudioClip clickSound;

    void Start()
    {
        background = GetComponent<Image>();
        multipleChoiceGroup.Subscribe(this);
        if (GetComponentInChildren<LevelLocker>())
        {
            if(!GetComponentInChildren<LevelLocker>().unlocked)
            {
                DisableButton();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDisabled)
            multipleChoiceGroup.OnEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDisabled)
            multipleChoiceGroup.OnExit(this);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!isDisabled)
        {
            HandleClick();
            OnClickAction?.Invoke();
        }
    }

    public virtual void HandleClick()
    {
        multipleChoiceGroup.OnClick(this);

        if (clickSound != null)
            AudioManager.instance.PlaySoundEffect(clickSound);
        AudioManager.instance.PlaySoundEffect("Switch");
    }

    public void DisableButton() => isDisabled = true;
    public void EnableButton() => isDisabled = false;

    void OnDestroy()
    {
        multipleChoiceGroup.Unsubscribe(this);
    }
}
