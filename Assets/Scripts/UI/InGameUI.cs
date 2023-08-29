using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : BaseUI

{
    public Transform followTarget;
    public Vector3 followOffset;

    protected virtual void LateUpdate()
    {
        if (followTarget != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(followTarget.position) + followOffset;
        }
    }

    public void SetTarget(Transform target)
    {
        followTarget = target;
        if (followTarget != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(followTarget.position) + followOffset;
        }
    }

    public void SetOffset(Vector3 offset)
    {
        followOffset = offset;
        if (followTarget != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(followTarget.position) + followOffset;
        }
    }

    public override void CloseUI()
    {
        base.CloseUI();

        GameManager.UI.CloseInGameUI(this);
    }
}
