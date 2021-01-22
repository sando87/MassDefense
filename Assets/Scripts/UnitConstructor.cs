using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitConstructor : MonoBehaviour
{
    static private UnitConstructor mInstacne = null;
    static public UnitConstructor Inst
    {
        get
        {
            if (mInstacne == null)
                mInstacne = GameObject.Find("UnitConstructor").GetComponent<UnitConstructor>();
            return mInstacne;
        }
    }

    public UnitSlot UnitSlotA;
    public UnitSlot UnitSlotB;
    public UnitSlot UnitSlotC;

    private void Start()
    {
        mInstacne = GameObject.Find("UnitConstructor").GetComponent<UnitConstructor>();
        Hide();
    }

    private void Update()
    {
        UpdateUnitIconColor();
    }

    private void UpdateUnitIconColor()
    {
        if (SystemInGame.Inst.IsUnitFull)
        {
            UnitSlotA.gameObject.SetActive(false);
            UnitSlotB.gameObject.SetActive(false);
            UnitSlotC.gameObject.SetActive(false);
        }
        else
        {
            int curMin = SystemInGame.Inst.CurrrentMineral;
            UnitSlotA.gameObject.SetActive(UnitSlotA.MineralCost <= curMin);
            UnitSlotB.gameObject.SetActive(UnitSlotB.MineralCost <= curMin);
            UnitSlotC.gameObject.SetActive(UnitSlotC.MineralCost <= curMin);
        }
    }


    public void Show(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
