using System;
using System.Reflection;
using Godot;

public class Scene : Node2D
{
    [Export(PropertyHint.Range, "0,4000")]
    public int Gravity = 2000;
    [Export(PropertyHint.Range, "0,8000")]
    public int TerminalVelocity = 6000;

    public override void _Ready() {
        SetInherits(this);
    }

    private void SetInherits(Node curNode) {
        foreach (Node child in curNode.GetChildren()) {
            SetInherits(child);
            foreach (var field in child.GetType().GetFields()) {
                foreach (var attribute in field.GetCustomAttributes()) {
                    // Console.WriteLine(child.Name + " has " + attribute + " for " + field.Name);
                    //Useful for debugging.
                    
                    if (attribute is InheritAttribute) {
                        if(!(GetType().GetField(field.Name) is null)) {
                            field.SetValue(child, GetType().GetField(field.Name).GetValue(this));
                        }
                        else if (!(new Globals().GetType().GetField(field.Name) is null)) {
                            field.SetValue(child, GetType().GetField(field.Name).GetValue(new Globals()));
                        }
                        else {
                            throw new MissingFieldException(
                                "The scene does not contain the field \"" + field.Name +
                                "\" which was supposed to be inherited by \"" + child.Name + "\" for \"" +
                                field.Name + "\"");
                            //We need to throw an error in the case that, for whatever reason we attempt
                            // to inherit something which is not available to it...
                        }
                    }
                }
            }
        }
    }
}
