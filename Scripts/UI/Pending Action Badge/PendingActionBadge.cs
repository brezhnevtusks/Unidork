using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unidork.Attributes;
using Unidork.Extensions;
using Unidork.Tweens;
using Unidork.Utility;
using UniRx;
using UnityEngine;

namespace Unidork.UI
{
	/// <summary>
	/// Controls a UI element that is shown whenever a menu item like a button or a tab has actions that
	/// the player can perform (collect reward, complete operation, etc).
	/// </summary>
	public class PendingActionBadge : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Object that gets the reference to the GameObject that has a component that implements <see cref="IPendingActionBadgeUser"/>.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Object that gets the reference to the GameObject that has a component that implements the IPendingActionBadgeUser interface.")]
		[SerializeField]
		private GameObjectGetter badgeUserGetter = new GameObjectGetter();

		/// <summary>
		/// Type of animation for showing this badge.
		/// </summary>
		[Space(20)]
		[Tooltip("Type of animations for showing this badge.")]
		[SerializeField]
		private PendingActionBadgeAnimationType showAnimationType = default;

		/// <summary>
		/// Name of the show trigger for this badge's animator.
		/// </summary>
		[ShowIf("@this.showAnimationType", PendingActionBadgeAnimationType.Animator)]
		[Tooltip("Name of the show trigger for this badge's animator.")]
		[SerializeField]
		private string showTriggerName = "Show";
		
		/// <summary>
		/// Settings to use for the show tween.
		/// </summary>
		[ShowIf("@this.showAnimationType", PendingActionBadgeAnimationType.Tween)]
		[Tooltip("Settings to use for the show tween.")]
		[SerializeField]
		private TweenSettings showTweenSettings = TweenSettings.CreateLinearTweenSettings();
		
		/// <summary>
		/// Type of animation for hiding this badge.
		/// </summary>
		[Space(10)]
		[Tooltip("Type of animation for hiding this badge.")]
		[SerializeField]
		private PendingActionBadgeAnimationType loopAnimationType = default;

		/// <summary>
		/// Type of loop animation for this based.
		/// </summary>
		[Space(10)]
		[Tooltip("Type of loop animation for this based.")]
		[SerializeField]
		private PendingActionBadgeAnimationType hideAnimationType = default;
		
		/// <summary>
		/// Name of the hide trigger for this badge's animator.
		/// </summary>
		[ShowIf("@this.hideAnimationType", PendingActionBadgeAnimationType.Animator)]
		[Tooltip("Name of the hide trigger for this badge's animator.")]
		[SerializeField]
		private string hideTriggerName = "Hide";
		
		/// <summary>
		/// Settings to use for the hide tween.
		/// </summary>
		[ShowIf("@this.hideAnimationType", PendingActionBadgeAnimationType.Tween)]
		[Tooltip("Settings to use for the hide tween.")]
		[SerializeField]
		private TweenSettings hideTweenSettings = TweenSettings.CreateLinearTweenSettings();

		/// <summary>
		/// Badge animator.
		/// </summary>
		[SerializeField, HideInInspector]
		private Animator animator;

		/// <summary>
		/// Component that this badge is tied to.
		/// </summary>
		private IPendingActionBadgeUser badgeUser;

		/// <summary>
		/// Hash for badge animator's show trigger.
		/// </summary>
		private int showTriggerHash;

		/// <summary>
		/// Hash for badge animator's hide trigger.
		/// </summary>
		private int hideTriggerHash;

		/// <summary>
		/// All active UniRx subscriptions this badge has.
		/// </summary>
		private readonly CompositeDisposable disposables = new CompositeDisposable();

		private CancellationTokenSource cancellationTokenSource;

		#endregion

		#region Init

		protected virtual void Awake()
		{
			switch (showAnimationType)
			{
				case PendingActionBadgeAnimationType.None:
					gameObject.SetActive(false);
					break;
				case PendingActionBadgeAnimationType.Animator:
					showTriggerHash = Animator.StringToHash(showTriggerName);
					break;
				case PendingActionBadgeAnimationType.Tween:
					transform.localScale = Vector3.zero;
					break;
			}

			if (hideAnimationType == PendingActionBadgeAnimationType.Animator)
			{
				hideTriggerHash = Animator.StringToHash(hideTriggerName);
			}

			GameObject badgeUserGo = badgeUserGetter.GameObject;

			if (badgeUserGo == null)
			{
				Debug.LogError($"Pending action badge {name} failed to find a pending action user!");
				return;
			}

			badgeUser = badgeUserGo.GetComponent<IPendingActionBadgeUser>();

			cancellationTokenSource = new CancellationTokenSource();
			
			CreateSubscriptionsAsync(cancellationTokenSource.Token).Forget();
		}

		/// <summary>
		/// Creates the pending action subscriptions asynchronously after the connected badge user is initialized.
		/// </summary>
		private async UniTaskVoid CreateSubscriptionsAsync(CancellationToken cancellationToken)
		{
			await UniTask.WaitUntil(() => badgeUser.HasPendingActions != null, cancellationToken: cancellationToken);

			if (cancellationTokenSource == null || cancellationToken.IsCancellationRequested)
			{
				return;
			}

			badgeUser
				.HasPendingActions
				.Subscribe(hasPendingActions =>
				{
					if (hasPendingActions)
					{
						Show();
					}
					else
					{
						Hide();
					}
				})
				.AddTo(disposables);
			
			cancellationTokenSource.Dispose();
		}

		private void Reset()
		{
			animator = GetComponent<Animator>();
		}

		#endregion

		#region Badge

		/// <summary>
		/// Shows the badge and plays the show animation if needed.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		private void Show()
		{
			switch (showAnimationType)
			{
				case PendingActionBadgeAnimationType.None:
					transform.localScale = Vector3.one;
					gameObject.SetActive(true);
					break;
				case PendingActionBadgeAnimationType.Animator:
					animator.ResetAllTriggers();
					animator.SetTrigger(showTriggerHash);
					break;
				case PendingActionBadgeAnimationType.Tween:
					transform.localScale = Vector3.zero;
					transform.DOScale(Vector3.one, showTweenSettings.Duration)
					         .SetEase(showTweenSettings.EaseFunction);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Hides the badge and plays the hide animation if needed.
		/// </summary>
		private void Hide()
		{
			switch (hideAnimationType)
			{
				case PendingActionBadgeAnimationType.None:
					gameObject.SetActive(false);
					break;
				case PendingActionBadgeAnimationType.Animator:
					animator.ResetAllTriggers();
					animator.SetTrigger(hideTriggerHash);
					break;
				case PendingActionBadgeAnimationType.Tween:
					transform.DOScale(Vector3.zero, hideTweenSettings.Duration)
					         .SetEase(hideTweenSettings.EaseFunction);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion

		#region Destroy

		private void OnDestroy()
		{
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;
			disposables.Clear();	
		}

		#endregion
	}    
}