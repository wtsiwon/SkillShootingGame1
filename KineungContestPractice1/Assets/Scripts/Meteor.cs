using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public Vector3 endPos;
    public Vector3 startPos;

    public float duration;

    public int dmg;
    public float spinSpd;

    public GameObject meteor;

    void Start()
    {
        GameManager.Instance.CameraShake(duration + 0.5f, 0.2f);
        transform.position = startPos;
        StartCoroutine(IMeteorMove());
    }

    private IEnumerator IMeteorMove()
    {
        float time = 0;
        while(true)
        {
            if (time >= duration) break;
            transform.position = Vector3.Lerp(transform.position, endPos, time / duration);
            time = Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        meteor.transform.Rotate(new Vector3(0, 0, spinSpd) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.OnDamaged(dmg);
        }
        else if(collision.TryGetComponent(out Player player))
        {
            player.OnDamaged(dmg);
        }
    }

}
