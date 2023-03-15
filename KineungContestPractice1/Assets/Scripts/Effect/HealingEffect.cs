using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : MonoBehaviour
{
    public float moveSpd;
    public float fadeTime;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(IFadeOut());
    }

    private IEnumerator IFadeOut()
    {
        float time = 0;
        while (time <= fadeTime)
        {
            time += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, 
                new Color(255,255,255 ,0), time / fadeTime);

            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x, transform.position.y + 0.5f, 0), time / fadeTime);
            yield return null;
        }
    }
    private void Update()
    {
        
    }
}
