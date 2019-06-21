using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Input
{
    public class SimulatedEyePose : IMixedRealityPose
    {
        public bool IsValid
        {
            get
            {
                var delta = DateTime.Now.ToUniversalTime().Subtract(Timestamp).TotalMilliseconds;
                if (delta > 250.0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public DateTime Timestamp { get; private set; } = DateTime.Now.ToUniversalTime();

        public Vector3 Origin => Ray.origin;

        public Vector3 Direction => Ray.direction;

        public Ray Ray { get; private set; }

        public SimulatedEyePose(DateTime timeStamp, Ray ray)
        {
            Timestamp = timeStamp;
            Ray = ray;
        }
    }
}
