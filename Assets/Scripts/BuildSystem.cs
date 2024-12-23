using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public GameObject buildingPrefab; // Yerle�tirilecek bina prefab'�
    public LayerMask groundLayer; // Zemin i�in kullan�lan Layer
    public LayerMask buildingLayer; // Binalar i�in kullan�lan Layer
    private GameObject previewBuilding; // �nizleme i�in kullan�lan bina
    private bool isPlacingBuilding = false;
    public GameObject buildingUI;
    public GameObject militaryBase;
    public GameObject boostedMilitaryBase;
    public float checkRadius = 3f; // �arp��ma kontrol� i�in yar��ap
    public ArrayList buildingPositions = new ArrayList();
    public GameObject unitSelection;
    public GameObject unitSelectionBox;
    string building;
    public void onMilitaryBaseButton()
    {
        if (WoodRockController.rockCount >= 200 && WoodRockController.woodCount >= 100)
        {
            building = "MilitaryBase";
            buildingPrefab = militaryBase;
            buildingUI.SetActive(false);
            StartPlacingBuilding();
        }
    }

    public void onBoostedMilitaryBaseButton()
    {
        if (WoodRockController.rockCount >= 500 && WoodRockController.woodCount >= 300)
        {
            building = "BoostedMilitaryBase";
            buildingPrefab = boostedMilitaryBase;
            buildingUI.SetActive(false);
            StartPlacingBuilding();
            
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildingUI.SetActive(true);
            unitSelection.SetActive(false);
            unitSelectionBox.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buildingUI.SetActive(false);
            unitSelection.SetActive(true);
            unitSelectionBox.SetActive(true);
        }

        if (isPlacingBuilding)
        {
            HandleBuildingPlacement();
        }
    }

    void StartPlacingBuilding()
    {
        if (buildingPrefab == null) return;

        isPlacingBuilding = true;
        previewBuilding = Instantiate(buildingPrefab);
        SetPreviewMaterial(previewBuilding, true); // �effaf materyal
        
    }

    void HandleBuildingPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 placePosition = hit.point;
            previewBuilding.transform.position = new Vector3(
                Mathf.Round(placePosition.x),
                placePosition.y,
                Mathf.Round(placePosition.z)
            );

            // �arp��ma kontrol�
            bool canPlace = CanPlaceBuilding(previewBuilding.transform.position);

            // Binan�n rengini de�i�tir (yerle�tirilebilir / yerle�tirilemez)
            SetPreviewMaterial(previewBuilding, canPlace);

            // Sa� mouse tu�u ile bina yerle�tirme
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceBuilding();
                unitSelection.SetActive(true);
                unitSelectionBox.SetActive(true);
            }

            // ESC ile bina yerle�tirme iptali
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelPlacingBuilding();
                unitSelection.SetActive(true);
                unitSelectionBox.SetActive(true);
            }
        }
    }

    void PlaceBuilding()
    {
        if (previewBuilding == null) return;
        Quaternion rotation = Quaternion.identity;
        // Ger�ek bina yerle�tirme
        if (buildingPrefab==militaryBase)
        {
           rotation = Quaternion.Euler(0, -180, 0);
        }
        GameObject placedBuilding = PhotonNetwork.Instantiate(building, previewBuilding.transform.position,rotation);
        if (buildingPrefab == militaryBase)
        {
            WoodRockController.rockCount -= 200;
            WoodRockController.woodCount -= 100;
        }
        if(buildingPrefab == boostedMilitaryBase)
        {
            WoodRockController.rockCount -= 500;
            WoodRockController.woodCount -= 300;

        }
        buildingPositions.Add(placedBuilding.transform.position);
        Destroy(previewBuilding);
        isPlacingBuilding = false;
    }

    void CancelPlacingBuilding()
    {
        if (previewBuilding != null)
        {
            Destroy(previewBuilding);
        }
        isPlacingBuilding = false;
    }

    void SetPreviewMaterial(GameObject obj, bool canPlace)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            Color color = material.color;
            color = canPlace ? Color.green : Color.red; // Yerle�tirilebilir: Ye�il, Yerle�tirilemez: K�rm�z�
            color.a = 0.05f; // �effafl�k
            material.color = color;
            renderer.material = material;
        }
    }

    bool CanPlaceBuilding(Vector3 position)
    {
        bool canPlace = true;
        foreach (Vector3 vector3 in buildingPositions)
        {
            float distanceX = vector3.x - position.x;
            float distanceZ = vector3.z - position.z;

            if (Mathf.Abs(distanceX) > 2 && Mathf.Abs(distanceZ) > 2)
            {
                canPlace = true;
            }
            else
            {
                canPlace = false;
            }
        }
        return canPlace;
    }
}
