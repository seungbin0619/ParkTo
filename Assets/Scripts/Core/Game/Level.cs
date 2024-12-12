using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string Name { get; }
    public string Description { get; }
    public int Number { get; }

    public List<Car> Cars { get; }
    public Grid Grid;

    public Level(string name, string description, int number, List<Car> cars, Grid grid) {
        Name = name;
        Description = description;
        Number = number;
        Cars = cars;
        Grid = grid;
    }

    public static Level Dummy() {
        Level dummy = new(
            "dummy", 
            "dummy level",
            0,
            new List<Car>() {
                
            },
            new Grid(new Vector2Int(3, 3))
        );
        return dummy;
    }
}
