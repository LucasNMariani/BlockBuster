
using UnityEngine;

public class IndestructableBricks : Bricks
{
    [SerializeField] GameObject[] _bricksLeft;
    [SerializeField] Color _blockedColor;
    private void Start()
    {
        _currentLife = _maxLife;
        _myMaterial = gameObject.GetComponent<MeshRenderer>().material;
        _myMaterial.color = _blockedColor;
        if (GameManager.instance != null) GameManager.instance.AddBrick(this);
        _canDestroy = false;
    }

    public override void Damage(float damagePoints)
    {
        if (!_canDestroy) return;

        base.Damage(damagePoints);
    }

    public void CantDestroy()
    {
       if(!IsEveryBrickDestroyed())
       {
            Debug.Log("Bloque Indestructible");
            AudioManager.Instance.PlaySFX("BrickIndestructableFound");
            _screen.SetTrigger("CameraShake");
            return;
       }

        _canDestroy = true;
        _myMaterial.color = _maxLifeColor;
        
    }

    bool IsEveryBrickDestroyed()
    {
        foreach (var brick in _bricksLeft)
        {
            if (brick != null) return false;
        }

        return true;
    }
}
