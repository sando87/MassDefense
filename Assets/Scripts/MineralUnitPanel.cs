using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineralUnitPanel : MonoBehaviour
{
    static private MineralUnitPanel mInstacne = null;
    static public MineralUnitPanel Inst
    {
        get
        {
            if (mInstacne == null)
                mInstacne = GameObject.Find("MineralUnitPanel").GetComponent<MineralUnitPanel>();
            return mInstacne;
        }
    }

    public Image MineralBar;
    public Text MineralText;
    public Text UnitText;

    private void Start()
    {
        MineralBar.fillAmount = 0;
        MineralText.text = "0/" + SystemConfig.MaxMineral.ToString();
        UnitText.text = "0/" + SystemConfig.MaxUnitCount.ToString();

        StartCoroutine(UpdatePanelEverySec());
    }

    public void UpdatePanel()
    {
        SystemInGame sysInst = SystemInGame.Inst;
        float rate = sysInst.CurrrentMineral / (float)SystemConfig.MaxMineral;
        MineralBar.fillAmount = rate;
        MineralText.text = sysInst.CurrrentMineral.ToString() + "/" + SystemConfig.MaxMineral.ToString();
        string unitText = sysInst.CurrrentUnitCount.ToString() + "/" + SystemConfig.MaxUnitCount.ToString();
        UnitText.text = unitText;
    }

    IEnumerator UpdatePanelEverySec()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            UpdatePanel();
        }
    }
}
