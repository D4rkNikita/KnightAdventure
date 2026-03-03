using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshSurfaceManagment : MonoBehaviour
{
    public static NavMeshSurfaceManagment Instance { get; private set; }

    private NavMeshSurface _navMeshSurface;

    private void Awake()
    {
        Instance = this;
        _navMeshSurface = GetComponent<NavMeshSurface>();
        _navMeshSurface.hideEditorLogs = true;
    }

    public void RebakeNavmeshSurface()
    {
        _navMeshSurface.BuildNavMesh();
    }
}
