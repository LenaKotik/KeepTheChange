using UnityEngine;

#nullable enable

public sealed class PlayerController: MonoBehaviour
{
    public float sensitivity = 100f;
    public float maxInteractDistance = 10f;
    // ---
    float rY = 0f;
    Interactable? I = null;
    void LookAround()
    {
        float rX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rY = Mathf.Clamp(rY, -90f, 90);

        transform.localEulerAngles = new Vector3(-rY, rX, 0);
    }
    void Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycast = new RaycastHit();
        if (!Physics.Raycast(ray, out raycast, maxInteractDistance)) 
        {
            I = null;
            return; 
        }
        I = raycast.collider.GetComponent<Interactable>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        LookAround();
        Raycast();
        if (Input.GetMouseButtonDown(0))
            I?.Interact();
    }
}