using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInGame : MonoBehaviour
{
    static private SystemInGame mInstacne = null;
    static public SystemInGame Inst
    {
        get
        {
            if (mInstacne == null)
                mInstacne = GameObject.Find("System/SystemInGame").GetComponent<SystemInGame>();
            return mInstacne;
        }
    }

    private void Start()
    {
        StartCoroutine(RaiseMineral());
    }

    private void Update()
    {
        
    }


    public int CurrrentMineral { get; set; }
    public int CurrrentUnitCount { get; set; }
    public bool IsUnitFull { get { return CurrrentUnitCount >= SystemConfig.MaxUnitCount; } }

    IEnumerator RaiseMineral()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            CurrrentMineral += SystemConfig.MineralPerSec;
            CurrrentMineral = Mathf.Clamp(CurrrentMineral, 0, SystemConfig.MaxMineral);
        }
    }
}
