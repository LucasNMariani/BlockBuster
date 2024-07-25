using System.Collections;

//using System;
using UnityEngine;


public class Bricks : MonoBehaviour
{
    [SerializeField] protected float _maxLife;
    [SerializeField] protected float _currentLife;
    [SerializeField] protected Color _maxLifeColor;
    [SerializeField] protected Color _lowLifeColor;
    [SerializeField] protected PowerUp[] _powerUps;
    [SerializeField] protected Animator _screen;
    protected bool _canDestroy;

    public bool canDestroy { get { return _canDestroy; } }

    protected Material _myMaterial;
    public float MaxLife => _maxLife;
   
    void Start()
    {
        _currentLife = _maxLife;
        _myMaterial = gameObject.GetComponent<MeshRenderer>().material;
        _myMaterial.color = _maxLifeColor;
        if (GameManager.instance != null) GameManager.instance.AddBrick(this);
        
        //EventManager.Subscribe(TypeEvent.ResetLevel, ResetLevelBrick);
    }

    public virtual void Damage(float damagePoints)
    {
        
        if (_currentLife != 0 && damagePoints != 0)
        {
            _currentLife -= damagePoints;

            //Llama una Coroutine el cual cambia el color suavemente.
            StartCoroutine(Lerp(_myMaterial.color));
        }
        if (_currentLife <= 0) Destroyed();
        else AudioManager.Instance.PlaySFX("MenuSound1");

    }

    public void ResetLevelBrick(params object[] parameters)
    {
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true); //En caso que no funcione porque está apagado, que se haga esta consulta y se active desde el GameManager
        _currentLife = _maxLife;
        GameManager.instance.AddBrick(this);
    }

    protected void Destroyed()
    {
        
        Debug.Log("Bloque destruido");
        AudioManager.Instance.PlaySFX("BrickDestroyedSound");
        _screen.SetTrigger("CameraShake");
        GameManager.instance.RemoveBrick(this);

        if (Random.value >= 0.7f && _powerUps.Length > 0)
        {
            //Instanciar PowerUp Random
            int randomValue = Random.Range(0, 5);
            if(randomValue == 0)
            {
               Instantiate(_powerUps[randomValue]);
                _powerUps[randomValue].transform.position = transform.position;
            }
             else
             {
                Instantiate(_powerUps[randomValue]);
               _powerUps[randomValue].transform.position = transform.position;
             }
            
        }
        Destroy(gameObject);
    }

    IEnumerator Lerp(Color a)
    {
        float ticks = 0;

        //Hace un Lerp entre el color actual del material y lowLifeColor, y lo guarda en una variable.
        Color b = Color.Lerp(_lowLifeColor, a, _currentLife / _maxLife);

        while (ticks <= 1)
        {
            //Cambia el color con suavidad usando el Lerp anterior y el color que haya actualmente.
            _myMaterial.color = Color.Lerp(a, b, ticks * 2);

            ticks += Time.deltaTime;

            yield return null;
        }
    }
}
