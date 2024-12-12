using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string Name { get; }
    public string Description { get; }
    public int Number { get; }


    public List<Car> Cars { get; }
    public Grid Grid;

    
}
