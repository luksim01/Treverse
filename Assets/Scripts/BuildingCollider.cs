using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollider : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [ContextMenu("Add MeshColliders")]
    private void AddMeshColliders()
    {
        if (_target == null) _target = transform;
        AddRecursively(_target);
    }

    private void AddRecursively(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.GetComponent<MeshFilter>() != null) child.gameObject.AddComponent<MeshCollider>();

            AddRecursively(child);
        }
    }
}
