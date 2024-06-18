[System.Serializable]
public struct WeaponInfo
{
    public WeaponInfo(int id, float baseDamage, float baseRate, float baseSpeed, int count, int per, ItemData.WeaponType type) : this()
    {
        ID = id;
        _baseDamage = baseDamage;
        _baseRate = baseRate;
        _baseSpeed = baseSpeed;
        this.Count = count;
        this.Per = per;
        this.Type = type;
    }
    
    public readonly int ID;
    private readonly float _baseDamage;
    private readonly float _baseRate;
    private readonly float _baseSpeed;

    public float DamageMultiplier;
    public int Count;
    public int Per;
    public float SpeedMultiplier;
    public float RateMultiplier;
    public ItemData.WeaponType Type;
    

    public float Damage() => _baseDamage * DamageMultiplier;
    public float Speed() => _baseSpeed * SpeedMultiplier;
    public float Rate() => _baseRate * RateMultiplier;
}