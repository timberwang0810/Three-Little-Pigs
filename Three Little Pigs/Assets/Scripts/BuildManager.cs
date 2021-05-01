using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildManager : MonoBehaviour
{
    public static BuildManager S;

    public int gridWidth;
    public int gridHeight;

    private float worldHeight;
    private float worldWidth;

    public GameObject strawTurretPrefab;
    public GameObject woodTurretPrefab;
    public GameObject brickTurretPrefab;

    private GameObject building = null;
    private GameObject currentTurret;

    private void Awake()
    {
        // Singleton Definition
        if (BuildManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        worldHeight = Camera.main.orthographicSize * 2.0f;
        worldWidth = worldHeight * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (building != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (currentTurret.GetComponent<Turret>().material == Material.STRAW)
            {
                int x = (int) Mathf.Floor(pos.x);
                int y = (int) Mathf.Ceil(pos.y);
                building.transform.position = new Vector3(x + worldWidth / (gridWidth * 2), y - worldHeight / (gridHeight * 2), 0);
            } else if (currentTurret.GetComponent<Turret>().material == Material.WOOD)
            {
                int x = (int)Mathf.Floor(pos.x);
                int y = (int)Mathf.Floor(pos.y);
                building.transform.position = new Vector3(x, y, 0);
            } else if (currentTurret.GetComponent<Turret>().material == Material.BRICK)
            {
                int x = (int)Mathf.Floor(pos.x);
                int y = (int)Mathf.Ceil(pos.y);
                building.transform.position = new Vector3(x, y - worldHeight / (gridHeight * 2), 0);
            }
            

            if (Input.GetMouseButtonDown(0))
            {
                if (building.GetComponent<Turret>().BuildTurret())
                {
                    GameManager.S.SubtractMoney(currentTurret.GetComponent<Turret>().cost);
                    building = null;
                }
            }

            if (Input.GetKey(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                Destroy(building);
                building = null;
            }
        }
    }

    public void btn_BuildTurret(String material)
    {
        GameObject turretType = MatchTurret((Material)Enum.Parse(typeof(Material), material.ToUpper()));
        if (GameManager.S.money >= turretType.GetComponent<Turret>().cost)
        {
            currentTurret = turretType;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            building = Instantiate(turretType, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        }
    }

    private GameObject MatchTurret(Material mat)
    {
        switch (mat)
        {
            case Material.STRAW:
                return strawTurretPrefab;
            case Material.WOOD:
                return woodTurretPrefab;
            case Material.BRICK:
                return brickTurretPrefab;
            default:
                return null;
        }
    }
}
