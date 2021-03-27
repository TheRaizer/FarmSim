using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingSpriteRenderer : MonoBehaviour
{
    [SerializeField] private float flashInterval;
    private float timer;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= flashInterval)
        {
            timer = 0;
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }
}
