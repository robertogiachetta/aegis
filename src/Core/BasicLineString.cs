﻿// <copyright file="BasicLineString.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using AEGIS.Collections;
    using AEGIS.Collections.Resources;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a basic line string geometry in spatial coordinate space.
    /// </summary>
    public class BasicLineString : IBasicLineString
    {
        /// <summary>
        /// The string format for coordinates. This field is constant.
        /// </summary>
        private const String CoordinateStringFormat = "{0} {1} {2}";

        /// <summary>
        /// The divider for coordinates. This field is constant.
        /// </summary>
        private const String CoordinateStringDivider = ",";

        /// <summary>
        /// The string for empty line strings. This field is constant.
        /// </summary>
        private const String LineStringEmptyString = "LINESTRING EMPTY";

        /// <summary>
        /// The string format for line strings. This field is constant.
        /// </summary>
        private const String LineStringStringFormat = "LINESTRING ({0})";

        /// <summary>
        /// The list of coordinates.
        /// </summary>
        private readonly List<Coordinate> coordinates;

        /// <summary>
        /// The envelope of the polygon.
        /// </summary>
        private Envelope envelope;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicLineString" /> class.
        /// </summary>
        public BasicLineString()
        {
            this.coordinates = new List<Coordinate>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicLineString" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BasicLineString(IEnumerable<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            this.coordinates = new List<Coordinate>(source.Elements());
        }

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>
        /// The inherent dimension of the geometry. The inherent dimension is always less than or equal to the coordinate dimension.
        /// </value>
        public Int32 Dimension { get { return 1; } }

        /// <summary>
        /// Gets the minimum bounding <see cref="Envelope" /> of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        public Envelope Envelope
        {
            get { return this.envelope ?? (this.envelope = Envelope.FromCoordinates(this.coordinates)); }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty
        {
            get { return this.coordinates.Count == 0 || this.coordinates.All(coordinate => coordinate.IsEmpty); }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public Boolean IsValid
        {
            get { return this.coordinates.All(coordinate => coordinate.IsValid); }
        }

        /// <summary>
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <value>The coordinate located at the specified index.</value>
        /// <param name="index">The zero-based index of the coordinate to get.</param>
        /// <returns>The coordinate located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public Coordinate this[Int32 index]
        {
            get { return this.GetCoordinate(index); }
        }

        /// <summary>
        /// Gets the number of coordinates in the line string.
        /// </summary>
        /// <value>The number of coordinates in the line string.</value>
        public Int32 Count { get { return this.coordinates.Count; } }

        /// <summary>
        /// Gets a value indicating whether the line string is closed.
        /// </summary>
        /// <value><c>true</c> if the first coordinate of the line string is equal to the last coordinate; otherwise, <c>false</c>.</value>
        public Boolean IsClosed { get { return this.coordinates.Count > 0 && this.coordinates[0] == this.coordinates[this.coordinates.Count - 1]; } }

        /// <summary>
        /// Gets the staring coordinate.
        /// </summary>
        /// <value>The first coordinate of the line string.</value>
        public Coordinate StartCoordinate
        {
            get
            {
                if (this.coordinates.Count == 0)
                    return null;
                return this.coordinates[0];
            }
        }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The last coordinate of the line string.</value>
        public Coordinate EndCoordinate
        {
            get
            {
                if (this.coordinates.Count == 0)
                    return null;
                return this.coordinates[this.coordinates.Count - 1];
            }
        }

        /// <summary>
        /// Determines whether the line string contains the specified coordinate within its coordinates.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line string contains the specified coordinate within its coordinates; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public virtual Boolean Contains(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));

            return this.coordinates.Contains(coordinate);
        }

        /// <summary>
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to get.</param>
        /// <returns>The coordinate located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual Coordinate GetCoordinate(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.coordinates.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            return this.coordinates[index];
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{Coordinate}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            foreach (Coordinate coordinate in this.coordinates)
                yield return coordinate;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return this.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public String ToString(IFormatProvider provider)
        {
            if (this.IsEmpty)
                return LineStringEmptyString;

            StringBuilder builder = new StringBuilder();
            for (Int32 index = 0; index < this.coordinates.Count; index++)
            {
                if (index > 0)
                    builder.Append(CoordinateStringDivider);

                builder.Append(String.Format(provider, CoordinateStringFormat, this.coordinates[index].X, this.coordinates[index].Y, this.coordinates[index].Z));
            }

            return String.Format(provider, LineStringStringFormat, builder.ToString());
        }
    }
}
