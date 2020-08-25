using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRight : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 120f);
    }
}
