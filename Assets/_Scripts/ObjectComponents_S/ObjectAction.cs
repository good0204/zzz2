using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAction : MonoBehaviour
{

    [SerializeField] Transform parent;
    [SerializeField] List<GameObject> games = new List<GameObject>();
    void Start()
    {
        // games = new List<GameObject>(GameObject.FindGameObjectsWithTag("Objects"));

        // for (int i = 0; i < games.Count; i++)
        // {
        //     // games[i].GetComponent<IComponent>().Action();
        // }
        // // foreach (Transform Child in parent)
        // // {
        // //     if (Child.tag == "Objects")
        // //     {
        // //         for (int i = 0; i < Child.GetComponents<IComponent>().Length; i++)
        // //             Child.GetComponents<IComponent>()[i].Action();
        // //     }
        // //     // if (Child.GetComponents<IComponent>() != null)
        // //     //     for (int i = 0; i < Child.GetComponents<IComponent>().Length; i++)
        // //     //         Child.GetComponents<IComponent>()[i].Action();
        // // }
    }
}
