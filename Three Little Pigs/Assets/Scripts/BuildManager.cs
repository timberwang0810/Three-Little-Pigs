using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class BuildManager : MonoBehaviour
{
    public static BuildManager S;

    public int gridWidth;
    public int gridHeight;

    private float worldHeight;
    private float worldWidth;

    [Serializable]
    public struct TurretInfo
    {
        public Material material;
        public int cost;
        public GameObject turretObject;
    }
    public TurretInfo[] turretArray;
    public Dictionary<Material, TurretInfo> turrets;

    private GameObject building = null;
    private TurretInfo currentTurret;

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
        turrets = new Dictionary<Material, TurretInfo>();
        foreach (TurretInfo t in turretArray)
        {
            turrets.Add(t.material, t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (building != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (currentTurret.material == Material.STRAW)
            {
                int x = (int) Mathf.Floor(pos.x);
                int y = (int) Mathf.Ceil(pos.y);
                if (SceneManager.GetActiveScene().name == "Level1")
                {
                    building.transform.position = new Vector3(x + worldWidth / (gridWidth * 2), y - worldHeight / (gridHeight * 4), 0);
                } else
                {
                    building.transform.position = new Vector3(x + worldWidth / (gridWidth * 2), y, 0);
                }
                
            } else if (currentTurret.material == Material.WOOD)
            {
                int x = (int)Mathf.Floor(pos.x);
                int y = (int)Mathf.Floor(pos.y);
                building.transform.position = new Vector3(x, y - worldHeight / (gridHeight * 4), 0);
            } else if (currentTurret.material == Material.BRICK)
            {
                int x = (int)Mathf.Floor(pos.x);
                int y = (int)Mathf.Floor(pos.y);
                building.transform.position = new Vector3(x, y, 0);
            }
            

            if (Input.GetMouseButtonDown(0))
            {
                if (building.GetComponent<Turret>().BuildTurret())
                {
                    GameManager.S.SubtractMoney(currentTurret.cost);
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
        TurretInfo turretType = turrets[(Material)Enum.Parse(typeof(Material), material.ToUpper())];
        if (GameManager.S.money >= turretType.cost)
        {
            currentTurret = turretType;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            building = Instantiate(turretType.turretObject, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        }
    }
}
