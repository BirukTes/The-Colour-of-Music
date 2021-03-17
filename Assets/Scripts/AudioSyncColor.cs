using System.Collections;
using UnityEngine;
using Light2DE = UnityEngine.Experimental.Rendering.Universal.Light2D;

public class AudioSyncColor : MonoBehaviour
{
    //Make sure your GameObject has an AudioSource component first
    AudioSource audioSource;
    public Color[] beatColors;
    public Color restColor;
    public Light2DE pointLight2D;
    public float bias;
    public float timeStep;
    public float restSmoothTime;

    private float m_previousAudioValue;
    private float m_audioValue;
    private float timer;

    void Start()
    {
        // Fetch the AudioSource from the GameObject this script is attached to
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            // if (m_isBeat) return;
            Color _c = beatColors[1];
            if (AudioProcessor.currentEmotionValue == "happy")
            {
                _c = beatColors[0];
            }
            StopCoroutine("MoveToColor");
            StartCoroutine("MoveToColor", _c);

            // pointLight2D.color = Color.Lerp(pointLight2D.color, restColor, restSmoothTime * Time.deltaTime);
        }
    }
    private IEnumerator MoveToColor(Color targetColour)
    {
        Color currentColour = pointLight2D.color;
        Color initialColour = currentColour;
        float timer = 10;

        while (currentColour != targetColour)
        {
            currentColour = Color.Lerp(initialColour, targetColour, timer / Time.deltaTime);
            timer += Time.deltaTime;

            pointLight2D.color = currentColour;

            yield return null;
        }
    }
}
