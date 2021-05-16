using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class Drag : MonoBehaviour
{
    bool dragging = false;
    Plane movePlane;


    private GameObject _drag;
    private Vector3 screenPosition;
    private Vector3 offset;

    private void Update()
    {
        if (_drag == null && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (gameObject == hit.transform.gameObject)
                {
                    _drag = hit.transform.gameObject;
                    screenPosition = Camera.main.WorldToScreenPoint(_drag.transform.position);
                    offset = _drag.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _drag = null;
        }

        if (_drag != null)
        {
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
            _drag.transform.position = new Vector3(currentPosition.x, _drag.transform.position.y, currentPosition.z);
        }
    }
}