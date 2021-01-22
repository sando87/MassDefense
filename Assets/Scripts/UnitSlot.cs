using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour
{
    public UnitConstructor Constructor;
    public GameObject UnitPrefab;
    public int MineralCost;

    public void OnClick()
    {
        CreateUnit();
        MineralUnitPanel.Inst.UpdatePanel();
        Constructor.Hide();
    }

    private void CreateUnit()
    {
        GameObject unitObj = Instantiate(UnitPrefab, Constructor.transform.position, Quaternion.identity);
        SystemInGame.Inst.CurrrentMineral -= MineralCost;
        SystemInGame.Inst.CurrrentUnitCount += 1;
    }
}
