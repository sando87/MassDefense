using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthController : MonoBehaviour
{
    //현재 카메라 특성상 y값이 높을수록 카메라에서 멀어지고 뒤쪽에 그려져야 한다.
    void Update()
    {
        Vector3 pos = transform.position;
        pos.z = pos.y * 0.1f;
        transform.position = pos;
    }
}
