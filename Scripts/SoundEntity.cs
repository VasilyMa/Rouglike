using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
public class SoundEntity : SourceEntity
{
    public static SoundEntity Instance { get; private set; }
    private EventReference _ambients;
    private EventInstance _ambientInstance;
    private EventReference _placeHolderSound;
    private SoundConfig _soundSettings;
    readonly string masterBusString = "Bus:/";
    private Bus masterBus;
    private Bus effectsBus;
    private Bus uiBus;
    private Bus ambientBus;
    private EventReference _currentAmbient;
    public SoundEntity()
    {
        Instance = this;

        masterBus = FMODUnity.RuntimeManager.GetBus(masterBusString);
        effectsBus = RuntimeManager.GetBus("bus:/Effects");
        uiBus = RuntimeManager.GetBus("bus:/UI");
        ambientBus = RuntimeManager.GetBus("bus:/Ambient");
    }


    public override SourceEntity Init() // only call when configs exist
    {
        _soundSettings = ConfigModule.GetConfig<SoundConfig>();
        /*_ambientInstance = RuntimeManager.CreateInstance(_ambients);
        _ambientInstance.start();*/

        return this;
    }

    public EventReference GetPlaceHolderSound()
    {
        return _placeHolderSound;
    }
    public SoundConfig GetSoundConfig()
    {
        return _soundSettings;
    }
    public void SetMasterVolume(float volume)
    {
        masterBus.setVolume(volume);
    }
    public float GetMasterVolume()
    {
        masterBus.getVolume(out float volume);
        return volume;
    }
    public void SetAmbientVolume(float volume)
    {
        ambientBus.setVolume(volume);
        _ambientInstance.setVolume(volume);
    }
    public float GetAmbientVolume()
    {
        ambientBus.getVolume(out float volume);
        return volume;
    }
    public void SetEffectsVolume(float volume)
    {
        effectsBus.setVolume(volume);
    }
    public float GetEffectsVolume()
    {
        effectsBus.getVolume(out float volume);
        return volume;
    }
    public void SetUIVolume(float volume)
    {
        uiBus.setVolume(volume);
    }
    public float GetUIVolume()
    {
        uiBus.getVolume(out float volume);
        return volume;
    }
    public void PlayAudioAtPosition(EventReference eventReference, Vector3 position)
    {
        RuntimeManager.PlayOneShot(eventReference, position);
    }

    public void PlayAudioAttached(EventReference eventReference, Transform attachTo)
    {
        RuntimeManager.PlayOneShotAttached(eventReference, attachTo.gameObject);
    }
    public EventInstance PlayAmbientAttached(EventReference eventReference)
    {
        if (IsSameAmbientPlaying(eventReference))
        {
            return _ambientInstance;
        }
        else
        {
            if (_ambientInstance.isValid())
            {
                StopAmbient();
            }
            _ambientInstance = RuntimeManager.CreateInstance(eventReference);
            _ambientInstance.start();
            return _ambientInstance;
        }
    }
    public void StopAmbient()
    {
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/Ambient"); // ������-���
        _ambientInstance.release();
        _ambientInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _ambientInstance.clearHandle();
        bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    private bool IsSameAmbientPlaying(EventReference reference)
    {
        if (!_ambientInstance.isValid()) { return false; }
        _ambientInstance.getDescription(out EventDescription description);
        description.getID(out GUID id);
        _ambientInstance.getPlaybackState(out PLAYBACK_STATE state);
        if (state != PLAYBACK_STATE.STOPPED && reference.Guid == id)
            return true;
        else return false;
    }
}
