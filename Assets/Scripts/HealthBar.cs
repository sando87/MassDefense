using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    private Vector3 localScale = new Vector3(1, 1, 1);

    public GameObject HealthBarObject;

    public Property Property { get; private set; }
    private float mCurrentHP = 0;

    [SerializeField]
    public UnityEvent EventDeath = null;

    void Start()
    {
        Property = GetComponent<Property>();
        mCurrentHP = Property.TotalHP;
        HealthBarObject = Instantiate(HealthBarObject, transform);
    }

    void Update()
    {
        if (mCurrentHP <= 0)
            EventDeath?.Invoke();

        float rate = mCurrentHP / Property.TotalHP;
        UpdateHealthBar(rate);
    }

    void UpdateHealthBar(float rate)
    {
        localScale.x = rate;
        HealthBarObject.transform.localPosition = offset;
        HealthBarObject.transform.GetChild(0).localScale = localScale;
    }

    public void GetDamaged(float attack)
    {
        mCurrentHP -= attack;
        if (mCurrentHP < 0)
            mCurrentHP = 0;
    }

}
