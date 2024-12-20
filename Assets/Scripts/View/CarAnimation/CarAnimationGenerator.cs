public static class CarAnimationGenerator {
    private enum AnimationType {
        None = -1,
        Starting,
        Stoping,
        Rotating,
        Accelerating
    }
    public static ICarAnimation Generate(CarView view, CarVariables from, CarVariables to) {
        AnimationType type = GetType(from, to);

        return type switch
        {
            _ => null,
        };
    }

    private static AnimationType GetType(CarVariables from, CarVariables to) {


        return AnimationType.None;
    }
}