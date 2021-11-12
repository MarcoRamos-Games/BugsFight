using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] damageSprites;
    public float lifeTime;

    private void Start()
    {
        Invoke(nameof(Destruction), lifeTime);
    }
    public void Setup(int damage)
    {
        GetComponent<SpriteRenderer>().sprite = damageSprites[damage - 1];
    }

    void Destruction()
    {
        Destroy(gameObject);
    }
}
