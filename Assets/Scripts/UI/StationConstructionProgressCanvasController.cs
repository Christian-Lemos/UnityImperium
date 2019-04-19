using UnityEngine;

[RequireComponent(typeof(StationController))]
public class StationConstructionProgressCanvasController : MonoBehaviour
{
    public Vector3 offset;

    public GameObject progressCanvasPrefab;

    public float scale;

    private StationConstructionProgressCanvas stationConstructionProgressCanvas;

    private StationController stationController;

    private void Start()
    {
        stationController = GetComponent<StationController>();
        if (!stationController.Constructed)
        {
            stationConstructionProgressCanvas = Instantiate(progressCanvasPrefab, transform.position, progressCanvasPrefab.transform.rotation, transform)
                .GetComponent<StationConstructionProgressCanvas>();

            Vector3 localScale = stationConstructionProgressCanvas.gameObject.transform.localScale;

            stationConstructionProgressCanvas.gameObject.transform.localScale = localScale * scale;

            stationConstructionProgressCanvas.gameObject.transform.localPosition = offset;
        }
    }

    private void Update()
    {
        if (stationConstructionProgressCanvas != null)
        {
            stationConstructionProgressCanvas.gameObject.SetActive(false);
            stationConstructionProgressCanvas.gameObject.SetActive(MapObjecsRenderingController.Instance.visibleObjects.Contains(gameObject));
        }

        if (stationController.Constructed && stationConstructionProgressCanvas != null)
        {
            stationConstructionProgressCanvas.gameObject.SetActive(false);
            enabled = false;
        }
        else
        {
            stationConstructionProgressCanvas.progressSlider.fillAmount = stationController.constructionProgress / 100;
        }
    }
}