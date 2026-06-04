using UnityEngine;

/// <summary>
/// Global constants used throughout the game.
/// Centralized configuration for easy tweaking.
/// </summary>
public static class GameConstants
{
    // Combat timings
    public const float COMBO_WINDOW = 1.5f;          // Seconds between hits
    public const float PERFECT_BLOCK_WINDOW = 0.15f; // 150ms window
    public const float INPUT_BUFFER_TIME = 0.1f;     // Input buffer duration
    
    // Damage modifiers
    public const float COMBO_DAMAGE_REDUCTION = 0.05f; // 5% per combo count
    public const float MIN_COMBO_MULTIPLIER = 0.5f;    // Minimum 50% damage
    public const float CRITICAL_HIT_MULTIPLIER = 1.5f; // 150% damage
    public const float BLOCKED_DAMAGE_REDUCTION = 0.4f; // 60% damage blocked
    
    // Rage system
    public const float RAGE_PER_HIT = 5f;           // Rage gained per attack hit
    public const float RAGE_PER_DAMAGE = 0.1f;      // Rage per damage taken
    public const float RAGE_DECAY_RATE = 5f;        // Rage decay per second
    public const float ULTIMATE_THRESHOLD = 100f;   // Rage needed for ultimate
    
    // AI
    public const float AI_DECISION_INTERVAL = 0.3f; // How often AI makes decisions
    public const float EASY_AI_REACTION_TIME = 0.5f;
    public const float MEDIUM_AI_REACTION_TIME = 0.3f;
    public const float HARD_AI_REACTION_TIME = 0.1f;
    public const float NIGHTMARE_AI_REACTION_TIME = 0.05f;
    
    // Physics
    public const float GROUND_DRAG = 0.95f;
    public const float AIR_DRAG = 0.85f;
    public const float JUMP_FORCE = 5f;
    
    // Match settings
    public const float MATCH_DURATION = 180f; // 3 minutes
    public const int MAX_ROUNDS = 3;
    
    // Camera
    public const float CAMERA_BASE_DISTANCE = 8f;
    public const float CAMERA_HEIGHT = 2f;
    public const float CAMERA_SHAKE_INTENSITY = 0.1f;
    public const float CAMERA_SHAKE_DURATION = 0.2f;
    
    // Layers
    public const string PLAYER_LAYER = "Player";
    public const string OPPONENT_LAYER = "Opponent";
    public const string HITBOX_LAYER = "HitBox";
}
