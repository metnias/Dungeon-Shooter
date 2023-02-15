using UnityEngine;

public class VPad_Handler : MonoBehaviour
{
    public float maxLength = 70f;
    public bool is4DPad = false;
    private GameObject player;
    private Vector2 defPos;
    private Vector2 downPos;
    private RectTransform rTransform;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rTransform = GetComponent<RectTransform>();
        defPos = rTransform.localPosition;
    }

    private void Update()
    {

    }

    public void Attack() => player.GetComponent<Player_Bow>().Shoot();

    public void PadDown()
    {
        downPos = Input.mousePosition;
    }

    public void PadUp()
    {
        rTransform.localPosition = defPos;
        var pCtrl = player.GetComponent<Player_Controller>();
        pCtrl.SetAxis(0f, 0f);
    }

    public void PadDrag()
    {
        Vector2 mousePos = Input.mousePosition;

        var newTabPos = mousePos - downPos;
        var axis = newTabPos.normalized;

        float len = newTabPos.magnitude;
        if (len > maxLength)
        {
            newTabPos.x = axis.x * maxLength;
            newTabPos.y = axis.y * maxLength;
        }

        rTransform.localPosition = defPos + newTabPos;

        var pCtrl = player.GetComponent<Player_Controller>();
        pCtrl.SetAxis(axis.x, axis.y);
    }

}
