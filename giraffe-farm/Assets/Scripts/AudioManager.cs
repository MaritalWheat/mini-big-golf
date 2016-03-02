using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
	
	public static AudioManager Instance;
	private List<SoundFile> m_currentlyPlayingSounds;
	private SoundFile m_currentMusic;
	
	private float m_masterVolume; //this changes listener volume 
	private Dictionary<SoundFile.Type, float> m_layerVolumes; //specific volumes for sound types
	
	void Awake()
	{
		if (Instance == null) {
			Instance = this;
		}
		
		m_currentlyPlayingSounds = new List<SoundFile>();
		m_masterVolume = AudioListener.volume;
		m_layerVolumes = new Dictionary<SoundFile.Type, float>();
		foreach (SoundFile.Type type in System.Enum.GetValues(typeof(SoundFile.Type))) {
			m_layerVolumes.Add(type, 1);
		}
		m_currentMusic = new SoundFile { m_source = gameObject.AddComponent<AudioSource>(), m_paused = false, m_volume = 1, m_type = SoundFile.Type.Music };
		m_currentMusic.m_source.loop = true;
	}
	
	void Start () {
		if (transform.parent != Camera.main.transform) {
			transform.parent = Camera.main.transform;
		}
		Vector3 newPos = Camera.main.transform.position;
		newPos.z += 5f;
		transform.position = newPos;
	}
	
	public static void SetMusic(AudioClip music)
	{
		AudioManager.Instance.m_currentMusic.m_source.clip = music;
	}
	
	public static void ChangeMusic(AudioClip music)
	{
		AudioManager.Instance.m_currentMusic.m_source.clip = music;
		AudioManager.PlayMusic();
	}
	
	public static AudioClip GetMusic()
	{
		return AudioManager.Instance.m_currentMusic.m_source.clip;
	}
	
	public static void PlayMusic()
	{
		AudioManager.Instance.m_currentMusic.m_source.Play();
	}
	
	public static void PauseMusic(bool pause)
	{
		if (pause) {
			AudioManager.Instance.m_currentMusic.m_source.Pause();
			AudioManager.Instance.m_currentMusic.m_paused = true;
		} else {
			AudioManager.PlayMusic();
			AudioManager.Instance.m_currentMusic.m_paused = false;
		}
	}
	
	public static bool IsMusicPaused()
	{
		return AudioManager.Instance.m_currentMusic.m_paused;
	}
	
	public static void PlaySoundAtObject(GameObject sourceObject, AudioClip clip)
	{
		AudioSource source = sourceObject.gameObject.AddComponent<AudioSource>();
		source.clip = clip;
		source.Play();
		Destroy(source, clip.length);
		
		Instance.m_currentlyPlayingSounds.Add(new SoundFile { m_source = source, m_volume = source.volume, m_paused = false, m_type = SoundFile.Type.World });
	}
	
	public static void PlaySoundAtObject(GameObject sourceObject, AudioClip clip, float volume)
	{
		if (volume > 1) volume = 1;
		if (volume < 0) volume = 0;
		AudioSource source = sourceObject.gameObject.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.Play();
		Destroy(source, clip.length);
		
		Instance.m_currentlyPlayingSounds.Add(new SoundFile { m_source = source, m_volume = source.volume, m_paused = false, m_type = SoundFile.Type.World });
	}
	
	public static void PlaySoundAtListener(AudioClip clip)
	{
		AudioSource source = Instance.gameObject.AddComponent<AudioSource>();
		source.clip = clip;
		source.Play();
		Destroy(source, clip.length);
		
		Instance.m_currentlyPlayingSounds.Add(new SoundFile{m_source = source, m_volume = source.volume, m_paused = false, m_type = SoundFile.Type.Default});
	}
	
	public static void PlaySoundAtListener(AudioClip clip, float volume)
	{
		if (volume > 1) volume = 1;
		if (volume < 0) volume = 0;
		AudioSource source = Instance.gameObject.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.Play();
		Destroy(source, clip.length);
		
		Instance.m_currentlyPlayingSounds.Add(new SoundFile { m_source = source, m_volume = source.volume, m_paused = false, m_type = SoundFile.Type.Default });
	}
	
	public static void PlaySoundAtListener(AudioClip clip, SoundFile.Type type)
	{
		AudioSource source = Instance.gameObject.AddComponent<AudioSource>();
		source.clip = clip;
		float layerVolume;
		Instance.m_layerVolumes.TryGetValue(type, out layerVolume);
		source.volume = layerVolume;
		source.Play();
		Destroy(source, clip.length);
		
		Instance.m_currentlyPlayingSounds.Add(new SoundFile { m_source = source, m_volume = source.volume, m_paused = false, m_type = type });
	}
	
	public static void SetLayerVolume(SoundFile.Type layer, float volume)
	{
		if (volume > 1) volume = 1;
		if (volume < 0) volume = 0;
		Instance.m_layerVolumes[layer] = volume;
		
		if (layer != SoundFile.Type.Music) {
			// change volume for all currently existing sounds in the layer, as well as clean up audio list
			for (int i = 0; i < Instance.m_currentlyPlayingSounds.Count; i++) {
				SoundFile sound = Instance.m_currentlyPlayingSounds[i];
				if (sound.m_source == null) {
					Instance.m_currentlyPlayingSounds.Remove(sound);
				} else if (sound.m_type == layer) {
					sound.m_volume = Instance.m_layerVolumes[layer];
					sound.m_source.volume = sound.m_volume;
				}
			}
		} else {
			AudioManager.Instance.m_currentMusic.m_volume = Instance.m_layerVolumes[layer];
			AudioManager.Instance.m_currentMusic.m_source.volume = AudioManager.Instance.m_currentMusic.m_volume;
		}
	}
	
	public static float GetLayerVolume(SoundFile.Type layer)
	{
		float layerVolume;
		Instance.m_layerVolumes.TryGetValue(layer, out layerVolume);
		return layerVolume;
	}
	
	public static void SetMasterVolume(float volume)
	{
		if (volume > 1) volume = 1;
		if (volume < 0) volume = 0;
		Instance.m_masterVolume = volume;
		AudioListener.volume = Instance.m_masterVolume;
	}
	
	public static float GetMasterVolume()
	{
		return AudioManager.Instance.m_masterVolume;
	}
	
	public static List<SoundFile> GetActiveSoundFiles()
	{
		return AudioManager.Instance.m_currentlyPlayingSounds;
	}
	
}

public class SoundFile {
	public enum Type {
		Default,
		GUI,
		World,
		VoiceOver,
		Music
	}
	public AudioSource m_source { get; set; }
	public float m_volume { get; set; }
	public bool m_paused { get; set; }
	public Type m_type { get; set; }
}