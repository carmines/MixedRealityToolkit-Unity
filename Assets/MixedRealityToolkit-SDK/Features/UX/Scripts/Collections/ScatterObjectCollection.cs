﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Internal.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.SDK.UX.Collections
{
    /// <summary>
    /// A Scatter Object Collection is simply a set of child objects randomly laid out within a radius.
    /// Pressing "update collection" will run the randomization, feel free to run as many times until you get the desired result.
    /// </summary>
    public class ScatterObjectCollection : GridObjectCollection
    {
        /// <summary>
        /// Overriding base function for laying out all the children when UpdateCollection is called.
        /// </summary>
        protected override void LayoutChildren()
        {
            float startOffsetX;
            float startOffsetY;
            Vector3[] nodeGrid = new Vector3[ NodeList.Count ];
            Vector3 newPos = Vector3.zero;

            // Now lets lay out the grid
            columns = Mathf.CeilToInt( (float)NodeList.Count / Rows );
            startOffsetX = ( columns * 0.5f ) * CellWidth;
            startOffsetY = ( Rows * 0.5f ) * CellHeight;
            halfCell = new Vector2( CellWidth * 0.5f, CellHeight * 0.5f );

            // First start with a grid then project onto surface
            ResolveGridLayout( nodeGrid, startOffsetX, startOffsetY, Layout );

            // Get randomized planar mapping
            // Calculate radius of each node while we're here
            // Then use the packer function to shift them into place
            for ( int i = 0; i < NodeList.Count; i++ )
            {
                ObjectCollectionNode node = NodeList[ i ];

                newPos = VectorExtensions.ScatterMapping( nodeGrid[ i ], Radius );
                Collider nodeCollider = NodeList[ i ].transform.GetComponentInChildren<Collider>();
                if ( nodeCollider != null )
                {
                    // Make the radius the largest of the object's dimensions to avoid overlap
                    Bounds bounds = nodeCollider.bounds;
                    node.Radius = Mathf.Max( Mathf.Max( bounds.size.x, bounds.size.y ), bounds.size.z ) * 0.5f;
                }
                else
                {
                    // Make the radius a default value
                    node.Radius = 1f;
                }
                node.transform.localPosition = newPos;
                UpdateNodeFacing( node );
                NodeList[ i ] = node;
            }

            // Iterate [x] times
            for ( int i = 0; i < 100; i++ )
            {
                IterateScatterPacking( NodeList, Radius );
            }
         }

        /// <summary>
        /// Pack randomly spaced nodes so they don't overlap
        /// Usually requires about 25 iterations for decent packing
        /// </summary>
        private void IterateScatterPacking( List<ObjectCollectionNode> nodes, float radiusPadding )
        {
            // Sort by closest to center (don't worry about z axis)
            // Use the position of the collection as the packing center
            nodes.Sort( ScatterSort );

            Vector3 difference;
            Vector2 difference2D;

            // Move them closer together
            float radiusPaddingSquared = Mathf.Pow( radiusPadding, 2f );

            for ( int i = 0; i < nodes.Count - 1; i++ )
            {
                for ( int j = i + 1; j < nodes.Count; j++ )
                {
                    if ( i != j )
                    {
                        difference = nodes[ j ].transform.localPosition - nodes[ i ].transform.localPosition;
                        // Ignore Z axis
                        difference2D.x = difference.x;
                        difference2D.y = difference.y;
                        float combinedRadius = nodes[ i ].Radius + nodes[ j ].Radius;
                        float distance = difference2D.SqrMagnitude() - radiusPaddingSquared;
                        float minSeparation = Mathf.Min( distance, radiusPaddingSquared );
                        distance -= minSeparation;

                        if ( distance < ( Mathf.Pow( combinedRadius, 2 ) ) )
                        {
                            difference2D.Normalize();
                            difference *= ( ( combinedRadius - Mathf.Sqrt( distance ) ) * 0.5f );
                            nodes[ j ].transform.localPosition += difference;
                            nodes[ i ].transform.localPosition -= difference;
                        }
                    }
                }
            }
        }

        private int ScatterSort( ObjectCollectionNode circle1, ObjectCollectionNode circle2 )
        {
            float distance1 = ( circle1.transform.localPosition ).sqrMagnitude;
            float distance2 = ( circle2.transform.localPosition ).sqrMagnitude;
            return distance1.CompareTo( distance2 );
        }
    }
}
