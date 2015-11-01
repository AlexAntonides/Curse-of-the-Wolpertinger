using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Health))]
public class BaseController : MonoBehaviour {

    public GameObject hitLocation;
    private Health _healthComp;

    public Image[] _hearts = new Image[10];

    void Start()
    {
        _healthComp = GetComponent<Health>();
    }

    void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        for(int i = 0; i < _hearts.Length; i++)
        {
            if (i < _healthComp.health)
            {
                _hearts[i].gameObject.SetActive(true);
            }
            else
            {
                _hearts[i].gameObject.SetActive(false);
            }
        }

        if (_healthComp.health <= 0)
        {
            // Win() or Lose();
        }
    }
}
