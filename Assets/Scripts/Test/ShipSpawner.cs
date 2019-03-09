using Imperium;
using Imperium.MapObjects;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipSpawner : MonoBehaviour
{
    public GameObject buttonGO;

    public GameObject inputJogadorGO;

    private readonly int mapLayer = 1 << (int)ObjectLayers.Map;

    private Button button;

    private InputField inputJogador;

    private Vector3 lastClickedPosition = new Vector3(0, 0, 0);

    [SerializeField]
    private ShipType shipType;

    private Spawner spawner;

    private void SpawnShip()
    {
        int player;
        if (Int32.TryParse(inputJogador.text, out player))
        {
            spawner.SpawnShip(shipType, player, lastClickedPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Failed to convert " + inputJogador.text + " to integer");
        }
    }

    private void Start()
    {
        button = buttonGO.GetComponent<Button>();
        inputJogador = inputJogadorGO.GetComponent<InputField>();
        spawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<Spawner>();

        button.onClick.AddListener(SpawnShip);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, mapLayer))
            {
                lastClickedPosition = hit.point;
            }
        }
    }
}