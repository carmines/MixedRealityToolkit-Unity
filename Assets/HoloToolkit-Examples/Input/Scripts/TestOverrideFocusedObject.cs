﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace HoloToolkit.Unity.InputModule.Tests
{
    public class TestOverrideFocusedObject : MonoBehaviour, IPointerHandler
    {
        private InputManager inputManager;
        private TextMesh textMesh;

        private void Start()
        {
            inputManager = InputManager.Instance;

            if (inputManager != null)
            {
                inputManager.OverrideFocusedObject = gameObject;
            }

            textMesh = FindObjectOfType<TextMesh>();
        }

        public void OnPointerUp(PointerEventData eventData) { }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerClicked(PointerEventData eventData)
        {
            if (textMesh != null && inputManager != null)
            {
                textMesh.text = "Air tap worked and OverrideFocusedObject is null.";
                inputManager.OverrideFocusedObject = null;
            }
        }
    }
}