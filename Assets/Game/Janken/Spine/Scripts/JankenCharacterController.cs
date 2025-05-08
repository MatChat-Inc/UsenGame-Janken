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
    
    private SkeletonMecanim _skletonMecanim;
    private JankenStateBehaviour _stateBehaviour;
    private AudioSource _audioSource;

    private JankenCharacter _character;
    
    public Animator Animator { get; private set; }
    public JankenCharacterState State { get; private set; } = JankenCharacterState.Idle;
    
    public bool Mute { get; set; } = false;
    public bool NeedComment { get; set; } = true;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _skletonMecanim = GetComponent<SkeletonMecanim>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Debug.Log("JankenCharacterController Start");
    }

    private void OnEnable()
    {
        _stateBehaviour = Animator.GetBehaviour<JankenStateBehaviour>();
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
        var commentIndex = NeedComment ? UnityEngine.Random.Range(0, 10) : -1;
        Animator.SetInteger("Result", result);
        Animator.SetFloat("Comment", commentIndex);
        Animator.SetTrigger("Start");
    }
    
    public void StopJanken()
    {
        Animator.SetTrigger("Stop");
    }
    
    public void SwitchCharacter(int index)
    {
        var character = JankenCharacters.Default.characters[index];
        
        if (_character == character) return;
        _character = character;
        
        var skeleton = _character.skeleton;
        var animator = _character.animator;
        
        Animator.runtimeAnimatorController = animator;
        if (_stateBehaviour != null)
            _stateBehaviour.OnAnimationStartEvent -= OnAnimationStartEvent;
        _stateBehaviour = Animator.GetBehaviour<JankenStateBehaviour>();
        _stateBehaviour.OnAnimationStartEvent += OnAnimationStartEvent;
        _skletonMecanim.skeletonDataAsset = skeleton;
        _skletonMecanim.Initialize(true);
    }
    
    public void JumpTo(string state)
    {
        Animator.CrossFade(state, 0.1f);
    }
    
    public void Play(AudioClip audioClip)
    {
        if (Mute) return;
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
