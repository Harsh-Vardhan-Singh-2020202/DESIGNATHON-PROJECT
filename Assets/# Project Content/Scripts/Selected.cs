using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Selected : MonoBehaviour
{
    public Material mat1;
    public Material mat2;
    public BoneList Bone_List;

    [HideInInspector] public bool selected = false;

    private MeshRenderer meshRenderer;
    private GameObject[] Bones;

    void Start()
    {
        // Get the MeshRenderer component attached to this GameObject
        meshRenderer = GetComponent<MeshRenderer>();

        // Assign mat1 as the initial material
        meshRenderer.material = mat1;

        Bones = Bone_List.Bones;
    }

    private void Update()
    {
        if (!selected)
        {
            meshRenderer.material = mat1;
        }
        else
        {
            meshRenderer.material = mat2;
        }
    }

    public void SwitchMaterial()
    {
        if (selected)
        {
            selected = false;
            meshRenderer.material = mat1;
        }
        else
        {
            selected = true;
            meshRenderer.material = mat2;
        }

        foreach (var Bone in Bones)
        {
            if (Bone != gameObject && Bone.GetComponent<Selected>().selected)
            {
                Bone.GetComponent<Selected>().selected = false;
            }
        }
    }
}