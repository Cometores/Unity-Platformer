using System;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;
    public int ChosenSkinId { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
