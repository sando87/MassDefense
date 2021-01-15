using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectProjectile : MonoBehaviour
{
    public float MoveSpeed = 1;
    public GameObject ExplosionEffect;

    public Skill SkillInfo { get; private set; }
    

    private void Start()
    {
        SkillInfo = GetComponent<Skill>();
        transform.position = SkillInfo.StartPos;

        StartCoroutine(ChaseTarget(SkillInfo.Target));
    }

    IEnumerator ChaseTarget(GameObject target)
    {
        Vector3 dest = Vector3.zero;
        Vector3 dir = Vector3.zero;
        Vector3 dirAfter = Vector3.zero;
        while (true)
        {
            if (target != null)
                dest = target.transform.position;

            dir = dest - transform.position;
            dir.z = 0;
            dir.Normalize();

            Quaternion qua = Quaternion.FromToRotation(Vector3.right, dir);
            transform.localRotation = qua;
            transform.position += (dir * MoveSpeed * Time.deltaTime);

            dirAfter = dest - transform.position;
            dirAfter.z = 0;
            if (dirAfter.magnitude < 0.1 || Vector3.Dot(dir, dirAfter) <= 0) //충분히 가깝거나 이미 지나쳤을 경우
                break;

            yield return null;
        }


        if (target != null)
        {
            Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
            HealthBar hp = target.GetComponent<HealthBar>();
            if (hp != null)
                hp.Reduce(SkillInfo.Damage);
        }

        Destroy(gameObject);
    }

}
