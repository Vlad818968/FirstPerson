using UnityEngine;
using UnityEngine.EventSystems;

public static class PointerEventDataExtension
{
    private static float _lastClickTime;
    private static GameObject _lastSelectedObject;

    private const float _doubleClickCoolDown = 0.3f;

    public static bool IsDoubleClick(this PointerEventData eventData)
    {
        if (_lastSelectedObject != eventData.pointerClick.gameObject)
        {
            _lastClickTime = eventData.clickTime;
            _lastSelectedObject = eventData.pointerClick.gameObject;
            return false;
        }

        var isDoubleClick = eventData.clickTime - _lastClickTime <= _doubleClickCoolDown;
        _lastClickTime = eventData.clickTime;
        return isDoubleClick;
    }
}