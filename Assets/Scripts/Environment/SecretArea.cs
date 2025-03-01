using System;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    [SerializeField] private Color gizmoColor;
    [SerializeField] private GameObject area;
    [SerializeField] private bool isEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnter)
        {
            area.SetActive(false);
        }
        else
        {
            area.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.color = gizmoColor;
            
            Vector3 size = new Vector3(boxCollider.size.x, boxCollider.size.y, 0);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.offset, size);
        }
    }
}
