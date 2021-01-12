using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Property Property { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Property = GetComponent<Property>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        // Open Functions..(On GUI)
        Debug.Log("OnClick");
    }
    public void OnDragDrop(Vector3 worldPos)
    {
        StopCoroutine("MoveTo");
        StartCoroutine("MoveTo", worldPos);
    }

    private IEnumerator MoveTo(Vector3 dest)
    {
        StopAllCoroutines();
        IsReadyToAttack = false;
        DetectEnable = false;

        float moveSpeed = Property.MoveSpeed;
        dest.z = dest.y * 0.1f; //y좌표가 높을수록 해당 객체는 뒤쪽에 그려져야 하므로...
        Vector3 dir = dest - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        float duration = distance / moveSpeed;
        float time = 0;
        while(time < duration)
        {
            transform.position += (dir * moveSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        IsReadyToAttack = true;
        DetectEnable = true;
    }


    public void OnDetectEvent(Collider2D[] colliders)
    {
        if(IsReadyToAttack)
        {
            foreach(Collider2D col in colliders)
            {
                Property ppt = col.GetComponent<Property>();
                if (ppt.IsEnemy(Property))
                {
                    HealthBar hp = colliders[0].gameObject.GetComponent<HealthBar>();
                    if (hp != null)
                    {
                        StartCoroutine(Attack(hp));
                        break;
                    }
                }
            }
        }
    }

    public bool DetectEnable
    {
        get { return GetComponent<AroundDetector>().enabled; }
        set { GetComponent<AroundDetector>().enabled = value; }
    }

    public bool IsReadyToAttack { get; private set; } = true;
    private IEnumerator Attack(HealthBar enemy)
    {
        IsReadyToAttack = false;
        DetectEnable = false;
        Unit enemyUnit = enemy.GetComponent<Unit>();

        while(!enemyUnit.IsDeath)
        {
            //Play Animation Attack
            //Play Attack Sound
            //Create Attack Particle
            enemy.GetDamaged(Property.AttackPoint);

            yield return new WaitForSeconds(1 / Property.AttackSpeed);

        }
        IsReadyToAttack = true;
        DetectEnable = true;
    }


    public bool IsDeath { get; private set; } = false;
    public void OnDeath()
    {
        if (IsDeath)
            return;

        IsDeath = true;
        StopAllCoroutines();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<UserEvent>().enabled = false;
        DetectEnable = false;
        //Play Animation Death
        //Play Death Sound
        Destroy(gameObject, 3.0f);
    }
}
