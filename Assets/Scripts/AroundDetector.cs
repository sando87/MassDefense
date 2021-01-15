﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AroundDetector : MonoBehaviour
{
    [Serializable]
    public class UnityEventDetect : UnityEvent<Collider2D[]> { }
    public UnityEventDetect EventDetect = null;

    public float DetectRange { get; set; } = 0;

    private Stats mStats = null;

    private void Start()
    {
        mStats = GetComponent<Stats>();
        DetectRange = mStats.AttackRange;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, DetectRange);
        if(hitColliders.Length > 1)
        {
            List<Collider2D> list = new List<Collider2D>();
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != gameObject)
                    list.Add(hitCollider);
            }
            Collider2D[] rets = list.ToArray();
            EventDetect?.Invoke(rets);
        }
    }
}
