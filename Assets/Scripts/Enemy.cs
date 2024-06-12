using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] BFX_BloodSettings blood;
    public void Damage(int damageState, Vector3 hitPoint, Vector3 hitDirection)
    {
        animator.SetTrigger("Damage");
        animator.SetInteger("DamageState", damageState);
        
        GameObject bloodObject = Instantiate(blood.gameObject, hitPoint, Quaternion.LookRotation(hitDirection));
        Destroy(bloodObject,20f);
    }
}
