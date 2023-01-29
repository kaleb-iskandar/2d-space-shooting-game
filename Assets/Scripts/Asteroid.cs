using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D asteroidRb;
    [SerializeField]
    private float speed = 50f,
        maxLifetime = 30f;
    private float size { get; set; }
    private float minSize { get; set; }
    private float maxSize { get; set; }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        asteroidRb = GetComponent<Rigidbody2D>();
        size = 1f;
        minSize = .5f;
        maxSize = 1.5f;
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0, 0, Random.value * 360f);
        transform.localScale = Vector3.one * size;
        asteroidRb.mass = size;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetSize(float size)
    {
        this.size = size;
    }
    public float GetSize()
    {
        return size;
    }
    public float GetMinSize()
    {
        return this.minSize;
    }
    public float GetMaxSize()
    {
        return this.maxSize;
    }
    public void SetTrejectory(Vector2 direction)
    {
        asteroidRb.AddForce(direction * speed);

        Destroy(gameObject, maxLifetime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (size/2>=minSize)
            {
                Split();
                Split();
            }
            GameManager.instance.AsteroidDestroyed(this);
            Destroy(gameObject);
        }
    }
    private void Split()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * .5f;
        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size *= .5f;
        half.SetTrejectory(Random.insideUnitCircle.normalized * speed);
    }
}
