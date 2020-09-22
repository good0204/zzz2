using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
/// <summary>
/// Attach this script to an empty gameObject. Create a secondary camera 
/// gameObject for offscreen rendering (not your main camera) and connect it 
/// with this script. Offscreen camera should have a texture object attached to it.  
/// OffscreenCamera texture object is used for rendering (please see camera properties). 
/// </summary> 
public class OffscreenRendering : MonoBehaviour
{
    /*#region public members 	
    /// <summary> 	
    /// The desired number of screenshots per second. 	
    /// </summary> 	
    [Tooltip("Number of screenshots per second.")]
    [SerializeField] int ScreenshotsPerSecond = 60;
    /// <summary> 	
    /// Camera used to render screen to texture. Offscreen camera 	
    /// with desired target texture size should be attached here, 	
    /// not the main camera. 	
    /// </summary> 	
    [Tooltip("The camera that is used for off-screen rendering.")]
    [SerializeField] Camera OffscreenCamera;
    [SerializeField] List<Sprite> textures = new List<Sprite>();
    [SerializeField] Animation ani;
    #endregion
    /// <summary> 	
    /// Keep track of saved frames. 	
    /// counter is added as postifx to file names. 	
    ///</summary>
    private int FrameCounter = 0;

    // Use this for initialization 	
    void Start()
    {
        RenderingStart();
    }

    /// <summary>     
    /// Captures x frames per second.      
    /// </summary>     
    /// <returns>Enumerator object</returns>     
    public void RenderingStart()
    {
        StartCoroutine(CaptureAndSaveFrames());
    }
    IEnumerator CaptureAndSaveFrames()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            // Remember currently active render texture. 			
            RenderTexture currentRT = RenderTexture.active;
            // Set target texture as active render texture. 			
            RenderTexture.active = OffscreenCamera.targetTexture;
            // Render to texture 			
            OffscreenCamera.Render();
            // Read offscreen texture 			
            Texture2D offscreenTexture = new Texture2D(OffscreenCamera.targetTexture.width, OffscreenCamera.targetTexture.height, TextureFormat.RGB24, false);
            offscreenTexture.ReadPixels(new Rect(0, 0, OffscreenCamera.targetTexture.width, OffscreenCamera.targetTexture.height), 0, 0, false);
            offscreenTexture.Apply();
            // Reset previous render texture. 			
            RenderTexture.active = currentRT;
            ++FrameCounter;
            // Encode texture into PNG 			
            byte[] bytes = offscreenTexture.EncodeToPNG();
            // File.WriteAllBytes(Application.dataPath + "/../Capture/capturedframe" + FrameCounter.ToString() + ".png", bytes);
            // Delete textures. 			
            // UnityEngine.Object.Destroy(offscreenTexture);
            //textures.Add(offscreenTexture);
            textures.Add(Sprite.Create(offscreenTexture, new Rect(0, 0, offscreenTexture.width, offscreenTexture.height), new Vector2(0.5f, 0.5f)));
            yield return new WaitForSeconds(1.0f / ScreenshotsPerSecond);
        }
    }
    private void OnDisable()
    {
        StopCapturing();
        TestAni();
    }

    /// <summary>     
    /// Stop image capture.     
    /// </summary>     
    public List<Sprite> StopCapturing()
    {
        StopCoroutine("CaptureAndSaveFrames");
        FrameCounter = 0;
        return textures;
    }
    void TestAni()
    {
        ani.clip.frameRate = 25;
        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(Image);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[textures.Count];
        for (int i = 0; i < (textures.Count); i++)
        {
            spriteKeyFrames[i] = new ObjectReferenceKeyframe();
            spriteKeyFrames[i].time = i % 60;
            spriteKeyFrames[i].value = textures[i];
        }
        AnimationUtility.SetObjectReferenceCurve(ani.clip, spriteBinding, spriteKeyFrames);
        ani.Play();
    }

    /// <summary> 	
    /// Resume image capture. 	
    /// </summary> 	
    public void ResumeCapturing()
    {
        StartCoroutine("CaptureAndSaveFrames");
    }*/
}