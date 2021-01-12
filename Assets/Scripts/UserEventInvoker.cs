using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserEventInvoker : MonoBehaviour
{
    public const float DragDetectCoefficient = 0.1f;

    private GameObject mDownObject = null;
    private Vector3 mDownPosition = Vector3.zero;
    private bool mIsDragged = false;

    void Update()
    {
        //여기서 각 객체들의 UserEvent를 발생시킨다
        InvokeUserInput();
    }

    void InvokeUserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPt);
            if (hit != null)
            {
                mDownObject = hit.gameObject;
                mDownPosition = worldPt;
                mIsDragged = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 worldPt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mIsDragged)
            {
                // Drag & Drop Event 발생
                if(mDownObject != null && mDownObject.activeSelf)
                {
                    UserEvent handler = mDownObject.GetComponent<UserEvent>();
                    if (handler != null)
                    {
                        handler.EventDragDrop?.Invoke(worldPt);
                    }
                }
            }
            else
            {
                // Click Event 발생
                Collider2D hit = Physics2D.OverlapPoint(worldPt);
                if (hit != null && hit.gameObject == mDownObject && hit.gameObject.activeSelf)
                {
                    UserEvent handler = hit.gameObject.GetComponent<UserEvent>();
                    if (handler != null)
                    {
                        handler.EventClick?.Invoke();
                    }
                }
            }

            mDownObject = null;
            mDownPosition = Vector3.zero;
            mIsDragged = false;
        }
        else if (Input.GetMouseButton(0))
        {
            if (mDownObject != null)
            {
                //Drag 여부를 감지만 한다.
                Vector3 curWorldPt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if ((curWorldPt - mDownPosition).magnitude >= DragDetectCoefficient)
                {
                    mIsDragged = true;
                }
            }
        }
        
    }
}
