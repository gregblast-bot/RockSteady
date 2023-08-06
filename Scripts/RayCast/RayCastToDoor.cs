using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastToDoor : MonoBehaviour
{
    [SerializeField]
    private int raycastLength = 100;

    [SerializeField]
    private LayerMask layerMaskInteract;

    [SerializeField]
    private string excludeLayerName = null;

    [SerializeField]
    private KeyCode openDoorKey = KeyCode.Mouse0;

    [SerializeField]
    private Image crossHair = null;

    private ControllerDoor raycastedObject;
    private bool isCrossHairActive;
    private bool doOnce;
    private const string interactableTag = "Door";
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // Hit logic
        RaycastHit hit;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Mouse and ray position setup
        mousePosition.x = 0f;
        mousePosition.y = 50f;
        mousePosition.z = 40f;
        mousePosition = cam.ScreenToWorldPoint(mousePosition);

        // Convert the mouse position to world coordinates
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Offset
        Vector3 offset = new Vector3(0, 0, 0);

        // Set the position of the image RectTransform to the calculated world position
        crossHair.transform.position = worldPosition + offset;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // Mask not used as of now
        //int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if (Physics.Raycast(ray, out hit, raycastLength))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                if (!doOnce)
                {
                    raycastedObject = hit.collider.gameObject.GetComponent<ControllerDoor>();
                    crossHairChange(true);
                }

                isCrossHairActive = true;
                doOnce = true;

                if (Input.GetKeyDown(openDoorKey))
                {
                    raycastedObject.PlayAnimation();
                }
            }
        }
        else
        {
            if (isCrossHairActive)
            {
                crossHairChange(false);
                doOnce = false;
            }
        }

    }

    private void crossHairChange(bool on)
    {
        if (on && !doOnce)
        {
            crossHair.color = Color.red;
        }
        else
        {
            crossHair.color = Color.white;
            isCrossHairActive = false;
        }
    }
}
