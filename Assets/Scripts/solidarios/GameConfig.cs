namespace GameConfig
{
    public enum Difficulty : byte
    {
        Easy = 0,
        Medium = 1,
        Hard = 2
    }

    public enum TextColorMode : byte
    {
        Wrong = 0,
        Correct = 1,
        Given = 2
    }

    public enum Orientation : byte
    {
        Horizontal = 0,
        Vertical = 1
    }

    public enum UnitOf : byte
    {
        distance = 0,
        time = 1,
        velocity = 2,
        acceleration = 3,
        angle = 4,
        angularVelocity = 5,
        force = 6,
        mass = 7,
        work = 8, //J
        energy = 9,//kW
        power = 10,//kWh
        momuntum = 11//kgm/s
    }
}