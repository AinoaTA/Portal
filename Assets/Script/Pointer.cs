using UnityEngine.UI;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public Sprite Portals;
    public Sprite NonePortals;
    //más tarde configuro esto.
    public Sprite Orange;
    public Sprite Blue;
    public LayerMask m_LayerMask;

    private Image currentImage;

    private void Awake()
    {
        currentImage = GetComponent<Image>();
    }

    private void Update()
    {
        ChangePortals();
    }
    public void ChangePortals()
    {
        Ray l_ray = GameController.GetGameController().GetPlayer().m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit l_RaycastHit;

        if (Physics.Raycast(l_ray, out l_RaycastHit, 20.0f) && l_RaycastHit.collider.CompareTag("WallDrawable")) //m_LayerMask))
               currentImage.sprite = Portals;
        else
            currentImage.sprite = NonePortals;
    }

}
