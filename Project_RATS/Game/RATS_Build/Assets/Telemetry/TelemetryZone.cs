using UnityEngine;

public class TelemetryZone : MonoBehaviour
{
    public string sectionName;

    private void OnTriggerEnter(Collider other) {
        TelemetryLogger.ChangeSection(this, sectionName);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        TelemetryLogger.ChangeSection(this, sectionName);
    }
}
