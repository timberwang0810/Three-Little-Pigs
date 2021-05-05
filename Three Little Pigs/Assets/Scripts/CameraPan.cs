using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    private Vector3 panDestination;
    private float targetOrthoSize;
    private bool isPanning = false;
    // Start is called before the first frame update
    void Start()
    {
        panDestination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPanning)
        {
            transform.position = Vector3.Lerp(transform.position, panDestination, Time.deltaTime * 3);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, targetOrthoSize, Time.deltaTime * 3);
        }
    }

    public void PanTo(Vector3 location, float targetOrthoSize)
    {
        panDestination = new Vector3(location.x, location.y, transform.position.z);
        this.targetOrthoSize = targetOrthoSize;
        isPanning = true;
    }
}
