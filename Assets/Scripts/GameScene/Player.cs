using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _health = 100.0f;
    private float maxHealth;
    public float Health
    {
        get => _health;
        set
        {
            //Debug.Log("Health:" + _health);
            _health = value;
            if (_health < 0)
            {
                Conductor.ShowScene(Conductor.Scenes.MainMenu);
            }
            Mathf.Clamp(_health, 0, 100);
        }
    }

    [SerializeField]
    private float healingValue = 0.5f;

    [SerializeField]
    private float _hunger = 100.0f;
    public float Hunger
    {
        get => _hunger;
        set
        {
            //Debug.Log("Hunger:" + _hunger);
            _hunger = value;
        }
    }

    [SerializeField]
    private float appetiteValue = 0.1f;

    [SerializeField]
    private float hungerDamageValue = 0.01f;

    [SerializeField]
    private bool _damage = false;
    public bool Damage
    {
        get => _damage;
        set 
        {
            _damage = value;
        }
    }

    [SerializeField]
    private GameObject damageVision;
    private Material damageVisionMaterial;

    public const string FEATHERINGEFFECT = "_FeatheringEffect";
    public const string VIGNETTECOLOR = "_VignetteColor";

    public const float MINFEATHERINGEFFECT = 0.08f;
    public const float MAXFEATHERINGEFFECT = 0.3f;

    public const float MINALPHAVIGNETTECOLOR = 0;
    public const float MAXALPHAVIGNETTECOLORT = 0.96f;

    private void Start()
    {
        maxHealth = _health;
        damageVisionMaterial = damageVision.GetComponent<Renderer>().sharedMaterial;
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;

        Healing(deltaTime);
        Appetite(deltaTime);
        ChangeVision();
    }

    #region Health
    public void DamagePlayer(float damageValue)
    {
        Health -= damageValue;
    }

    private void Healing(float deltaTime)
    {
        if (!Damage && Health < maxHealth)
        {
            Health += healingValue * deltaTime;
        }
    }
    #endregion

    #region Hunger   
    private void Appetite(float deltaTime)
    {
        if (Hunger > 0)
        {
            Hunger -= appetiteValue * deltaTime;
        }
        else
        {
            DamagePlayer(hungerDamageValue);
        }
    }

    public void Eating(float foodValue)
    {
        Hunger += foodValue;
    }
    #endregion

    #region Vision
    private void ChangeVision()
    {
        float featheringEffect = MAXFEATHERINGEFFECT - ((MAXFEATHERINGEFFECT - MINFEATHERINGEFFECT) / 100 * (100 - Hunger));
        float alphaVignetteColor = (MAXALPHAVIGNETTECOLORT - MINALPHAVIGNETTECOLOR) / 100 * (100 - Hunger);
        float rVignetteColor = 0;

        if (Health < 50 || Hunger <= 0 || Damage)
        {
            rVignetteColor = 0.75f;

            if (featheringEffect > 0.1)
            {
                featheringEffect = 0.1f;
                alphaVignetteColor = 0.7f;
            }
        }

        damageVisionMaterial.SetFloat(FEATHERINGEFFECT, featheringEffect);
        damageVisionMaterial.SetColor(VIGNETTECOLOR, new(rVignetteColor, 0, 0, alphaVignetteColor));
    }
    #endregion
}