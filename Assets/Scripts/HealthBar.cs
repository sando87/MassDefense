using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    public enum HealthBarSize { Small, Medium, Big }

    public GameObject HealthBarPrefab;
    public HealthBarSize BarSize;

    private SpriteRenderer UnitSpriteRenderer;
    private SpriteRenderer BarSpriteRenderer;
    private GameObject HealthBarBG;
    private GameObject HealthBarGreen;
    private GameObject HealthBarRed;
    private Vector3 localScale = new Vector3(1, 1, 1);
    private Vector3 offset = Vector3.zero;

    public Stats Stats { get; private set; }


    void Start()
    {
        Stats = GetComponent<Stats>();
        HealthBarBG = Instantiate(HealthBarPrefab, transform);
        HealthBarGreen = HealthBarBG.transform.Find("Green").gameObject;
        HealthBarRed = HealthBarBG.transform.Find("Red").gameObject;

        UnitSpriteRenderer = GetComponent<SpriteRenderer>();
        BarSpriteRenderer = HealthBarBG.GetComponent<SpriteRenderer>();

        HealthBarGreen.SetActive(Stats.PlayerType == PlayerType.Human);
        HealthBarRed.SetActive(Stats.PlayerType == PlayerType.Computer);
        float size = BarSize == HealthBarSize.Small ? 0.2f : (BarSize == HealthBarSize.Medium ? 0.35f : 0.7f);
        HealthBarBG.transform.localScale = new Vector3(size, size, 1);
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        offset.y = (UnitSpriteRenderer.size.y + BarSpriteRenderer.size.y * HealthBarBG.transform.localScale.y) * 0.5f;
        localScale.x = Stats.HPRate;

        HealthBarBG.transform.localPosition = offset;
        HealthBarGreen.transform.localScale = localScale;
        HealthBarRed.transform.localScale = localScale;
    }

}
