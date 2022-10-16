using System;
using UnityEngine;

/// <summary>
/// This is a container that houses the 'X or O' elements.
/// </summary>
internal class Tic : MonoBehaviour
{
    [NonSerialized] public bool IsClicked;
    [NonSerialized] public int Index;

    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider2D;

    public State state;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();

        Index = transform.GetSiblingIndex();
    }

    private void OnEnable()
    {
        state = State.Draw;

        IsClicked = false;
    }

    /// <summary>
    /// Initializes this container with either 'X or O'.
    /// </summary>
    public void SetOnClick(Sprite sprite, State newState)
    {
        state = newState;

        IsClicked = true;

        _spriteRenderer.sprite = sprite;

        _circleCollider2D.enabled = false;
    }
}
