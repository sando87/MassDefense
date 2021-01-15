using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public UnityEvent EventHeathZero = null;

    public enum HealthBarSize { Small, Medium, Big }

    public GameObject HealthBarPrefab;
    public HealthBarSize BarSize;

    private SpriteRenderer BarSpriteRenderer;
    private GameObject HealthBarBG;
    private GameObject HealthBarGreen;
    private GameObject HealthBarRed;
    private Vector3 localScale = new Vector3(1, 1, 1);
    private Vector3 offset = Vector3.zero;

    public Stats Stats { get; private set; }
    public float CurrentHP { get; private set; }


    void Start()
    {
        Stats = GetComponent<Stats>();
        CurrentHP = Stats.TotalHP;
        HealthBarBG = Instantiate(HealthBarPrefab, transform);
        HealthBarGreen = HealthBarBG.transform.Find("Green").gameObject;
        HealthBarRed = HealthBarBG.transform.Find("Red").gameObject;

        BarSpriteRenderer = HealthBarBG.GetComponent<SpriteRenderer>();

        HealthBarGreen.SetActive(Stats.PlayerType == PlayerType.Human);
        HealthBarRed.SetActive(Stats.PlayerType == PlayerType.Computer);
        float size = BarSize == HealthBarSize.Small ? 0.2f : (BarSize == HealthBarSize.Medium ? 0.35f : 0.7f);
        HealthBarBG.transform.localScale = new Vector3(size, size, 1);

        ShowHealthBar(false);
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        offset.y = Stats.CharacterHeight + BarSpriteRenderer.size.y * HealthBarBG.transform.localScale.y * 0.5f;
        localScale.x = CurrentHP / Stats.TotalHP;

        HealthBarBG.transform.localPosition = offset;
        HealthBarGreen.transform.localScale = localScale;
        HealthBarRed.transform.localScale = localScale;
    }

    void ShowHealthBar(bool show)
    {
        HealthBarBG.SetActive(show);
        enabled = show;
    }

    public void Reduce(float health)
    {
        if (CurrentHP <= 0)
            return;

        CurrentHP -= health;
        ShowHealthBar(true);
        StartCoroutine("HideBar");
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            EventHeathZero?.Invoke();
        }
    }

    public void Increase(float health)
    {
        CurrentHP += health;
        ShowHealthBar(true);
        StartCoroutine("HideBar");
        if (CurrentHP >= Stats.TotalHP)
            CurrentHP = Stats.TotalHP;
    }

    IEnumerator HideBar()
    {
        yield return new WaitForSeconds(SystemConfig.HealthBarShowTimeSec);
        ShowHealthBar(false);
    }

}
