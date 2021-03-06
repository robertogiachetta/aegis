﻿// <copyright file="StoredFeature.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Features
{
    using System;
    using AEGIS.Resources;
    using AEGIS.Storage.Resources;

    /// <summary>
    /// Represents a feature located in a store.
    /// </summary>
    public class StoredFeature : IStoredFeature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredFeature" /> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="identifier">The identifier.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredFeature(IFeatureDriver driver, String identifier)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));
            this.Factory = new StoredFeatureFactory(driver);
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredFeature" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The identifier.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredFeature(IStoredFeatureFactory factory, String identifier)
        {
            this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        /// <summary>
        /// Gets the attribute collection of the feature.
        /// </summary>
        /// <value>The collection of attribute.</value>
        public IAttributeCollection Attributes { get { return this.Driver.AttributeDriver.ReadAttributes(this.Identifier); } }

        /// <summary>
        /// Gets the geometry of the feature.
        /// </summary>
        /// <value>The geometry of the feature.</value>
        public IGeometry Geometry { get { return this.Driver.GeometryDriver.ReadGeometry(this.Identifier); } }

        /// <summary>
        /// Gets the unique identifier of the feature.
        /// </summary>
        /// <value>The identifier of the feature.</value>
        public String Identifier { get; private set; }

        /// <summary>
        /// Gets the driver of the feature.
        /// </summary>
        /// <value>The driver of the feature.</value>
        public IFeatureDriver Driver { get { return this.Factory.Driver; } }

        /// <summary>
        /// Gets the factory of the feature.
        /// </summary>
        /// <value>The factory implementation the feature was constructed by.</value>
        public IStoredFeatureFactory Factory { get; private set; }
    }
}
