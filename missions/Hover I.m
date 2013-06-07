Mission
{
    name = Hover I
    description = Build a vessel that is able to hover for 10 seconds between 100m and 150m above Kerbins surface.
    reward = 5000

    repeatable = true

    OrbitGoal
    {
        minAltitude = 100
        maxAltitude = 150
        minSeconds = TIME(10s)

        maxSpeedOverGround = 5

        throttleDown = false
    }
}
