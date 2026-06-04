using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject containing all character configuration data.
/// Used for easy character creation and balancing without code changes.
/// </summary>
[CreateAssetMenu(fileName = "Character_", menuName = "Fighting Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string characterName;
    [SerializeField] private Sprite characterPortrait;
    [SerializeField] private Material characterMaterial;
    
    [Header("Stats")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float power = 1f;
    [SerializeField] private float defense = 10f;
    [SerializeField] private float staminaRegenRate = 15f;
    
    [Header("Attacks")]
    [SerializeField] private AttackData lightPunch;
    [SerializeField] private AttackData heavyPunch;
    [SerializeField] private AttackData lightKick;
    [SerializeField] private AttackData heavyKick;
    [SerializeField] private List<ComboData> combos = new List<ComboData>();
    [SerializeField] private AttackData specialAttack;
    [SerializeField] private AttackData ultimateAttack;
    [SerializeField] private HiddenMoveData hiddenMove;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem auraParticles;
    [SerializeField] private ParticleSystem hitEffects;
    [SerializeField] private Color characterColor = Color.white;
    [SerializeField] private Color auraColor = Color.white;
    
    [Header("Audio")]
    [SerializeField] private AudioClip combatMusic;
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip kickSound;
    [SerializeField] private AudioClip specialSound;
    [SerializeField] private AudioClip ultimateSound;
    
    // Getters
    public string CharacterName => characterName;
    public Sprite CharacterPortrait => characterPortrait;
    public Material CharacterMaterial => characterMaterial;
    public float MaxHP => maxHP;
    public float Speed => speed;
    public float Power => power;
    public float Defense => defense;
    public float StaminaRegenRate => staminaRegenRate;
    
    public AttackData LightPunch => lightPunch;
    public AttackData HeavyPunch => heavyPunch;
    public AttackData LightKick => lightKick;
    public AttackData HeavyKick => heavyKick;
    public List<ComboData> Combos => combos;
    public AttackData SpecialAttack => specialAttack;
    public AttackData UltimateAttack => ultimateAttack;
    public HiddenMoveData HiddenMove => hiddenMove;
    
    public ParticleSystem AuraParticles => auraParticles;
    public ParticleSystem HitEffects => hitEffects;
    public Color CharacterColor => characterColor;
    public Color AuraColor => auraColor;
    
    public AudioClip CombatMusic => combatMusic;
    public AudioClip PunchSound => punchSound;
    public AudioClip KickSound => kickSound;
    public AudioClip SpecialSound => specialSound;
    public AudioClip UltimateSound => ultimateSound;
}

[System.Serializable]
public class AttackData
{
    public string name;
    public AttackType type;
    public float baseDamage = 10f;
    public float staminaCost = 10f;
    public float knockbackForce = 5f;
    public Vector3 hitBoxSize = Vector3.one;
    public float hitBoxDuration = 0.2f;
    public float animationDuration = 0.5f;
    public float cooldown = 0f;
    public bool canBeBlocked = true;
    public bool isAirAttack = false;
    public ReactionType reactionType = ReactionType.Light;
}

[System.Serializable]
public class ComboData
{
    public string name;
    public List<AttackData> sequence = new List<AttackData>();
    public List<float> timings = new List<float>(); // Time window for each hit
    public bool isHidden = false;
    public string unlockedBy = "";
}

[System.Serializable]
public class HiddenMoveData
{
    public string name;
    public List<InputType> inputSequence = new List<InputType>();
    public float maxTimeBetweenInputs = 0.5f;
    public AttackData attack;
    public string description;
}

public enum AttackType
{
    Light,
    Medium,
    Heavy,
    Special,
    Ultimate
}

public enum ReactionType
{
    None,
    Light,
    Medium,
    Heavy,
    Knockdown
}

public enum InputType
{
    Forward,
    Back,
    Jump,
    Punch,
    Kick,
    Block,
    Special
}
