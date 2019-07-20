using Assets.Lib.Civilization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Lib.Events;
public class UIModifiersDisplayer : MonoBehaviour
{
    
    [SerializeField]
    private GameObject modifierIconPrefab; 
    [SerializeField]
    private Vector3 initialPosition;
    [SerializeField]
    private float offset;

    private HashSet<Modifier> currentModifiers = new HashSet<Modifier>();

    void Start()
    {
        modifierIconPrefab = GetComponentInChildren<ModifierIcon>(true).gameObject;

        ObjectSelector.Instance.AddSelectionObserver(OnSelect);
        RectTransform rectTransform = modifierIconPrefab.GetComponent<RectTransform>();

        initialPosition = rectTransform.localPosition   ;
        offset = rectTransform.sizeDelta.x;
    }


    private void OnSelect(List<GameObject> gameObjects)
    {
        RemoveOldIcons();
        if (gameObjects.Count == 1)
        {
            GameObject target = gameObjects[0];
            HashSet<Modifier> modifiers = target.GetModifiers();

            CreateIcons(modifiers);
            currentModifiers = modifiers;

            foreach(Modifier modifier in modifiers)
            {
                modifier.OnDestroyEvent(() =>
                {
                    OnModifierDestroyed(modifier);
                });
            }
        }
        
    }

    private void OnModifierDestroyed(Modifier modifier)
    {
        currentModifiers.Remove(modifier);
        CreateIcons(this.currentModifiers);
    }
  

    private void CreateIcons(HashSet<Modifier> modifiers)
    {
        int i = 0;
        foreach (Modifier modifier in modifiers)
        {
            float positionX = this.initialPosition.x + (offset * i);
            GameObject icon = Instantiate(this.modifierIconPrefab, this.transform);

            RectTransform rectTransform = icon.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(positionX, rectTransform.localPosition.y, rectTransform.localPosition.z);
            icon.GetComponent<ModifierIcon>().modifier = modifier;
            icon.SetActive(true);
            i++;
        }
    }

    private void RemoveOldIcons()
    {
        ModifierIcon[] modifierIcons = GetComponentsInChildren<ModifierIcon>(true);
        foreach(ModifierIcon modifierIcon in modifierIcons)
        {
          
            if(!modifierIcon.gameObject.Equals(this.modifierIconPrefab))
            {
                Destroy(modifierIcon.gameObject);
            }
        }
    }
}
