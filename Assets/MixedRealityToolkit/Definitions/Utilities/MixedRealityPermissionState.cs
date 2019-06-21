// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microsoft.MixedReality.Toolkit
{
    public enum MixedRealityPermissionState
    {
        Unspecified = 0, // The user has not specified whether the app can track their gaze.
        Allowed = 1, // The user has given permission for the app to to track their gaze.
        DeniedByUser = 2, // The user has denied permission for the app to track their gaze.
        DeniedBySystem = 3 // The system has denied permission for the app to track the user's gaze.
    }
}
