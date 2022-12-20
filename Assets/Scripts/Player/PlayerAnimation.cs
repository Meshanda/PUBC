using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Vector3 _lastPosition;

    private void Start()
    {
        _lastPosition = transform.position;
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }
    void Update()
    {
        IsMoving();
        _lastPosition = transform.position;
    }

    private void IsMoving()
    {
        _animator.SetBool("IsRunning", transform.position != _lastPosition);
    }
}
