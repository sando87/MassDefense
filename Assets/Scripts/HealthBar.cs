using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    private Vector3 localScale = new Vector3(1, 1, 1);

    public float TotalHP { get; set; }
    public float CurrentHP { get; set; }
    public GameObject HealthBarObject;

    [SerializeField]
    public UnityEvent EventDeath = null;

    void Start()
    {
        HealthBarObject = Instantiate(HealthBarObject, transform);
    }

    void Update()
    {
        if (CurrentHP < 0)
        {
            CurrentHP = 0;
            EventDeath?.Invoke();
        }

        float rate = CurrentHP / TotalHP;
        UpdateHealthBar(rate);
    }

    void UpdateHealthBar(float rate)
    {
        localScale.x = rate;
        HealthBarObject.transform.localPosition = offset;
        HealthBarObject.transform.GetChild(0).localScale = localScale;
    }

}
