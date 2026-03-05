using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private static readonly int Running = Animator.StringToHash(IsRunning);
    private static readonly int Die = Animator.StringToHash(IsDie);
    private const string IsRunning = "IsRunning";
    private const string IsDie = "IsDie";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private FlashBlink _flashBlink;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlink = GetComponent<FlashBlink>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Update()
    {
        if (_animator)
        {
            _animator.SetBool(Running, Player.Instance.IsRunning());
        }

        if(Player.Instance.IsAlive())
            AdjustPlayerFacingDirection();
    }

    private void OnDestroy()
    {
         Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();

        _spriteRenderer.flipX = mousePos.x < playerPos.x;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(Die, true);
        _flashBlink.StopBlinking();
    }
}
