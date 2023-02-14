using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bow : MonoBehaviour
{
    public float shootSpeed = 12f;
    public float shotDelay = 0.25f;
    public GameObject bowPrefab;
    public GameObject arrowPrefab;

    private GameObject bow;
    private Player_Controller ctrler;

    private bool shooting = false;

    private void Start()
    {
        var pos = transform.position;
        bow = Instantiate(bowPrefab, pos, Quaternion.identity, transform);
        ctrler = GetComponent<Player_Controller>();
    }

    private void Update()
    {
        float bowZ = -1f;
        if (ctrler.angleZ > 30f && ctrler.angleZ < 150f) bowZ = 1f; // go behind when aiming upwards
        bow.transform.localRotation = Quaternion.Euler(0f, 0f, ctrler.angleZ);
        bow.transform.localPosition = new Vector3(0f, 0f, bowZ); // Z sort

        if (Input.GetKey(KeyCode.Z)) Shoot();
    }

    public void Shoot()
    {
        if (Player_Inventory.numArrow < 1 || shooting) return;
        //Player_Inventory.numArrow--; // use arrow
        shooting = true; // set cooldown

        var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0f, 0f, ctrler.angleZ)); // create arrow
        Vector2 vel = new(Mathf.Cos(ctrler.angleZ * Mathf.Deg2Rad), Mathf.Sin(ctrler.angleZ * Mathf.Deg2Rad)); // set direction
        vel *= shootSpeed; // and speed
        Debug.Log($"{ctrler.angleZ}: {vel.x},{vel.y}");
        arrow.GetComponent<Rigidbody2D>().AddForce(vel, ForceMode2D.Impulse); // apply velocity
        Destroy(arrow, 5f);

        Invoke(nameof(ShootEnd), shotDelay);
    }

    private void ShootEnd() => shooting = false;
}
