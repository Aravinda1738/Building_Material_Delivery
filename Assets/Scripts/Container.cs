using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    private SO_Container containerData;
    [SerializeField]
    private SO_DebugMode DebugMode;
    private GameObject sp;

    private float moveSpeed = 0.005f;

    private Vector3 moveToPos;

    public bool isMovingOut = false;


    

    [Tooltip("you can add only 4 elements")]
    public DeleveryItem[] items;



    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void Start()
    {
        moveToPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 500);
        
    }

    private void FixedUpdate()
    {
        //on game win
        if (isMovingOut)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, moveToPos.z, moveSpeed * Time.deltaTime));
        }

    }


    public void InitialLoad(List<int> ids)
    {
        
    }
    public void Load()
    {

    }

    public void Unload()
    {

    }

    public void MoveOut()
    {
       isMovingOut=true;


       
       
    }


    private void OnValidate()
    {

        if (items == null || items.Length != containerData.totalItemsCanHold)
        {

            Array.Resize(ref items, containerData.totalItemsCanHold);
        }
    }

   

    }
