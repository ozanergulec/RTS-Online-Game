using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }
    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();
    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask attackable;
    public GameObject groundMarker;
    private Camera cam;
    public bool attackCursorVisible;
    PhotonView pw;

    private void Start()
    {
        cam = Camera.main;
        pw = GetComponent<PhotonView>();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                //if we are hitting clickable object
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
                {
                    //if we hold left shift it adds multiple units to the selected list
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MultiSelect(hit.collider.gameObject);
                    }
                    else
                    {
                        SelectByClicking(hit.collider.gameObject);
                    }
                }
                else  //if we are not hitting 
                {
                    //if we stop pressing shift
                    if (Input.GetKey(KeyCode.LeftShift) == false)
                    {
                        DeselectAll();

                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    Vector3 markerPosition = hit.point;
                    markerPosition.y = 0.06f;
                    groundMarker.transform.position = markerPosition;
                    groundMarker.gameObject.SetActive(false);
                    groundMarker.SetActive(true);
                }
            }

            //Attack target
            if (unitsSelected.Count > 0 && AtLeastOneOffensiveUnit(unitsSelected))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                //if we are hitting a clickable object 
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
                {
                    Debug.Log("Enemy hovered with the mouse");

                    attackCursorVisible = true;
                    if (Input.GetMouseButtonDown(1))
                    {
                        Transform target = hit.transform;
                        foreach (GameObject unit in unitsSelected)
                        {
                            if (unit.GetComponent<AttackController>())
                            {
                                unit.GetComponent<AttackController>().targetToAttack = target;
                            }
                        }
                    }
                }
                else
                {
                    attackCursorVisible = false;
                }

            }
            CursorSelector();
        
        
    }
    
    private void CursorSelector()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, Mathf.Infinity, clickable))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Selectable);
        }
        else if(Physics.Raycast(ray,out hit,Mathf.Infinity, attackable)&& unitsSelected.Count>0 && AtLeastOneOffensiveUnit(unitsSelected))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Attackable);
        }
        else if(Physics.Raycast(ray, out hit,Mathf.Infinity,ground) && unitsSelected.Count>0)
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Walkable);
        }
        else
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.None);
        }
    }
    

    private bool AtLeastOneOffensiveUnit(List<GameObject> unitsSelected)
    {
        foreach (GameObject unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }
        }
        return false;
    }

    private void MultiSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit))
        {
            selectUnit(unit,true);
            unitsSelected.Remove(unit);
        }
        else
        {
            selectUnit(unit,true);
            unitsSelected.Add(unit);
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            unit.GetComponent<Movement>().isCommandedToMove = false;
            selectUnit(unit,false);
        }

        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        unitsSelected.Add(unit);
        selectUnit(unit,true);
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<Movement>().enabled = shouldMove;
    }
    
    private void triggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.Find("Indicator").gameObject.SetActive(isVisible);
    }

    internal void DragSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            selectUnit(unit, true);
        }
    }
    private void selectUnit(GameObject unit,bool isSelected)
    {
        triggerSelectionIndicator(unit,isSelected);
        EnableUnitMovement(unit,isSelected);
    }
}
