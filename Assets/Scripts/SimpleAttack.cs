using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttack : MonoBehaviour
{
    private void Start()
    {
        Skill skill = GetComponent<Skill>();
        transform.position = skill.EndPos;

        if (skill.Target != null)
        {
            HealthBar hp = skill.Target.GetComponent<HealthBar>();
            if (hp != null)
                hp.Reduce(skill.Damage);
        }

        Destroy(gameObject, 1);
    }
}
