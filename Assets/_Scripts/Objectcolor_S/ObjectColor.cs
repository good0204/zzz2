using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColor : MonoBehaviour
{
    [SerializeField] Material[] skyboxs;
    [SerializeField] Material ObjectMaterial;
    [SerializeField] Material bottomMaterial;
    [SerializeField] Material CharacterMaterial;
    [SerializeField] Color[] objectColors;
    [SerializeField] Color[] bottomColors;

    public void ChangeObjectColor(int StageNum)
    {
        RenderSettings.skybox = skyboxs[StageNum];
        ObjectMaterial.color = objectColors[StageNum];
        bottomMaterial.color = bottomColors[StageNum];
        CharacterMaterial.color = objectColors[StageNum];
    }
}
