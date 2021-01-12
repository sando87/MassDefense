using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        float moveSpeed = 1.0f;
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
    }


    public void OnDetectEvent(Collider[] colliders)
    {
        if(IsReadyToAttack)
        {
            HealthBar hp = colliders[0].gameObject.GetComponent<HealthBar>();
            if(hp != null)
                StartCoroutine(Attack(hp));
        }
    }

    public bool IsReadyToAttack { get; private set; } = true;
    private IEnumerator Attack(HealthBar enemy)
    {
        IsReadyToAttack = false;

        //Play Animation Attack
        //Play Attack Sound
        //Create Attack Particle
        enemy.CurrentHP -= 3.0f;

        yield return new WaitForSeconds(1.0f);
        IsReadyToAttack = true;
    }


    public bool IsDeath { get; private set; } = false;
    public void OnDeath()
    {
        if (IsDeath)
            return;

        IsDeath = true;
        StopAllCoroutines();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<DetectEvent>().enabled = false;
        GetComponent<UserEvent>().enabled = false;
        //Play Animation Death
        //Play Death Sound
        Destroy(gameObject, 3.0f);
    }
}
