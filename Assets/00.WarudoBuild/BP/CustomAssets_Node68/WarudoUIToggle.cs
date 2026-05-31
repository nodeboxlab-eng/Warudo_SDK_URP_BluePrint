using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 클릭 시 on/off 상태를 유지하는 커스텀 토글입니다.
/// 리모컨 버튼의 <see cref="UIElementData.ToggleStateOnClick"/> 과 별개로 두고 사용합니다.
/// 같은 오브젝트에 <see cref="Button"/> 을 붙이지 마세요(중복 반응 방지).
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
[DisallowMultipleComponent]
public class WarudoUIToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    bool _isOn;

    [SerializeField]
    BoolUnityEvent _onValueChanged;

    bool _pointerInputEnabled = true;
    Action<bool> _interactionCommitted;

    /// <summary>현재 토글 상태입니다.</summary>
    public bool IsOn => _isOn;

    /// <summary>편집 모드 등에서 클릭 반응을 끕니다.</summary>
    public void SetPointerInputEnabled(bool enabled)
    {
        _pointerInputEnabled = enabled;
    }

    /// <summary>사용자 클릭으로 값이 확정될 때 호출됩니다 (동기화용 SetIsOnWithoutNotify 는 호출하지 않음).</summary>
    public void SetInteractionCommittedCallback(Action<bool> callback)
    {
        _interactionCommitted = callback;
    }

    [Serializable]
    public class BoolUnityEvent : UnityEvent<bool> { }

#if UNITY_EDITOR
    void OnValidate()
    {
        EnsureGraphicRaycasts();
    }
#endif

    void Awake()
    {
        EnsureGraphicRaycasts();
    }

    void EnsureGraphicRaycasts()
    {
        var img = GetComponent<Image>();
        if (img != null)
            img.raycastTarget = true;
    }

    /// <summary>클릭과 동일하게 상태를 뒤집습니다.</summary>
    public void Toggle()
    {
        SetIsOn(!_isOn);
    }

    /// <summary>상태를 설정하고, 필요 시 콜백을 호출합니다.</summary>
    public void SetIsOn(bool value, bool sendCallback = true)
    {
        if (_isOn == value)
            return;
        _isOn = value;
        if (sendCallback)
        {
            _onValueChanged?.Invoke(_isOn);
            _interactionCommitted?.Invoke(_isOn);
        }
        OnIsOnChanged(_isOn);
    }

    /// <summary>프로그램에서만 값을 맞출 때 사용합니다(이벤트 없음).</summary>
    public void SetIsOnWithoutNotify(bool value)
    {
        if (_isOn == value)
            return;
        _isOn = value;
        OnIsOnChanged(_isOn);
    }

    protected virtual void OnIsOnChanged(bool on) { }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData == null || !isActiveAndEnabled)
            return;
        if (!_pointerInputEnabled)
            return;
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        Toggle();
    }
}
