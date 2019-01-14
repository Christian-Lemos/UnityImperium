using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Imperium.Enum;
using Imperium.Economy;



public class UIResourcesUpdater : MonoBehaviour {


    private PlayerDatabase database;

    [SerializeField]
    private GameObject UIResourcePrefab;

    private Dictionary<ResourceType, Text> texts = new Dictionary<ResourceType, Text>();

    private int player;

    private void Start()
    {
        database = PlayerDatabase.INSTANCE;

        for(int j = 0; j < PlayerDatabase.INSTANCE.gameSceneData.players.Count; j++)
        {
            if(PlayerDatabase.INSTANCE.gameSceneData.players[j].playerType == PlayerType.Real)
            {
                player = PlayerDatabase.INSTANCE.gameSceneData.players[j].PlayerNumber;
            }
        }

        //player = PlayerDatabase.INSTANCE.gameSceneData.RealPlayer;

        float x_axis_offset = UIResourcePrefab.GetComponent<RectTransform>().localPosition.x;
        if(x_axis_offset > 0)
        {
            x_axis_offset *= -1;
        }
        int i = 0;
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            Resource resource = new Resource(resourceType);
            GameObject obj = Instantiate(UIResourcePrefab, this.transform);
            RectTransform rectTransform = obj.GetComponent<RectTransform>();

            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - x_axis_offset * i, rectTransform.localPosition.y, rectTransform.localPosition.z);

            obj.GetComponentInChildren<RawImage>().texture = resource.Icon;

            texts.Add(resource.Type, obj.GetComponentInChildren<Text>());

            i++;
        }
    }


    private void Update()
    {
        Dictionary<ResourceType, int> currentResources = PlayerDatabase.INSTANCE.GetPlayerResources(player);
        foreach (KeyValuePair<ResourceType, int> entry in currentResources)
        {
            this.texts[entry.Key].text = entry.Value.ToString();
        }
    }
}
