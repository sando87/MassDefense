using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingMissile : MonoBehaviour
{
    public float MoveSpeed = 1;
    public float RotateSpeed = 0;
    public float RotateAccel = 1;
    public GameObject ExplosionEffect;

    public Skill SkillInfo { get; private set; }


    private void Start()
    {
        SkillInfo = GetComponent<Skill>();
        transform.position = SkillInfo.StartPos;

        Vector3 startDir = SkillInfo.EndPos.x > SkillInfo.StartPos.x ? new Vector3(1, 1, 0) : new Vector3(-1, 1, 0);
        startDir.Normalize();

        Quaternion qua = Quaternion.FromToRotation(Vector3.right, startDir);
        transform.localRotation = qua;

        StartCoroutine(SmoothRotateToTarget(SkillInfo.Target));
    }

    IEnumerator SmoothRotateToTarget(GameObject target)
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

            Quaternion targetQua = Quaternion.FromToRotation(Vector3.right, dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQua, RotateSpeed * Time.deltaTime);
            transform.position += (transform.right * MoveSpeed * Time.deltaTime);

            RotateSpeed += RotateAccel;

            dirAfter = dest - transform.position;
            dirAfter.z = 0;
            if (dirAfter.magnitude < 0.1) //충분히 가까울 경우
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
