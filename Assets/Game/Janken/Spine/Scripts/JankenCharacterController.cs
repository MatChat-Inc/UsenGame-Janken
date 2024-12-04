using System;
using Game.Janken.Spine.Scripts;
using Luna;
using Luna.Extensions.Unity;
using Spine.Unity;
using UnityEngine;

public class JankenCharacterController : MonoBehaviour
{
    public event OnStateChangeDelegate OnStateChange;
    
    private Animator _animator;
    private SkeletonMecanim _skletonMecanim;
    private JankenStateBehaviour _stateBehaviour;

    public JankenCharacterState State { get; private set; } = JankenCharacterState.Idle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _skletonMecanim = GetComponent<SkeletonMecanim>();
        _stateBehaviour = _animator.GetBehaviour<JankenStateBehaviour>();
        
        _stateBehaviour.OnAnimationStartEvent += (behaviour, animator, stateInfo, layerIndex) =>
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
            else if (stateInfo.IsTag("comment"))
                state = JankenCharacterState.Comment;
            
            if (state != State)
            {
                State = state;
                OnStateChange?.Invoke(state);
            }
        };
    }

    private void Start()
    {
        
    }
    
    private void Update()
    {
        
    }
    
    public void StartJanken()
    {
        var result = UnityEngine.Random.Range(1, 4);
        var commentIndex = UnityEngine.Random.Range(0, 11);
        _animator.SetInteger("Result", result);
        _animator.SetFloat("Comment", commentIndex);
        
        _animator.SetTrigger("Start");
    }
    
    public void Play(AudioClip audioClip)
    {
        SFXManager.Play(audioClip);
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
