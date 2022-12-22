using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSoundScript : MonoBehaviour
{
    private static KeepSoundScript instance = null;
    public static KeepSoundScript Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

}
