using System;
using Game.Janken.Spine.Scripts;
using Luna;
using Luna.Core.Animation;
using Luna.Extensions;
using Spine.Unity;
using UnityEngine;
using USEN.Games.Janken;

public class JankenCharacterController : MonoBehaviour
{
    public event OnStateChangeDelegate OnStateChange;
    
    private Animator _animator;
    private SkeletonMecanim _skletonMecanim;
    private JankenStateBehaviour _stateBehaviour;
    private AudioSource _audioSource;

    public JankenCharacterState State { get; private set; } = JankenCharacterState.Idle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _skletonMecanim = GetComponent<SkeletonMecanim>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _stateBehaviour = _animator.GetBehaviour<JankenStateBehaviour>();
        if (_stateBehaviour != null)
            _stateBehaviour.OnAnimationStartEvent += OnAnimationStartEvent;
    }
    
    private void OnDisable()
    {
        if (_stateBehaviour != null)
            _stateBehaviour.OnAnimationStartEvent -= OnAnimationStartEvent;
    }
    
    public void StartJanken()
    {
        var result = UnityEngine.Random.Range(1, 4);
        var commentIndex = UnityEngine.Random.Range(0, 10);
        _animator.SetInteger("Result", result);
        _animator.SetFloat("Comment", commentIndex);
        _animator.SetTrigger("Start");
    }
    
    public void StopJanken()
    {
        _animator.SetTrigger("Stop");
    }
    
    public void SwitchCharacter(int index)
    {
        var skeleton = JankenCharacters.Default.characters[index].skeleton;
        var animator = JankenCharacters.Default.characters[index].animator;
        
        _animator.runtimeAnimatorController = animator;
        if (_stateBehaviour != null)
            _stateBehaviour.OnAnimationStartEvent -= OnAnimationStartEvent;
        _stateBehaviour = _animator.GetBehaviour<JankenStateBehaviour>();
        _stateBehaviour.OnAnimationStartEvent += OnAnimationStartEvent;
        _skletonMecanim.skeletonDataAsset = skeleton;
        _skletonMecanim.Initialize(true);
    }
    
    public void Play(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
    
    public void PlayAsync(string address)
    {
        new Asset<AudioClip>(address).Load().Then(Play);
    }
    
    public void ResetState()
    {
        State = JankenCharacterState.Idle;
    }
    
    private void OnAnimationStartEvent(AnimationStateBehaviour behaviour, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var state = State;
        if (stateInfo.IsTag("idle"))
            state = JankenCharacterState.Idle;
        else if (stateInfo.IsTag("waiting"))
            state = JankenCharacterState.Waiting;
        else if (stateInfo.IsTag("pon"))
            state = JankenCharacterState.Pon;
        else if (stateInfo.IsTag("end"))
            state = JankenCharacterState.End;
        else if (stateInfo.IsTag("comment")) state = JankenCharacterState.Comment;

        Debug.Log($"OnAnimationStartEvent: {state}");

        if (state != State)
        {
            State = state;
            OnStateChange?.Invoke(state);
        }
    }
    
    public enum JankenCharacterState
    {
        Idle,
        Waiting,
        Pon,
        End,
        Comment
    }
    
    public enum JankennResult
    {
        Rock = 1,
        Scissors,
        Paper
    }
    
    public delegate void OnStateChangeDelegate(JankenCharacterState state);
}
