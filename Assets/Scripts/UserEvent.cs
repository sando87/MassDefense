using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UserEvent : MonoBehaviour
{
    [SerializeField]
    public UnityEvent EventClick = null;

    [Serializable]
    public class UnityEventDragDrop : UnityEvent<Vector3> { }
    public UnityEventDragDrop EventDragDrop = null;
}
