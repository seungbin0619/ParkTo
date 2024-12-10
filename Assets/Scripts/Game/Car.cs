using UnityEngine;
public partial class Car : PhysicsObject
{
    
}

public partial class Car : IAssignable<Trigger>
{
    public void Assign(Trigger t)
    {
        
    }

    public bool IsAssignable()
    {
        return true;
    }
}