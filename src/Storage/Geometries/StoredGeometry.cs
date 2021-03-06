﻿// <copyright file="StoredGeometry.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
//
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

namespace AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AEGIS.Collections;
    using AEGIS.Geometries;
    using AEGIS.Resources;
    using AEGIS.Storage.Resources;

    /// <summary>
    /// Represents a geometry located in a store.
    /// </summary>
    public abstract class StoredGeometry : Geometry, IStoredGeometry
    {
        /// <summary>
        /// The empty array of indexes. This field is read-only.
        /// </summary>
        private static readonly Int32[] EmptyIndexes = new Int32[0];

        /// <summary>
        /// The geometry driver. This field is read-only.
        /// </summary>
        private readonly IGeometryDriver driver;

        /// <summary>
        /// The array of indexes. This field is read-only.
        /// </summary>
        private readonly Int32[] indexes;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredGeometry" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system driver.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        protected StoredGeometry(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, referenceSystem)
        {
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            this.indexes = indexes == null ? EmptyIndexes : indexes.ToArray();
        }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        public override Envelope Envelope
        {
            get
            {
                return this.indexes.Length == 0 ? this.Driver.ReadEnvelope(this.Identifier) : this.Driver.ReadEnvelope(this.Identifier, this.indexes);
            }
        }

        /// <summary>
        /// Gets the feature identifier.
        /// </summary>
        /// <value>The unique feature identifier within the store.</value>
        public String Identifier { get; private set; }

        /// <summary>
        /// Gets the collection of indexes within the feature.
        /// </summary>
        /// <value>The collection of indexes which determine the location of the geometry within the feature.</value>
        public IEnumerable<Int32> Indexes { get { return this.indexes; } }

        /// <summary>
        /// Gets the driver of the geometry.
        /// </summary>
        /// <value>The driver of the geometry.</value>
        public IGeometryDriver Driver { get { return this.driver; } }

        /// <summary>
        /// Creates a coordinate for the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        protected void CreateCoordinate(Coordinate coordinate)
        {
            if (this.indexes.Length == 0)
                this.Driver.CreateCoordinate(coordinate, this.Identifier);
            else
                this.Driver.CreateCoordinate(coordinate, this.Identifier, this.indexes);
        }

        /// <summary>
        /// Creates a coordinate for the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="index">The index of the coordinate within the geometry.</param>
        protected void CreateCoordinate(Coordinate coordinate, Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.CreateCoordinate(coordinate, this.Identifier, index);
            else
                this.Driver.CreateCoordinate(coordinate, this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Creates a collection of coordinates for the geometry.
        /// </summary>
        /// <param name="collection">The list of coordinates.</param>
        protected void CreateCoordinates(IReadOnlyList<Coordinate> collection)
        {
            if (this.indexes.Length == 0)
                this.Driver.CreateCoordinates(collection, this.Identifier);
            else
                this.Driver.CreateCoordinates(collection, this.Identifier, this.indexes);
        }

        /// <summary>
        /// Creates a collection of coordinates for the geometry.
        /// </summary>
        /// <param name="collection">The list of coordinates.</param>
        /// <param name="index">The starting index of the coordinates within the geometry.</param>
        protected void CreateCoordinates(IReadOnlyList<Coordinate> collection, Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.CreateCoordinates(collection, this.Identifier, index);
            else
                this.Driver.CreateCoordinates(collection, this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads the number of coordinate collections of the geometry.
        /// </summary>
        /// <returns>The number of coordinate collections within the geometry.</returns>
        protected Int32 ReadCollectionCount()
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCollectionCount(this.Identifier);
            else
                return this.Driver.ReadCollectionCount(this.Identifier, this.indexes);
        }

        /// <summary>
        /// Reads the number of coordinate collections of the geometry.
        /// </summary>
        /// <param name="index">The starting index of the coordinates within the geometry.</param>
        /// <returns>The number of coordinate collections within the geometry.</returns>
        protected Int32 ReadCollectionCount(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCollectionCount(this.Identifier, index);

            return this.Driver.ReadCollectionCount(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads a coordinate of the specified geometry.
        /// </summary>
        /// <param name="index">The index of the coordinate.</param>
        /// <returns>The coordinate.</returns>
        protected Coordinate ReadCoordinate(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinate(this.Identifier, index);

            return this.Driver.ReadCoordinate(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads the number of coordinate of the geometry.
        /// </summary>
        /// <returns>The number of coordinates within the geometry.</returns>
        protected Int32 ReadCoordinateCount()
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinateCount(this.Identifier);

            return this.Driver.ReadCoordinateCount(this.Identifier, this.indexes);
        }

        /// <summary>
        /// Reads the number of coordinate of the geometry.
        /// </summary>
        /// <param name="index">The index of the coordinates within the geometry.</param>
        /// <returns>The number of coordinates within the geometry.</returns>
        protected Int32 ReadCoordinateCount(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinateCount(this.Identifier, index);

            return this.Driver.ReadCoordinateCount(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads the coordinates of the geometry.
        /// </summary>
        /// <returns>The list of coordinates.</returns>
        protected IReadOnlyList<Coordinate> ReadCoordinates()
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinates(this.Identifier);

            return this.Driver.ReadCoordinates(this.Identifier, this.indexes);
        }

        /// <summary>
        /// Reads the coordinates of the geometry.
        /// </summary>
        /// <param name="index">The index of the coordinates within the geometry.</param>
        /// <returns>The list of coordinates.</returns>
        protected IReadOnlyList<Coordinate> ReadCoordinates(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinates(this.Identifier, index);

            return this.Driver.ReadCoordinates(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads the specified inner geometry.
        /// </summary>
        /// <param name="index">The index of the inner geometry within the geometry.</param>
        /// <returns>The geometry read by the driver.</returns>
        protected IGeometry ReadGeometry(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadGeometry(this.Identifier, index);
            else
                return this.Driver.ReadGeometry(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Updates a coordinate of the geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="index">The index of the coordinate within the geometry.</param>
        protected void UpdateCoordinate(Coordinate coordinate, Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.UpdateCoordinate(coordinate, this.Identifier, index);
            else
                this.Driver.UpdateCoordinate(coordinate, this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Updates the coordinates of the geometry.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        protected void UpdateCoordinates(IReadOnlyList<Coordinate> coordinates)
        {
            if (this.indexes.Length == 0)
                this.Driver.UpdateCoordinates(coordinates, this.Identifier);
            else
                this.Driver.UpdateCoordinates(coordinates, this.Identifier, this.indexes);
        }

        /// <summary>
        /// Deletes the coordinates of the geometry.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="index">The index of the coordinate collection within the geometry.</param>
        protected void UpdateCoordinates(IReadOnlyList<Coordinate> coordinates, Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.UpdateCoordinates(coordinates, this.Identifier, index);
            else
                this.Driver.UpdateCoordinates(coordinates, this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Deletes a coordinate of the specified geometry.
        /// </summary>
        /// <param name="index">The index of the coordinate.</param>
        protected void DeleteCoordinate(Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.DeleteCoordinate(this.Identifier, index);
            else
                this.Driver.DeleteCoordinate(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Deletes the coordinates of the geometry.
        /// </summary>
        protected void DeleteCoordinates()
        {
            if (this.indexes.Length == 0)
                this.Driver.DeleteCoordinates(this.Identifier);
            else
                this.Driver.DeleteCoordinates(this.Identifier, this.indexes);
        }

        /// <summary>
        /// Deletes the coordinates of the geometry.
        /// </summary>
        /// <param name="index">The index of the coordinate collection within the geometry.</param>
        protected void DeleteCoordinates(Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.DeleteCoordinates(this.Identifier, index);
            else
                this.Driver.DeleteCoordinates(this.Identifier, this.indexes.Append(index));
        }
    }
}
