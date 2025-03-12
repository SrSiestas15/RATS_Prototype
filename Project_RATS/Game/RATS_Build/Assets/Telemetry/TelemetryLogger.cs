using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

using LoggingPolicy = TelemetrySettings.LoggingPolicy;

/// <summary>
/// Main interface to the Telemetry system.
/// Place this component on a GameObject in each scene that needs telemetry.
/// You can also label it with a "section" to identify where in the game
/// the event occurred (menu, level 1, etc.)
/// Be sure to create a TelemetrySettings asset first, and it will automatically find it.
/// </summary>
public partial class TelemetryLogger : MonoBehaviour
{
    /// <summary>
    /// Settings used by this telemetry logger. You can get by with just one Settings asset used by
    /// every logger in your whole project, unless you need something more complicated.
    /// </summary>
    [field:Tooltip("Go to Assets->Create->Telemetry->Settings to configure login/settings for your project.")]
    [field:SerializeField]
    public TelemetrySettings Settings { get; private set; }

    [Tooltip("Telemetry sent from this scene will use this label.")]
    [SerializeField] string _section;

    /// <summary>
    /// Short text label identifying where in your game the telemetry events are coming from (menu, level 1, etc.).
    /// </summary>
    public string Section
    {
        get { return _section; }
    }

    /// <summary>
    /// Report a change in game section without changing scenes - like when passing through
    /// a trigger volume into / out of a special zone.
    /// </summary>
    /// <param name="newSection">Short label for the new section you're moving into.</param>
    public void ChangeSection(string newSection)
    {
        _section = newSection;
        _service.ProcessSectionChange(this);
    }

    [Tooltip("When successfully connected to the telemetry server, this will be called with the session ID number. Use this if you want to display the session number on-screen.")]
    public UnityEvent<int> OnConnectionSuccess;

    [Tooltip("When connection to the telemetry service fails, this will be called with the failure message. Use this if you want to print the error on-screen.")]
    public UnityEvent<string> OnConnectionFail;


    // Network communication for this telemetry source to log into.
    // One service can be shared by multiple loggers (eg. one for menus, one for gameplay, one for AI...)
    Service _service;


    // Lookup table to cache the right logger to use for each loaded scene.
    static List<TelemetryLogger> _loggers = new();

    /// <summary>
    /// Try to get the session index, uniquely identifying this playtest run in the database.
    /// </summary>
    /// <param name="sessionIndex">Set to the session index if connected, or -1 if not.</param>
    /// <returns>True if the telemetry service has finished connecting, or false if still in progress / failed.</returns>
    public static bool TryGetSessionId(out int sessionIndex) {
        if (TryGetLogger(default, out var logger))
            return logger.TryGetSessionIndex(out sessionIndex);
        sessionIndex = -1;
        return false;
    }

    /// <summary>
    /// Shortcut to look up a logger in the same scene as the provided component.
    /// </summary>
    /// <param name="source">Component responsible for the event to log.</param>
    /// <returns>A telemetry logger, or null if none could be found.</returns>
    public static TelemetryLogger GetLogger(Component source) {
        TryGetLogger(source != null ? source.gameObject.scene : default, out var logger);
        return logger;
    }

    /// <summary>
    /// Searches for the best matching logger to log an event from the given scene.
    /// </summary>
    /// <param name="source">The scene where the event occurred. Passing default will grab a logger arbitrarily.</param>
    /// <param name="logger">The telemetry logger for the source scene, or an arbitrary fallback logger, or null if none could be found.</param>
    /// <returns>True if a logger could be found, false if there are no loggers available in the loaded scenes.</returns>
    public static bool TryGetLogger(Scene source, out TelemetryLogger logger) {       
        logger = null;
        var backupScene = SceneManager.GetActiveScene();

        string sourceName = source == null ? "null" : source.name;

        foreach(var entry in _loggers) {
            var scene = entry.gameObject.scene;

            if(scene == source) {
                logger = entry;
                break;
            }

            if (logger == null || scene == backupScene) {
                logger = entry;
            }
        }

        // If all our attempts have failed, report an error and abort.
        if (logger == null) {
            Debug.LogError($"Trying to log telemetry in scene '{sourceName}' where no TelemetryLogger is available. Be sure to place a TelemetryLogger in your scene.");
            return false;
        } else if (logger.gameObject.scene != source) {
            // If we found a logger, but it's mismatched, warn about it.
            if (logger.Settings.debugLogging <= LoggingPolicy.WarningsAndErrors)
                Debug.LogWarning($"Trying to log telemetry in scene '{sourceName}' with no loggers. Redirecting to active logger from '{logger.gameObject.scene.name}' instead. This might lead to misleading 'section' values reported in your telemetry.");
        }

        return true;
    }

    /// <summary>
    /// Change the section label used in a scene, auto-selecting the right logger for the scene this sender is in.
    /// </summary>
    /// <param name="sender">The script/component that fired the event, used to find the right logger. Can use null to select a default logger if you only have one.</param>
    /// <param name="newSection">Short label for the new section you're moving into.</param>
    public static void ChangeSection(Component sender, string newSection) {
        if (TryGetLogger(sender.gameObject.scene, out var logger))
            logger.ChangeSection(newSection);
    }

    /// <summary>
    /// Log a telemetry event, auto-selecting the right logger for the scene this sender is in.
    /// </summary>
    /// <typeparam name="T">Payload data type, inferred.</typeparam>
    /// <param name="sender">The script/component that fired the event, used to find the right logger. Can use null to select a default logger if you don't need to distinguish game sections.</param>
    /// <param name="eventType">Short label for the kind of event.</param>
    /// <param name="data">Optional payload data.</param>            
    public static void Log<T>(Component sender, string eventType, T data) {
        Scene source = sender != null ? sender.gameObject.scene : default;
        Log(source, eventType, data);
    }

    /// <summary>
    /// Log a telemetry event, auto-selecting the right logger for the scene the event happened in.
    /// </summary>
    /// <typeparam name="T">Payload data type, inferred.</typeparam>
    /// <param name="source">The scene the event occurred within, used to find the right logger. Can use default to select a default logger if you don't need to distinguish game sections.</param>
    /// <param name="eventType">Short label for the kind of event.</param>
    /// <param name="data">Optional payload data.</param>   
    public static void Log<T>(Scene source, string eventType, T data) {        
        // Ask the selected logger to log this event.
        if (TryGetLogger(source, out var logger))
            logger.Log(eventType, data);
    }

    /// <summary>
    /// Log an event from this game section, with a data payload.
    /// </summary>
    /// <typeparam name="T">Payload data type (inferred)</typeparam>
    /// <param name="eventType">Short text label for the event.</param>
    /// <param name="data">Serializable data to attach to the event.</param>
    public void Log<T>( string eventType, T data) {
        if (_service == null) Initialize();

        var telemetry = new TelemetryEvent<T>(eventType, data);
        telemetry.section = Section;
        _service.TryLog(this, telemetry);
    }

    /// <summary>
    /// Try to get the session index, uniquely identifying this playtest run in the database.
    /// </summary>
    /// <param name="sessionIndex">Set to the session index if connected, or -1 if not.</param>
    /// <returns>True if the telemetry service has finished connecting, or false if still in progress / failed.</returns>
    public bool TryGetSessionIndex(out int sessionIndex) {
        if (_service == null) Initialize();
        return _service.TryGetSessionIndex(out sessionIndex);
    }

    // Spin up / connect to the telemetry service, and add this logger to our cache so it's easy to find.
    void Initialize() {
        // Ensure there's at most one cached logger in the scene..
        foreach(var entry in _loggers) {
            if (entry.gameObject.scene == gameObject.scene) {
                Debug.LogWarning($"Two different TelemetryLoggers in scene '{gameObject.scene.name}'. This might lead to accidentally logging through the wrong one.");
                break;
            }
        }
        _loggers.Add(this);       

        // Fetch a telemetry service that matches our TelemetrySettings, spinning up a new one if needed.
        _service = Service.Connect(this, OnConnect, OnFail);
    }

    void OnConnect(int sessionId) {
        OnConnectionSuccess?.Invoke(sessionId);
    }


    void OnFail(string message)
    {
        OnConnectionFail?.Invoke(message);
    }

    void Awake() {
        // Cache ourselves in time for the first logging requests.
        if (_service == null) Initialize();
    }

    void OnDestroy() {
        // Erase this logger from the cache, if it was the cached logger for this scene.
        _loggers.Remove(this);

        // De-register event listeners.
        if (_service != null) {
            _service.OnConnectionSuccess -= OnConnect;
            _service.OnConnectionFail -= OnFail;
        }
    }
    
#if UNITY_EDITOR
    // These methods auto-populate the fields when a
    // new TelemetryLogger is added.
    void Reset() {        
        _section = gameObject.scene.name;
    }

    void OnValidate() {
        if (Settings == null)
            Settings = TelemetrySettings.GetDefault();
    }
#endif

#region Shortcuts
    // Used to send events with no payload data - a struct that contains no members.
    [System.Serializable]
    public struct Empty { }
    static readonly Empty NO_DATA = default;

    /// <summary>
    /// Shortcut to log an event from this game section, with no other data.
    /// </summary>
    /// <param name="sender">Reference to the object that generated the event - used to select the right logger/section to report it from.</param>
    /// <param name="eventType">Short text label for the event.</param>
    public static void Log(Component sender, string eventType)
    {
        Log(sender, eventType, NO_DATA);
    }

    /// <summary>
    /// Shortcut to log an event from this game section, with no other data.
    /// </summary>
    /// <param name="eventType">Short text label for the event.</param>
    public void Log(string eventType)
    {
        Log(eventType, NO_DATA);
    }
#endregion Shortcuts
}
