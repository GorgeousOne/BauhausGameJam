using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {
    
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    [SerializeField] private float spacing = 1;
    private int _maxHealth;
    
    private List<SpriteRenderer> _heartContainers = new();

    void Start()
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.OnHealthChange.AddListener(_updateHealth);
        _maxHealth = player.maxHealth;
        
        for (int i = 0; i < _maxHealth; ++i) {
            GameObject heartContainer = new GameObject("heart" + i);
            SpriteRenderer sprite = heartContainer.AddComponent<SpriteRenderer>();
            sprite.sprite = emptyHeartSprite;
            
            Transform childTransform = heartContainer.transform;
            childTransform.SetParent(transform);
            childTransform.localPosition = new Vector3(i * spacing, 0, 0);
            _heartContainers.Add(sprite);
        }
        _updateHealth(player.Health);
    }

    private void _updateHealth(int newHealth) {
        if (newHealth <= 0) {
            //do endscreen stuff?
        }
        for (int i = 0; i < _maxHealth; ++i) {
            _heartContainers[i].sprite = newHealth > i ? fullHeartSprite : emptyHeartSprite;
        }
    }
}
