#if TOOLS
using Godot;

[Tool]
public class Internals : EditorPlugin
{
    public override void _EnterTree()
    {
        var script = GD.Load<Script>("addons/Internals/Scene.cs");
        var texture = GD.Load<Texture>("addons/Internals/SceneNode.png");
        AddCustomType("Scene", "Node2D", script, texture);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("Scene");
    }
}
#endif