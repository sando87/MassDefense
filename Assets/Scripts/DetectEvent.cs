using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectEvent : MonoBehaviour
{

    [Serializable]
    public class UnityEventDetect : UnityEvent<Collider[]> { }
    public UnityEventDetect EventDetect = null;

    public float DetectRadius = 1;

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, DetectRadius);
        if(hitColliders.Length > 1)
        {
            List<Collider> list = new List<Collider>();
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != gameObject)
                    list.Add(hitCollider);
            }
            Collider[] rets = list.ToArray();
            EventDetect?.Invoke(rets);
        }
    }
}
