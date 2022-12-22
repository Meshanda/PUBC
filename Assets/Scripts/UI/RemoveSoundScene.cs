using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSoundScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KeepSoundScript.Instance.gameObject.GetComponent<AudioSource>().Pause();
    }
}
