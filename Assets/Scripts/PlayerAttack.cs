using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   [Header("Components")] 
   [SerializeField] PlayerMovement playerMovement;
   [SerializeField] Animator animator;
   
   [SerializeField] SwordObject swordObject;
   
   int attackState = 1;
   float tempAttackWaitTime = 3f;
   bool _canAttack = true;
   bool _canBlock = true;

   void Start()
   {
      swordObject.OnHitEnemy += OnHitEnemy;
   }

   void Update()
   {
      AttackAndBlock();
      
      ResetAttackAnimation();
   }

   #region Attack & Block

   void AttackAndBlock()
   {
      if (Input.GetMouseButtonDown(0) && GetCanAttack())
      {
         AttackAnimation();
      }

      if (Input.GetMouseButtonDown(1) && GetCanBlock())
      {
         BlockAnimation(true);
         SetCanAttack(false);
      }else if (Input.GetMouseButtonUp(1) && GetCanBlock())
      {
         BlockAnimation(false);
         SetCanAttack(true);
      }
   }

   #endregion

   #region Actions

   public void OnAttackStart()
   {
      playerMovement.SetCanMove(false);
      SetCanAttack(false);
      SetCanBlock(false);
   }

   public void OnAttack()
   {
      swordObject.check = true;
   }
   
   public void OnAttackEnd()
   {
      playerMovement.SetCanMove(true);
      SetCanAttack(true);
      SetCanBlock(true);
      swordObject.check = false;
   }
   
   public void OnHitEnemy(Enemy enemy, Vector3 hitPoint, Vector3 hitDirection)
   {
      Debug.Log("Hit Enemy");
      enemy.Damage(attackState, hitPoint, hitDirection);
   }

   #endregion
   

   #region Set & Get Attack & Block

   void SetCanAttack(bool value)
   {
      _canAttack = value;
   }
   
   bool GetCanAttack()
   {
      return _canAttack;
   }
   
   void SetCanBlock(bool value)
   {
      _canBlock = value;
   }
   
   bool GetCanBlock()
   {
      return _canBlock;
   }

   #endregion

   #region Animations

   void ResetAttackAnimation()
   {
      if(tempAttackWaitTime > 0)
      {
         tempAttackWaitTime -= Time.deltaTime;
      }else
      {
         attackState = 1;
      }
   }

   void AttackAnimation()
   {
      animator.SetTrigger("Attack");
      animator.SetInteger("AttackState", attackState);
        
      attackState = attackState == 4 ? 1 : attackState + 1;
      tempAttackWaitTime = 3f;
   }
   
   void BlockAnimation(bool status)
   {
      animator.SetBool("Block",status);
   }

   #endregion
  
}
