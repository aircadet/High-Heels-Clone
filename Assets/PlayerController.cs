using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   [Header("--MOVEMENT--")]
   [SerializeField] private VariableJoystick joystick;
   [SerializeField] private float forwardSpeed;
   [SerializeField] private float sideSpeed;
   [SerializeField] private float turnAngle;
   [SerializeField] private float minX;
   [SerializeField] private float maxX;
   [HideInInspector] public bool playerCanMove = false;

   [Header("--COLLECTING--")]
   [SerializeField] private int openTo;
   [SerializeField] private Transform rightHeelHolder;
   [SerializeField] private Transform leftHeelHolder;

   [Header("--RELEASING--")] [SerializeField]
   private GameObject releasedHeelPrefab;


   [HideInInspector] public Animator animator;
   public static PlayerController instance;

   #region Unity

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }

      animator = GetComponent<Animator>();
   }

   private void Start()
   {
   }

   private void Update()
   {
      Move();
   }

   #endregion

   #region Movement

   void Move()
   {
      if (playerCanMove)
      {
         
         // Go forward allways
         transform.position += (Time.deltaTime * forwardSpeed) * transform.forward;
         
         // side movement

         if (joystick.Horizontal > 0)
         {
            // right
            transform.DORotate(new Vector3(0, turnAngle, 0), .5f);
            transform.position += (Time.deltaTime * sideSpeed) * Vector3.right;
         }
         else if (joystick.Horizontal < 0)
         {
            // left
            
            transform.DORotate(new Vector3(0, -turnAngle, 0), .5f);
            transform.position -= (Time.deltaTime * sideSpeed) * Vector3.right;
         }
         else
         {
            transform.DORotate(Vector3.zero, .5f);
         }

         HolPlayerInMap();
      }
   }

   void HolPlayerInMap()
   {
      Vector3 pos = transform.position;
      pos.x = Mathf.Clamp(pos.x, minX, maxX);

      transform.position = pos;
   }

   #endregion

   #region Heel Collecting

   public void CollectHeel()
   {
      RaisePlayer();
      OpenHeel(rightHeelHolder,openTo);
      OpenHeel(leftHeelHolder,openTo);
      openTo++;

   }

   void OpenHeel(Transform parent, int num)
   {
      GameObject heel = parent.GetChild(num).gameObject;
      
      heel.SetActive(true);
      heel.transform.DOScaleY(.9025f, .25f);
   }

   void RaisePlayer()
   {
      transform.DOMoveY(transform.position.y + 1, .25f);
   }
   

   #endregion

   #region Heel Releasing
   public void ReleaseHeel(float y)
   {
      openTo--;

      if (openTo <0)
      {
         // GAME OVER
         playerCanMove = false;
         GetComponent<Rigidbody>().isKinematic = true;
         transform.DOMoveZ(transform.position.z - 1, .2f);
         animator.SetTrigger("Death");
         StartCoroutine(InGameUI.instance.levelFail());
      }
      else
      {
         Vector3 pos = transform.position;
         pos.y = y;
         Instantiate(releasedHeelPrefab, pos, Quaternion.identity);
         GameObject rightHeel = rightHeelHolder.GetChild(openTo).gameObject;
         rightHeel.transform.DOScaleY(0, 0);
         rightHeel.SetActive(false);

         GameObject leftHeel = leftHeelHolder.GetChild(openTo).gameObject;
         leftHeel.transform.DOScaleY(0, 0);
         leftHeel.SetActive(false);
      }
      
      
   }

   #endregion

}
