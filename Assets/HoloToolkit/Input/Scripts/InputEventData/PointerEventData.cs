﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.EventSystems;

namespace HoloToolkit.Unity.InputModule
{
    /// <summary>
    /// Describes an input event that involves a tap.
    /// </summary>
    public class PointerEventData : InputEventData
    {
        /// <summary>
        /// Number of Clicks or Taps that triggered the event.
        /// </summary>
        public int ClickCount { get; private set; }

        public PointerEventData(EventSystem eventSystem) : base(eventSystem) { }

        public void Initialize(IInputSource inputSource, uint sourceId, int clickCount, object[] tags = null)
        {
            Initialize(inputSource, sourceId, tags);
            ClickCount = clickCount;
        }

        public void Initialize(IInputSource inputSource, uint sourceId, int clickCount, Handedness handedness, object[] tags = null)
        {
            Initialize(inputSource, sourceId, handedness, tags);
            ClickCount = clickCount;
        }
    }
}