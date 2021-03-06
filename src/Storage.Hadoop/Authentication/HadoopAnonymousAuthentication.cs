﻿// <copyright file="HadoopAnonymousAuthentication.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Authentication
{
    using System;
    using AEGIS.Storage;

    /// <summary>
    /// Represents an anonymous Hadoop file system authentication.
    /// </summary>
    public class HadoopAnonymousAuthentication : IHadoopAuthentication
    {
        /// <summary>
        /// Gets the type of the authentication.
        /// </summary>
        /// <value>The type of the authentication.</value>
        public StorageAuthenticationType AutenticationType { get { return StorageAuthenticationType.Anonymous; } }

        /// <summary>
        /// Gets the request of the authentication.
        /// </summary>
        /// <value>The request form of the authentication.</value>
        public String Request { get { return String.Empty; } }
    }
}
