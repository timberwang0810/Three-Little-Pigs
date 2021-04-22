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

    [Serializable]
    public struct TurretInfo
    {
        public string name;
        public int cost;
        public GameObject turretObject;
    }
    public TurretInfo[] turretArray;
    public Dictionary<string, TurretInfo> turrets;

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
        turrets = new Dictionary<string, TurretInfo>();
        foreach (TurretInfo t in turretArray)
        {
            turrets.Add(t.name, t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (building != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (currentTurret.name == "straw")
            {
                int x = (int) Mathf.Floor(pos.x);
                int y = (int) Mathf.Ceil(pos.y);
                building.transform.position = new Vector3(x + worldWidth / (gridWidth * 2), y - worldHeight / (gridHeight * 2), 0);
            } else if (currentTurret.name == "wood")
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

            if (Input.GetKey(KeyCode.Escape))
            {
                Destroy(building);
                building = null;
            }
        }
    }

    public void btn_BuildTurret(string name)
    {
        TurretInfo turretType = turrets[name];
        if (GameManager.S.money >= turretType.cost)
        {
            currentTurret = turretType;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            building = Instantiate(turretType.turretObject, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        }
    }
}
