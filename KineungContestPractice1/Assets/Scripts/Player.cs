using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float moveSpd;

    public float atkSpd;

    public float bulletSpd;

    public bool isShoot;

    public int level;

    public Vector3 clampPosition;

    public Bullet bullet;

    void Start()
    {
        StartCoroutine(IShoot());
    }

    void Update()
    {
        Move();
        InputShootKey();
        transform.position = ClampPosition();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, y, 0);

        transform.Translate(dir * moveSpd * Time.deltaTime);
    }

    private void InputShootKey()
    {
        if (Input.GetKey(KeyCode.J))
        {
            isShoot = true;
        }
        else
        {
            isShoot = false;
        }
    }

    private IEnumerator IShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (isShoot == true)
            {
                Bullet bullet1 = Instantiate(bullet);
                bullet1.SetBullet(transform.position, Vector3.up ,bulletSpd, level);
                yield return new WaitForSeconds(atkSpd);
            }

        }
    }

    private Vector3 ClampPosition()
    {
        Vector3 vector3 = new Vector3();

        vector3.x = Mathf.Clamp(transform.position.x, -clampPosition.x, clampPosition.x);
        vector3.y = Mathf.Clamp(transform.position.y, -clampPosition.y, clampPosition.y);
        vector3.z = 0;
        return vector3;
    }

    /// <summary>
    /// 파트너 추가
    /// </summary>
    public void AddPartner()
    {
        
    }

    private void Invicibility(float time)
    {

    }
}
