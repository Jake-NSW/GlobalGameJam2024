using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Rendering.Universal;

public class CreateUrpMaterialContextMenu
{
    [MenuItem("Assets/Create URP Material")]
    private static void CreateUrpMaterial()
    {
        // Get the selected textures
        var selectedTextures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);
        
        // regex search for the words 'diffuse', 'albedo', 'color', 'colour', '_c', '_d' in selected textures
        // if found, assign to _diffuseTexture
        var diffuseRegex = new Regex("diffuse|albedo|color|colour|_c|_d|rgb", RegexOptions.IgnoreCase);
        var diffuseTexture = selectedTextures.FirstOrDefault(texture => diffuseRegex.IsMatch(texture.name));
        
        var normalRegex = new Regex("normal|_n|norm|normals", RegexOptions.IgnoreCase);
        var normalTexture = selectedTextures.FirstOrDefault(texture => normalRegex.IsMatch(texture.name));
        
        var metallicRegex = new Regex("metallic|_m|metalness", RegexOptions.IgnoreCase);
        var metallicTexture = selectedTextures.FirstOrDefault(texture => metallicRegex.IsMatch(texture.name));
        
        var roughnessRegex = new Regex("roughness|_r|rough", RegexOptions.IgnoreCase);
        var roughnessTexture = selectedTextures.FirstOrDefault(texture => roughnessRegex.IsMatch(texture.name));


        // Create a new URP material
        Material newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        // Assign the selected textures to the material
        if(diffuseTexture != null)
        {
            newMaterial.mainTexture = diffuseTexture;
        }
        if(normalTexture != null)
        {
            newMaterial.SetTexture("_BumpMap", normalTexture);
        }
                
        
        var texturePath = AssetDatabase.GetAssetPath(selectedTextures[0]);
        var textureDir = System.IO.Path.GetDirectoryName(texturePath);
        var textureName = FindCommonName();
        
        // check dir exsits
        if (!System.IO.Directory.Exists(textureDir))
        {
            Debug.LogError("Texture directory does not exist");
        }
        
        if(metallicTexture != null && roughnessTexture != null)
        {
            Texture2D combinedTexture = CombineTextures(metallicTexture, roughnessTexture);
            var newTextureName = textureName + "_mr";
            var texturePathCombined = System.IO.Path.Combine(textureDir, newTextureName + ".png");
            // save the asset
            System.IO.File.WriteAllBytes(texturePathCombined, combinedTexture.EncodeToPNG());
            AssetDatabase.ImportAsset(texturePathCombined, ImportAssetOptions.ForceUpdate);
            newMaterial.SetTexture("_MetallicGlossMap", AssetDatabase.LoadAssetAtPath<Texture2D>(texturePathCombined));
        }
        
        var materialPath = System.IO.Path.Combine(textureDir, textureName + ".mat");
        AssetDatabase.CreateAsset(newMaterial, materialPath);
    }
    
    private static string FindCommonName()
    {
        // Get the selected textures
        var selectedTextures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);

        // Split the first texture's name by underscore
        var firstTextureNameParts = selectedTextures[0].name.Split('_');

        // Assume the common phrase is the first part of the first texture's name
        var commonPhrase = firstTextureNameParts[0];

        // Check if all textures have the same common phrase
        foreach (var texture in selectedTextures)
        {
            var textureNameParts = texture.name.Split('_');
            if (textureNameParts[0] == commonPhrase) continue;
            // If a texture does not have the same common phrase, set commonPhrase to null and break the loop
            commonPhrase = null;
            break;
        }

        return commonPhrase ?? "NewMaterial";
    }

    private static Texture2D CombineTextures(Texture2D metallicTexture, Texture2D roughnessTexture)
    {
        // Get the TextureImporter for the metallic texture
        TextureImporter metallicTextureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(metallicTexture)) as TextureImporter;
        // Get the TextureImporter for the roughness texture
        TextureImporter roughnessTextureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(roughnessTexture)) as TextureImporter;

        // Make the textures temporarily readable
        bool metallicWasReadable = metallicTextureImporter.isReadable;
        bool roughnessWasReadable = roughnessTextureImporter.isReadable;
        metallicTextureImporter.isReadable = true;
        roughnessTextureImporter.isReadable = true;
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(metallicTexture), ImportAssetOptions.ForceUpdate);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(roughnessTexture), ImportAssetOptions.ForceUpdate);
        
        // Create a new texture that is the same size as the metallic texture
        Texture2D combinedTexture = new Texture2D(metallicTexture.width, metallicTexture.height);

        // Loop over each pixel in the metallic texture
        for (int y = 0; y < metallicTexture.height; y++)
        {
            for (int x = 0; x < metallicTexture.width; x++)
            {
                // Get the color of the current pixel in the metallic texture
                var metallicColor = metallicTexture.GetPixel(x, y);

                // Get the color of the current pixel in the roughness texture
                var roughnessColor = roughnessTexture.GetPixel(x, y);

                // Combine the metallic and roughness colors into one color
                var combinedColor = new Color(metallicColor.r, metallicColor.g, metallicColor.b, roughnessColor.r);

                // Set the color of the current pixel in the combined texture to the combined color
                combinedTexture.SetPixel(x, y, combinedColor);
            }
        }

        // Apply the changes to the combined texture
        combinedTexture.Apply();
        
        // Set the textures back to their original readable state
        metallicTextureImporter.isReadable = metallicWasReadable;
        roughnessTextureImporter.isReadable = roughnessWasReadable;
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(metallicTexture), ImportAssetOptions.ForceUpdate);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(roughnessTexture), ImportAssetOptions.ForceUpdate);

        return combinedTexture;
    }
}