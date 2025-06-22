using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinneMesh : MonoBehaviour//人物材质变化
{
    [SerializeField] List<SkinnedMeshRenderer> meshesToHiglight;//所需要染色的部位
    [SerializeField] Material originalMaterial;//原部位颜色
    [SerializeField] Material highligtedMaterial;//被锁定后部位的颜色
    public void HighlightMesh(bool higlight)
    {
        foreach (var mesh in meshesToHiglight)
        {
            mesh.material = (higlight) ? highligtedMaterial : originalMaterial;

        }

    }
}
