using UnityEngine;

#nullable enable

public sealed class PlayerController: MonoBehaviour
{
    enum PlayerState
    {
        Interacting,
        Inspecting
    }
    public float sensitivity = 100f;
    public float maxInteractDistance = 10f;
    public float InspectDistance = 1f;
    // ---
    float rY = 0f;
    float rYIns = 0f;
    float rXIns = 0f;
    Interactable? I = null;
    Inspectable? Ins = null;
    PlayerState state = PlayerState.Interacting;

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
        if (Physics.Raycast(ray, out raycast, maxInteractDistance))
        {
            I = raycast.collider.GetComponent<Interactable>();
            Ins = raycast.collider.GetComponent<Inspectable>();
        }
        else
        {
            I = null;
            Ins = null;
        }
    }
    void Inspect()
    {
        if (Ins != null)
        {
            Rigidbody? r = Ins.GetComponent<Rigidbody>();
            if (r != null)
                r.MovePosition(transform.position + transform.forward * InspectDistance);

            rXIns += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            rYIns += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            Ins.transform.localEulerAngles = new Vector3(rYIns, -rXIns, 1);

            return;
        }
        throw new System.InvalidOperationException("Inspecting null");
    }
    void SetInspecting(bool value = true)
    {
        if (Ins != null)
        {
            state = value ? PlayerState.Inspecting : PlayerState.Interacting;
            Ins.SetUp(!value);

            return;
        }
        throw new System.InvalidOperationException("trying to inspect when Ins is null");
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        switch (state)
        {
            case PlayerState.Interacting:
                LookAround();
                Raycast();
                if (Input.GetMouseButtonDown(0)) // Left click
                {
                    if (Ins != null)
                        SetInspecting();
                    else
                        I?.Interact();
                }

            break;
            case PlayerState.Inspecting:
                Inspect();
                if (Input.GetMouseButtonDown(1)) // Right click
                    SetInspecting(false);
                break;
        }
    }
}