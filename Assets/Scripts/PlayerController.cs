using UnityEngine;

#nullable enable

public sealed class PlayerController: MonoBehaviour
{
    enum PlayerState
    {
        Interacting,
        Inspecting,
        Holding
    }
    public float sensitivity = 100f;
    public float maxInteractDistance = 10f;
    public Vector2 HoldDistance = new Vector2(0.5f, 0.25f);
    public float InspectDistance = 1f;
    public float ThrowForce = 10f;
    // ---
    float rY = 0f;
    Vector2 rIns = new Vector2(0f, 0f);

    Interactable? Intrct = null;
    Inspectable? Inspct = null;
    Pickable? Pcup = null;

    Pickable? Held = null;
    Transform? HeldParent = null;

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
            Intrct = raycast.collider.GetComponent<Interactable>();
            Inspct = raycast.collider.GetComponent<Inspectable>();
            Pcup = raycast.collider.GetComponent<Pickable>();
        }
        else
        {
            Intrct = null;
            Inspct = null;
            Pcup = null;
        }
    }
    void PickUp()
    {
        if (Pcup != null)
        {
            state = PlayerState.Holding;
            Held = Pcup;
            Held.IsHeld = true;
            Held.GetComponent<Rigidbody>().isKinematic = true;
            //Held.transform.localPosition = transform.forward * HoldDistance.x - transform.up * HoldDistance.y;
            HeldParent = Held.transform.parent;
            Held.transform.SetParent(transform, true);

            return;
        }
        throw new System.InvalidOperationException("Attempting to pick up null"); 

    }
    void Inspect()
    {
        if (Inspct != null)
        {
            Rigidbody? r = Inspct.GetComponent<Rigidbody>();
            if (r != null)
                r.MovePosition(transform.position + transform.forward * InspectDistance);

            rIns += new Vector2( Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime,
                                 Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime );

            Inspct.transform.localEulerAngles = new Vector3(rIns.x,rIns.y, 1);

            return;
        }
        throw new System.InvalidOperationException("Inspecting null");
    }
    void SetInspecting(bool value = true)
    {
        if (Inspct != null)
        {
            state = value ? PlayerState.Inspecting : PlayerState.Interacting;
            Inspct.SetUp(!value);

            return;
        }
        throw new System.InvalidOperationException("Attempting to inspect null");
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
                    if (Inspct != null)
                        SetInspecting();
                    else if (Pcup != null)
                        PickUp();
                    else
                        Intrct?.Interact();
                }
            break;
            case PlayerState.Holding:
#pragma warning disable CS8602
                LookAround();

                if (Input.GetMouseButtonDown(0)) // Left click
                {
                    Held.Use();
                }
                if (Input.GetMouseButtonDown(1)) // Right click
                {
                    bool placed = Held.Place();
                    Held.IsHeld = !placed;
                    if (placed)
                    {
                        Held =  null;
                        state = PlayerState.Interacting;
                    }
                }
                if (Input.GetMouseButtonDown(2)) // Middle 'click'
                {
                    Held.GetComponent<Rigidbody>().isKinematic = false;
                    // Throw the item forward, a little bit upward
                    Held.GetComponent<Rigidbody>().AddForceAtPosition((transform.forward + transform.up * 0.5f) * ThrowForce, Held.transform.position-transform.forward);
                    Held.transform.SetParent(HeldParent);
                    Held = null;
                    state = PlayerState.Interacting;
                }
#pragma warning restore CS8602
                break;
            case PlayerState.Inspecting:
                Inspect();
                if (Input.GetMouseButtonDown(1)) // Right click
                    SetInspecting(false);
                break;
        }
    }
}