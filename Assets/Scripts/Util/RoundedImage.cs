using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode, RequireComponent(typeof(Image))]
public class RoundedImage : MonoBehaviour
{
	private static readonly int Props = Shader.PropertyToID("_WidthHeightRadius");

	public float radius;

	private Image _image;
	private bool _isSetup;

	void OnRectTransformDimensionsChange()
	{
		Refresh();
	}
	
	private void OnValidate()
	{
		Refresh();
	}
		
	private void SetupMaterial()
	{
		if (_isSetup)
		{
			return;
		}

		_image = GetComponent<Image>();
		if (_image.material != null)
		{
			_image.material = Instantiate(_image.material);
		}

		_isSetup = true;	
	}

	private void Refresh()
	{
		SetupMaterial();
			
		var rect = ((RectTransform) transform).rect;
		_image.material.SetVector(Props, new Vector4(rect.width, rect.height, radius, 0));
	}
}