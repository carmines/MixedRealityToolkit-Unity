﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Internal.EventDatum.Teleport;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces.InputSystem;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces.TeleportSystem;
using Microsoft.MixedReality.Toolkit.Internal.Managers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Microsoft.MixedReality.Toolkit.SDK.Teleportation
{
    /// <summary>
    /// The Mixed Reality Toolkit's specific implementation of the <see cref="IMixedRealityTeleportSystem"/>
    /// </summary>
    public class MixedRealityTeleportManager : MixedRealityEventManager, IMixedRealityTeleportSystem
    {
        private TeleportEventData teleportEventData;

        #region IMixedRealityManager Implementation

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();
            InitializeInternal();
        }

        private void InitializeInternal()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                var eventSystems = Object.FindObjectsOfType<EventSystem>();

                if (eventSystems.Length == 0)
                {
                    if (!MixedRealityManager.Instance.ActiveProfile.IsInputSystemEnabled)
                    {
                        new GameObject("Event System").AddComponent<EventSystem>();
                    }
                    else
                    {
                        Debug.Log("The Input System didn't properly add an event system to your scene. Please make sure the Input System's priority is set higher than the teleport system.");
                    }
                }
                else if (eventSystems.Length > 1)
                {
                    Debug.Log("Too many event systems in the scene. The Teleport System requires only one.");
                }
            }
#endif // UNITY_EDITOR

            teleportEventData = new TeleportEventData(EventSystem.current);
        }

        #endregion IMixedRealityManager Implementation

        #region IEventSystemManager Implementation

        /// <inheritdoc />
        public override void HandleEvent<T>(BaseEventData eventData, ExecuteEvents.EventFunction<T> eventHandler)
        {
            Debug.Assert(eventData != null);
            var teleportData = ExecuteEvents.ValidateEventData<TeleportEventData>(eventData);
            Debug.Assert(teleportData != null);
            Debug.Assert(!teleportData.used);

            // Process all the event listeners
            base.HandleEvent(teleportData, eventHandler);
        }

        /// <summary>
        /// Unregister a <see cref="GameObject"/> from listening to Teleport events.
        /// </summary>
        /// <param name="listener"></param>
        public override void Register(GameObject listener)
        {
            base.Register(listener);
        }

        /// <summary>
        /// Unregister a <see cref="GameObject"/> from listening to Teleport events.
        /// </summary>
        /// <param name="listener"></param>
        public override void Unregister(GameObject listener)
        {
            base.Unregister(listener);
        }

        #endregion IEventSystemManager Implementation

        private static readonly ExecuteEvents.EventFunction<IMixedRealityTeleportHandler> OnTeleportRequestHandler =
            delegate (IMixedRealityTeleportHandler handler, BaseEventData eventData)
            {
                var casted = ExecuteEvents.ValidateEventData<TeleportEventData>(eventData);
                handler.OnTeleportRequest(casted);
            };

        /// <inheritdoc />
        public void RaiseTeleportRequest(IMixedRealityPointer pointer, IMixedRealityTeleportTarget target)
        {
            // initialize event
            teleportEventData.Initialize(pointer, target);

            // Pass handler
            HandleEvent(teleportEventData, OnTeleportRequestHandler);
        }

        private static readonly ExecuteEvents.EventFunction<IMixedRealityTeleportHandler> OnTeleportStartedHandler =
            delegate (IMixedRealityTeleportHandler handler, BaseEventData eventData)
            {
                var casted = ExecuteEvents.ValidateEventData<TeleportEventData>(eventData);
                handler.OnTeleportStarted(casted);
            };

        /// <inheritdoc />
        public void RaiseTeleportStarted(IMixedRealityPointer pointer, IMixedRealityTeleportTarget target)
        {
            // initialize event
            teleportEventData.Initialize(pointer, target);

            // Pass handler
            HandleEvent(teleportEventData, OnTeleportStartedHandler);
        }

        private static readonly ExecuteEvents.EventFunction<IMixedRealityTeleportHandler> OnTeleportCompletedHandler =
            delegate (IMixedRealityTeleportHandler handler, BaseEventData eventData)
            {
                var casted = ExecuteEvents.ValidateEventData<TeleportEventData>(eventData);
                handler.OnTeleportCompleted(casted);
            };

        /// <inheritdoc />
        public void RaiseTeleportComplete(IMixedRealityPointer pointer, IMixedRealityTeleportTarget target)
        {
            // initialize event
            teleportEventData.Initialize(pointer, target);

            // Pass handler
            HandleEvent(teleportEventData, OnTeleportCompletedHandler);
        }

        private static readonly ExecuteEvents.EventFunction<IMixedRealityTeleportHandler> OnTeleportCanceledHandler =
            delegate (IMixedRealityTeleportHandler handler, BaseEventData eventData)
            {
                var casted = ExecuteEvents.ValidateEventData<TeleportEventData>(eventData);
                handler.OnTeleportCanceled(casted);
            };

        /// <inheritdoc />
        public void RaiseTeleportCanceled(IMixedRealityPointer pointer, IMixedRealityTeleportTarget target)
        {
            // initialize event
            teleportEventData.Initialize(pointer, target);

            // Pass handler
            HandleEvent(teleportEventData, OnTeleportCanceledHandler);
        }
    }
}
