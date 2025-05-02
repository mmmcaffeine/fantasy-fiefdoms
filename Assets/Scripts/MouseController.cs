using System;
using UnityEngine;

// TODO Remove duplication, repetition, and magic numbers by introducing an enumeration for the mouse buttons and
//      a private array for the Action<RaycastHit> that we can index based on the mouse button
public class MouseController : Singleton<MouseController>
{
    public Action<RaycastHit> OnLeftMouseClick;
    public Action<RaycastHit> OnRightMouseClick;
    public Action<RaycastHit> OnMiddleMouseClick;

    void Update()
    {
        if (Input.GetMouseButton(0)) CheckMouseClick(0);
        if (Input.GetMouseButton(1)) CheckMouseClick(1);
        if (Input.GetMouseButton(2)) CheckMouseClick(2);
    }

    private void CheckMouseClick(int mouseButton)
    {
        var ray = Camera.main?.ScreenPointToRay(Input.mousePosition);

        if (ray is null)
        {
            return;
        }

        if (Physics.Raycast(ray.Value, out var hit, Mathf.Infinity))
        {
            switch (mouseButton)
            {
                case 0:
                    OnLeftMouseClick?.Invoke(hit);
                    break;
                case 1:
                    OnRightMouseClick?.Invoke(hit);
                    break;
                case 2:
                    OnMiddleMouseClick?.Invoke(hit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mouseButton), mouseButton,
                        "Expected a mouse button of 0 (left), 1 (right), or 2 (middle).");
            }
        }
    }
}
