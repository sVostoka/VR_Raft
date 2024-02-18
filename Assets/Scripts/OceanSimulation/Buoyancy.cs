using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using System.Linq;
using System.Collections;

public class Buoyancy : MonoBehaviour
{
    public Material ocean;
    public RenderTexture oceanTexture;

    private Texture2D oceanTexture2D;
    public int textureScanWidth = 2;

    private AsyncGPUReadbackRequest request;
    private NativeArray<byte> oceanPixelsPersist;

    private Vector3 targetNormal = Vector3.up;
    private float avgHeight = 0;
    private int asyncGPUCount = 0;
    private int asyncGPUCountDistinct = 0;
    private float NormalThreshold = 0.001f;
    private float NormalLerpSpeed = 1;

    public float NormalIntensity = 0.5f;
    public float initialHeight;
    public float heightStr = 1;

    public float yVelocity = 0.0f;

    Water water;
    private Mesh _mesh;
    
    private void OnDestroy()
    {
        //oceanPixelsPersist.Dispose();
    }

    private void GetOceanPixels()
    {
        if (request.done)
        {
            if (!request.hasError)
            {
                Vector3[] points = new Vector3[]
                {
                    new Vector3(0, oceanPixelsPersist[textureScanWidth * (textureScanWidth - 1)] * NormalIntensity, textureScanWidth / 2),
                    new Vector3(0, oceanPixelsPersist[0] * NormalIntensity, 0),
                    new Vector3(textureScanWidth / 2, oceanPixelsPersist[textureScanWidth - 1] * NormalIntensity, 0),
                    new Vector3(textureScanWidth / 2, oceanPixelsPersist[(textureScanWidth * textureScanWidth - 1)] * NormalIntensity, textureScanWidth / 2)
                };
                targetNormal = GetNormalFromPoints(points);
                avgHeight = points.Select(p => Mathf.Pow(((float)p.y / NormalIntensity) / 255f, 3.76f)).Average();
                asyncGPUCount++;
                asyncGPUCountDistinct++;
            }
            var offset = textureScanWidth / 2 - 1;
            request = AsyncGPUReadback.RequestIntoNativeArray(ref oceanPixelsPersist,
                oceanTexture,
                mipIndex: 0,
                x: oceanTexture.width / 2 - offset,
                width: textureScanWidth,
                y: oceanTexture.height / 2 - offset,
                height: textureScanWidth,
                z: 0,
                depth: 1,
                TextureFormat.R8);
        }
    }

    private Vector3 GetNormalFromPoints(Vector3[] points)
    {
        var v1 = Vector3.Cross(points[2] - points[0], points[1] - points[0]);
        var v2 = Vector3.Cross(points[0] - points[2], points[3] - points[2]);
        return (v1 + v2).normalized;
    }

    private void Start()
    {
        oceanTexture2D = new Texture2D(textureScanWidth, textureScanWidth);
        oceanPixelsPersist = new NativeArray<byte>(textureScanWidth * textureScanWidth, Unity.Collections.Allocator.Persistent);
        initialHeight = transform.localPosition.y;
    }

    private void Update()
    {
        GetOceanPixels();

        var currNormal = transform.TransformDirection(Vector3.up);
        if (Vector3.Distance(currNormal, targetNormal) > NormalThreshold)
        {
            var fromLook = Quaternion.LookRotation(transform.forward - Vector3.Project(transform.forward, currNormal), currNormal);
            var toLook = Quaternion.LookRotation(transform.forward - Vector3.Project(transform.forward, targetNormal), targetNormal);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.RotateTowards(fromLook, toLook, 90f), Time.deltaTime * NormalLerpSpeed);
        }

        transform.localPosition = new Vector3(transform.localPosition.x, initialHeight + avgHeight * heightStr, transform.localPosition.z);
    }
}
