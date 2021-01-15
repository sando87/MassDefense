using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Vector3 StartPos { get; set; }
    public Vector3 EndPos { get; set; }
    public GameObject Target { get; set; }
    public GameObject Owner { get; set; }
    public float Damage { get; set; }
}
