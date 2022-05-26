namespace Rocket.Utils
{
    public static class SpeedExtensions
    {
        public static float GetForwardAcceleration(this float speed, CarModelData data)
        {
            float throttle = 0;

            if (speed > data.MaxSpeed)
            {
                throttle = 0;
            }
            else if (speed > data.IntermediateSpeed)
            {
                throttle = RoboUtils.Scale(14f, 14.1f, 1.6f, 0, speed);
            }
            else
            {
                throttle = RoboUtils.Scale(0, 14, 16, 1.6f, speed);
            }

            return throttle;
        }


        public static float GetTurnRadius(this float forwardSpeed)
        {
            float turnRadius = 0;

            var curvature = RoboUtils.Scale(0, 5, 0.0069f, 0.00398f, forwardSpeed);

            if (forwardSpeed >= 500 / 100)
                curvature = RoboUtils.Scale(5, 10, 0.00398f, 0.00235f, forwardSpeed);

            if (forwardSpeed >= 1000 / 100)
                curvature = RoboUtils.Scale(10, 15, 0.00235f, 0.001375f, forwardSpeed);

            if (forwardSpeed >= 1500 / 100)
                curvature = RoboUtils.Scale(15, 17.5f, 0.001375f, 0.0011f, forwardSpeed);

            if (forwardSpeed >= 1750 / 100)
                curvature = RoboUtils.Scale(17.5f, 23, 0.0011f, 0.00088f, forwardSpeed);

            turnRadius = 1 / (curvature * 100);
            return turnRadius;
        }
    }

}
