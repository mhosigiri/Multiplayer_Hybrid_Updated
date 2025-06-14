using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FlashingHighlight : MonoBehaviour
{

    [SerializeField] private GameObject ObjectToFlash;
    [SerializeField] private float FlashSpeed = 0.6f;
    [SerializeField] private Material m_StartMaterial;

    private Renderer ObjRenderer;
    private Material m_FlashingMaterial;
    private Material m_HighlightMaterial;
    private Material[] m_ResetMaterials;

    struct ShaderPropertyLookup
    {
        public static readonly int surface = Shader.PropertyToID("_Surface");
        public static readonly int mode = Shader.PropertyToID("_Mode");
        public static readonly int srcBlend = Shader.PropertyToID("_SrcBlend");
        public static readonly int dstBlend = Shader.PropertyToID("_DstBlend");
        public static readonly int zWrite = Shader.PropertyToID("_ZWrite");
        public static readonly int baseColor = Shader.PropertyToID("_BaseColor");
        public static readonly int color = Shader.PropertyToID("_Color"); // Legacy
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        // Create yellow lit material
        var shaderName = GraphicsSettings.currentRenderPipeline ? "Universal Render Pipeline/Lit" : "Standard";
        var defaultShader = Shader.Find(shaderName);

        if (defaultShader == null)
        {
            Debug.LogWarning("Failed to create default materials for Flashing Highlight," +
                $" was unable to find \"{shaderName}\" Shader. Make sure the shader is included into the game build.", this);
            Destroy(this);
        }

        if (m_HighlightMaterial == null)
        {
            m_HighlightMaterial = new Material(defaultShader);

            var isSRP = GraphicsSettings.currentRenderPipeline != null;
            //m_NameID = isSRP ? ShaderPropertyLookup.baseColor : ShaderPropertyLookup.color;
            m_HighlightMaterial.SetColor(isSRP ?
                ShaderPropertyLookup.baseColor : ShaderPropertyLookup.color, new Color(1f, 1f, 0f, 1f));
        }

        // Initialize private renderer and material variables
        ObjRenderer = ObjectToFlash.GetComponentInChildren<Renderer>();

        if (m_StartMaterial == null)
        {
            m_StartMaterial = new Material(defaultShader);
            m_StartMaterial.color = ObjRenderer.material.color;
        }

        m_ResetMaterials = ObjRenderer.materials;
        m_FlashingMaterial = new Material(defaultShader);

        //ObjRenderer.material = m_FlashingMaterial;

        // Set all materials on object to the flashing material
        Material[] materials = new Material[ObjRenderer.materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials.SetValue(m_FlashingMaterial, i);
        }
        ObjRenderer.materials = materials;
    }

    void Update()
    {
        // ping-pong between the materials over the duration
        float lerp = (Mathf.PingPong(Time.time, FlashSpeed) / FlashSpeed);
        m_FlashingMaterial.Lerp(m_StartMaterial, m_HighlightMaterial, lerp);
    }

    private void OnDisable()
    {
        ObjRenderer.materials = m_ResetMaterials;
    }
}
