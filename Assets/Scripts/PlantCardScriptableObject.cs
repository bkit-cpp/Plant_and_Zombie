using UnityEngine;

[CreateAssetMenu(menuName ="Cards/Plant Card", fileName ="New Plant Card")]
public class PlantCardScriptableObject : ScriptableObject
{
    public Texture plantIcon;
    public GameObject Bullet;
    public Sprite plantSprite;
    public LayerMask zombieLayer;
    public int cost;
    public float cooldown;
    public bool isSunFlower;
    public SunSpawner sunSpawnerTemplate;
    public float health;
    public float damage;
    public float range;
    public float speed;
    public float fireRate;
    [Header("Mine Parameters")]
    public bool isMine;
    public GameObject Explosion;
    public float growDuration;
    public Sprite grownSprite;
    public float blinkingRate;
    [Header("0 : Grey ungrown, 1 : red ungrown, 2 : Grey grown, 3 : red ungrown")]
    public Sprite[] mineStates;

    public Vector2 size;
}
