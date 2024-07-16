using UnityEngine;

namespace Unidork.Constants
{
	/// <summary>
	/// Common animator parameters.
	/// </summary>
	public static class AnimatorConstants
    {
        #region Tags

        public const string IdleAnimatorTag = "Idle";
        public const string StandAnimatorTag = "Stand";
	    public const string MoveAnimatorTag = "Move";
        public const string WalkAnimatorTag = "Walk";
        public const string RunAnimatorTag = "Run";
        public const string SneakAnimatorTag = "Sneak";
        public const string CrouchAnimatorTag = "Crouch";
        public const string JumpAnimatorTag = "Jump";
        public const string RollAnimatorTag = "Roll";
        public const string DashAnimatorTag = "Dash";
        public const string SlideAnimatorTag = "Slide";
        public const string AimAnimatorTag = "Aim";
        public const string AttackAnimatorTag = "Attack";
        public const string DieAnimatorTag = "Die";
        public const string DanceAnimatorTag = "Dance";
        public const string CelebrateAnimatorTag = "Celebrate";

        public const string IntroAnimatorTag = "Intro";
        public const string OutroAnimatorTag = "Outro";
        public const string PressAnimatorTag = "Press";
	    public const string PlayAnimatorTag = "Play";
	    public const string StartAnimatorTag = "Start";

	    public const string ShowAnimatorTag = "Show";
	    public const string HideAnimatorTag = "Hide";

        #endregion

        #region Hashes

        public static readonly int IdleAnimatorHash = Animator.StringToHash(IdleAnimatorTag);
        public static readonly int StandAnimatorHash = Animator.StringToHash(StandAnimatorTag);
	    public static readonly int MoveAnimatorHash = Animator.StringToHash(MoveAnimatorTag);
        public static readonly int WalkAnimatorHash = Animator.StringToHash(WalkAnimatorTag);
        public static readonly int RunAnimatorHash = Animator.StringToHash(RunAnimatorTag);
        public static readonly int SneakAnimatorHash = Animator.StringToHash(SneakAnimatorTag);
        public static readonly int CrouchAnimatorHash = Animator.StringToHash(CrouchAnimatorTag);
        public static readonly int JumpAnimatorHash = Animator.StringToHash(JumpAnimatorTag);
        public static readonly int RollAnimatorHash = Animator.StringToHash(RollAnimatorTag); 
        public static readonly int DashAnimatorHash = Animator.StringToHash(DashAnimatorTag); 
        public static readonly int SlideAnimatorHash = Animator.StringToHash(SlideAnimatorTag); 
        public static readonly int AimAnimatorHash = Animator.StringToHash(AimAnimatorTag);
        public static readonly int AttackAnimatorHash = Animator.StringToHash(AttackAnimatorTag);
        public static readonly int DieAnimatorHash = Animator.StringToHash(DieAnimatorTag);
        public static readonly int DanceAnimatorHash = Animator.StringToHash(DanceAnimatorTag);
        public static readonly int CelebrateAnimatorHash = Animator.StringToHash(CelebrateAnimatorTag);

        public static readonly int IsIdlingAnimatorHash = Animator.StringToHash("IsIdling");
        public static readonly int IsStandingAnimatorHash = Animator.StringToHash("IsStanding");
        public static readonly int IsMovingAnimatorHash = Animator.StringToHash("IsMoving");
        public static readonly int IsWalkingAnimatorHash = Animator.StringToHash("IsWalking");
        public static readonly int IsRunningAnimatorHash = Animator.StringToHash("IsRunning");
        public static readonly int IsSneakingAnimatorHash = Animator.StringToHash("IsSneaking");
        public static readonly int IsCrouchingAnimatorHash = Animator.StringToHash("IsCrouching");
        public static readonly int IsJumpingAnimatorHash = Animator.StringToHash("IsJumping");
        public static readonly int IsRollingAnimatorHash = Animator.StringToHash("IsRolling");
        public static readonly int IsDashingAnimatorHash = Animator.StringToHash("IsDashing");
        public static readonly int IsSlidingAnimatorHash = Animator.StringToHash("IsSliding");
        public static readonly int IsAimingAnimatorHash = Animator.StringToHash("IsAiming");
        public static readonly int IsAttackingAnimatorHash = Animator.StringToHash("IsAttacking");
        public static readonly int IsDyingAnimatorHash = Animator.StringToHash("IsDying");
        public static readonly int IsDancingAnimatorHash = Animator.StringToHash("IsDancing");
        public static readonly int IsCelebratingAnimatorHash = Animator.StringToHash("IsCelebrating");

        public static readonly int IntroAnimatorHash = Animator.StringToHash(IntroAnimatorTag);
        public static readonly int OutroAnimatorHash = Animator.StringToHash(OutroAnimatorTag);
        public static readonly int PressAnimatorHash = Animator.StringToHash(PressAnimatorTag);
	    public static readonly int PlayAnimatorHash = Animator.StringToHash(PlayAnimatorTag);
	    public static readonly int StartAnimatorHash = Animator.StringToHash(StartAnimatorTag);

	    public static readonly int MoveSpeedAnimatorHash = Animator.StringToHash("MoveSpeed");

	    public static readonly int ShowAnimatorHash = Animator.StringToHash(ShowAnimatorTag);
	    public static readonly int HideAnimatorHash = Animator.StringToHash(HideAnimatorTag);

	    #endregion
    }
}