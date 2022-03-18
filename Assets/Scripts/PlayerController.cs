using UnityEngine;

#nullable enable

public class PlayerController: MonoBehaviour
{
    public float sensitivity = 100f;
    public float maxInteractDistance = 10f;
    public float highlightIntensity = 2f;
    // ---
    float rY = 0f;
    Interactable? I = null;
    Color Icolor;
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

        if (I == raycast.collider.GetComponent<Interactable>()) { Debug.Log(I); return; }

        Material? m = I?.GetComponent<Renderer>()?.material;
        if (m != null)
        {
            // m != null => I != null, no null reference
#pragma warning disable CS8602
            m.color = Icolor;
            I.GetComponent<Renderer>().material = m;
#pragma warning restore CS8602
        }

        I = raycast.collider.GetComponent<Interactable>();
        
        if (I == null) return;

        m = I.GetComponent<Renderer>()?.material;
        if (m == null) return;

        Icolor = m.color;
        m.color *= highlightIntensity;
        I.GetComponent<Renderer>().material = m;
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