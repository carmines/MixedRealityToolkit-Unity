// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microsoft.MixedReality.Toolkit.Input
{
    /// <summary>
    /// Unity specific data structure for poses
    /// </summary>
    public interface IMixedRealityPose
    {
        bool IsValid { get; }

        System.DateTime Timestamp { get; }

        UnityEngine.Vector3 Origin { get; }

        UnityEngine.Vector3 Direction { get; }

        UnityEngine.Ray Ray { get; }
    }
}
