using UnityEngine;

public class ResourceMarerial : MonoBehaviour
{
    public Material material { get { return changeMaterial; } }
    [SerializeField] private Material changeMaterial;
}
