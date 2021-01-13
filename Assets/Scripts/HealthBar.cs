using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    private Vector3 localScale = new Vector3(1, 1, 1);

    public GameObject HealthBarObject;

    public Stats Stats { get; private set; }


    void Start()
    {
        Stats = GetComponent<Stats>();
        HealthBarObject = Instantiate(HealthBarObject, transform);
    }

    void Update()
    {
        float rate = Stats.HPRate;
        UpdateHealthBar(rate);
    }

    void UpdateHealthBar(float rate)
    {
        localScale.x = rate;
        HealthBarObject.transform.localPosition = offset;
        HealthBarObject.transform.GetChild(0).localScale = localScale;
    }

}
