using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDeath
{
    private float m_Yaw;
    private float m_Pitch;

    public Transform m_PitchControllerTransform;
    public float m_YawRotationalSpeed = 360.0f;
    public float m_PitchRotationalSpeed = -180.0f;
    public float m_MinPitch = -80.0f;
    public float m_MaxPitch = 50.0f;
    public bool m_InvertedYaw = true;
    public bool m_InvertedPitch = true;
    public LayerMask m_ShootLayerMask;

    public Camera m_Camera;
    private CharacterController m_CharacterController;

    public float m_Speed = 10.0f;
    public float m_FastSpeedMultiplier = 3f;
    public float m_JumpSpeed = 10.0f;
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_RunKeyCode = KeyCode.J;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;

    public KeyCode m_Attach = KeyCode.E;


    private float touchingGroundValue = 0.5f;
    private float m_VerticalSpeed = 0.0f;
    private float touchingGround = 0.3f; //initial value
    private bool m_OnGround = false;
    public float m_DotToEnterPortal = 0.5f;

    private GameObject m_AttachObject = null;
    public bool m_ThrewCompanion = false; //variable para cuando soltamos el cubo (se veía afectado[el evento del botón] por el canattachdelay)
    public Transform m_AttachPosition;
    public float m_AttachObjectTime = 1f;
    private float m_CurrentAttachObjectTime = 0f;

    private float m_DetachForce = 550f;

    Vector3 m_Direction;

    public Portal BluePortal;
    public Portal OrangePortal;
    private Portal currentPortal;
    private Vector3 PortalPos;
    private Quaternion PortalRot;

    private Vector3 nextPos;
    private Vector3 nextRot;
    private bool Spawn=false;
    private float sizeScale;
    private float localScalePortals = 1;

    public AudioSource PortalShoot;

    void Awake()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_CharacterController = GetComponent<CharacterController>();
        m_Pitch = m_PitchControllerTransform.localRotation.eulerAngles.x;

        GameController.GetGameController().SetPlayer(this);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        PlayerCamera();
        PlayerMovement();
        //Attach
        if (CanAttach() && Input.GetKeyDown(m_Attach))
            Attach();
        if (m_AttachObject != null && Input.GetMouseButtonDown(1))
            Detach(0f);
        if (m_AttachObject != null && Input.GetMouseButtonDown(0))
            Detach(m_DetachForce);


        UpdateAttachPosition();

        //-------------------------------------------------------PORTALS------------------------------------------------------\\
        //Portals
        if (CanAttach() && Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            PortalPos = BluePortal.transform.position;
            PortalRot = BluePortal.transform.rotation;
        }
        else if (CanAttach() && Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0))
        {
            PortalPos = OrangePortal.transform.position;
            PortalRot = OrangePortal.transform.rotation;
        }

        //Active funct.
        if (CanAttach() && Input.GetMouseButton(0))
        {
            ShootPortal(BluePortal);
            ReSizing();

        }

        else if (CanAttach() && Input.GetMouseButton(1))
        {
            ShootPortal(OrangePortal);
            ReSizing();
        }
            

       

        //SpawnPortal

        if (currentPortal != null && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
        {
            if (Spawn)
            {
                currentPortal.Renderer.SetActive(true);
                currentPortal.gameObject.SetActive(true);
            }
            else
            {
                currentPortal.Renderer.SetActive(true);
                currentPortal.gameObject.SetActive(true);
                currentPortal.transform.position = PortalPos;
                currentPortal.transform.rotation = PortalRot;
            }
        }
        //-------------------------------------------------------END PORTALS------------------------------------------------------\\      
        
    }
    private void ReSizing()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && sizeScale > localScalePortals / 2)
        {
            sizeScale = sizeScale - 0.25f;
            currentPortal.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f && sizeScale <= localScalePortals * 2)
        {
            sizeScale = sizeScale + 0.25f;
            currentPortal.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);
        }
    }

    private void Detach(float ForceToApply)
    {
        if (m_CurrentAttachObjectTime >= m_AttachObjectTime)
        {
            Rigidbody l_Rigid = m_AttachObject.GetComponent<Rigidbody>();
            l_Rigid.isKinematic = false;
            l_Rigid.AddForce(m_Camera.transform.forward * ForceToApply);
            if(m_AttachObject is Companion)
            {
                Companion l_Companion = m_AttachObject.GetComponent<Companion>();
                l_Companion.SetTelportActive(true);
            }
            m_AttachObject.transform.SetParent(null);
            m_ThrewCompanion = true;
            StartCoroutine(DelayCanAttach());
        }
    }
    private void PlayerCamera()
    {
        float l_MouseAxisY = Input.GetAxis("Mouse Y");
        m_Pitch += (l_MouseAxisY) * m_PitchRotationalSpeed * Time.deltaTime * (m_InvertedPitch ? -1.0f : 1.0f);
        float l_MouseAxisX = Input.GetAxis("Mouse X");
        m_Yaw += l_MouseAxisX * m_YawRotationalSpeed * Time.deltaTime * (m_InvertedYaw ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);


        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchControllerTransform.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
    }
    private void PlayerMovement()
    {
        float l_YawInRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (m_Yaw + 90.0f) * Mathf.Deg2Rad;
        Vector3 l_Forward = new Vector3(Mathf.Sin(l_YawInRadians), 0.0f, Mathf.Cos(l_YawInRadians));
        Vector3 l_Right = new Vector3(Mathf.Sin(l_Yaw90InRadians), 0.0f, Mathf.Cos(l_Yaw90InRadians));
        Vector3 l_Movement = Vector3.zero;

        if (Input.GetKey(m_UpKeyCode))
            l_Movement = l_Forward;
        else if (Input.GetKey(m_DownKeyCode))
            l_Movement = -l_Forward;
        if (Input.GetKey(m_RightKeyCode))
            l_Movement += l_Right;
        else if (Input.GetKey(m_LeftKeyCode))
            l_Movement -= l_Right;


        float l_SpeedMultiplier = 1.0f;
        if (Input.GetKey(m_RunKeyCode))
            l_SpeedMultiplier = m_FastSpeedMultiplier;

        l_Movement.Normalize();
        m_Direction = l_Movement;

        l_Movement = l_Movement * Time.deltaTime * m_Speed * l_SpeedMultiplier;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;
        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;

        Gravity(l_Movement);
    }
    void UpdateAttachPosition()
    {

        if (m_AttachObject !=null && m_CurrentAttachObjectTime<m_AttachObjectTime)
        {
            m_CurrentAttachObjectTime += Time.deltaTime;
            float l_Pct = Mathf.Min(1.0f, m_CurrentAttachObjectTime / m_AttachObjectTime);
            m_AttachObject.transform.position = Vector3.Lerp(m_AttachObject.transform.position, m_AttachPosition.position, m_CurrentAttachObjectTime/ m_AttachObjectTime);

            m_AttachObject.transform.rotation = Quaternion.Lerp(m_AttachObject.transform.rotation, m_AttachPosition.rotation, m_CurrentAttachObjectTime / m_AttachObjectTime);

            if (l_Pct == 1.0f)
                m_AttachObject.transform.SetParent(m_AttachPosition);
        }
    }

    void Attach()
    {
        Ray l_ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_ray, out l_RaycastHit, 15.0f, m_ShootLayerMask.value))
        {
            
            if (l_RaycastHit.collider.CompareTag("Companion") || l_RaycastHit.collider.CompareTag("Turret") || l_RaycastHit.collider.CompareTag("RefractionCube"))
            {
                StartAttachObject(l_RaycastHit.collider.gameObject);
                m_ThrewCompanion = false;
            }
                
        }
    }
    void StartAttachObject(GameObject AttachObject)
    {
        if (m_AttachObject == null)
        {
            m_AttachObject = AttachObject;
            m_AttachObject.GetComponent<Rigidbody>().isKinematic = true;
            if(m_AttachObject is Companion)
            {
                Companion l_Companion = m_AttachObject.GetComponent<Companion>();
                l_Companion.SetTelportActive(false);
            }
            m_CurrentAttachObjectTime = 0;
        }
    }

    public bool CanAttach()
    { 
       return m_AttachObject==null;
    }
    private IEnumerator DelayCanAttach()
    {
        yield return new WaitForSeconds(0.2f);
        m_AttachObject = null;
    }

    private void Gravity(Vector3 movement)
    {
        CollisionFlags l_CollisionFlags = m_CharacterController.Move(movement);
        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            touchingGround += Time.deltaTime;
            m_OnGround = true;
            m_VerticalSpeed = 0.0f;
        }
        else
            m_OnGround = false;
        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;

        if (touchingGround > touchingGroundValue && m_OnGround && Input.GetKeyDown(m_JumpKeyCode))
        {
            m_VerticalSpeed = m_JumpSpeed;
            touchingGround = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
            Teleport(other.GetComponent<Portal>());

    }

    public void Teleport(Portal PortalToTeleport)
    {
        Vector3 l_PortalZX = PortalToTeleport.transform.forward;
        l_PortalZX.y = 0;
        l_PortalZX.Normalize();
        float l_Dot = Vector3.Dot(l_PortalZX, -m_Direction);
        if (l_Dot > m_DotToEnterPortal)
        {
            m_CharacterController.enabled = false;

            Vector3 l_LocalPosition = PortalToTeleport.transform.InverseTransformPoint(transform.position); //pasamos de mundo a local
            Vector3 l_LocalDirection = PortalToTeleport.transform.InverseTransformDirection(-transform.forward);
            transform.position = PortalToTeleport.m_OtherPortal.transform.TransformPoint(l_LocalPosition);
            transform.forward = PortalToTeleport.m_OtherPortal.transform.TransformDirection(l_LocalDirection);

            m_CharacterController.enabled = true;
            m_Yaw = transform.rotation.eulerAngles.y;

            Vector3 l_LocalMovementDirection = PortalToTeleport.transform.InverseTransformDirection(-m_Direction);
            m_Direction = PortalToTeleport.m_OtherPortal.transform.TransformDirection(l_LocalMovementDirection);
        }
    }

    public void ShootPortal(Portal PortalToSpawn)
    {

        currentPortal = PortalToSpawn;
        currentPortal.gameObject.SetActive(false);
        Ray l_ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_ray, out l_RaycastHit, 20.0f, m_ShootLayerMask.value))
        {
            PortalShoot.Play();
            currentPortal.transform.position = l_RaycastHit.point;
            currentPortal.transform.rotation = Quaternion.LookRotation(l_RaycastHit.normal);
            nextPos = l_RaycastHit.point;
            nextRot = l_RaycastHit.normal;

            if (currentPortal.IsValidPosition(nextPos, nextRot))
            {
                currentPortal.gameObject.SetActive(true);
                currentPortal.Renderer.SetActive(true);
                Spawn = true;
            }
            else
                Spawn = false;
        }
    }

    public void Death()
    {
       //ps a poner la funsiona aquo
    }
}


