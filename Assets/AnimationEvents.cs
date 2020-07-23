using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent OnStart;
    public UnityEvent OnKick;
    public UnityEvent OnAttackStart;
    public UnityEvent OnAttackEnd;

    

    public GameObject weaponTrail;

    private void Start() 
    {
        //OnStart.Invoke();
        Invoke("Deactivate", 0.5f);
    }

   public void Kick()
    {
        OnKick.Invoke();
    }

    public void AttackStart()
    {
        //weaponTrail.SetActive(true);
        OnAttackStart.Invoke();
    }

    public void Deactivate()
    {
        print("Deactivate");
        //weaponTrail.SetActive(false);
        OnAttackEnd.Invoke();
    }
    public void AttackEnd()
    {
        print("Deactivate");
        //weaponTrail.SetActive(false);
        OnAttackEnd.Invoke();
    }
}
