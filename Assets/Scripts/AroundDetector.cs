using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AroundDetector : MonoBehaviour
{
    [Serializable]
    public class UnityEventDetect : UnityEvent<Collider2D[]> { }
    public UnityEventDetect EventDetect = null;

    private Property mProperty = null;

    private void Start()
    {
        mProperty = GetComponent<Property>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, mProperty.AttackRange);
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
