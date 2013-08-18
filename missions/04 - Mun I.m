Mission
{
    name = Mun I
    description = The government is satisfied with your missions thus far, but requires more sensor data on other astronomical objects nearby. Construct a rocket that is able to reach the Mun, maintain a stable orbit, and then return to the surface of Kerbin intact. Since this mission is of a general exploratory nature and to verify our theories on orbital transfers, only a basic set of sensors are required.

    reward = 80000
    category = ORBIT

    requiresMission = Sputnik III
    packageOrder = 4

    OrbitGoal
    {
        body = Mun

        minPeA = 10000
        maxApA = 900000
    }

    PartGoal
    {
        partName = longAntenna
        partCount = 3
    }

    LandingGoal
    {
        body = Kerbin
    }
}
