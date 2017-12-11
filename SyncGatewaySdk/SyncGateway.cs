// 
//  SyncGateway.cs
// 
//  Author:
//   Jim Borden  <jim.borden@couchbase.com>
// 
//  Copyright (c) 2017 Couchbase, Inc All rights reserved.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// 

using JetBrains.Annotations;
using RestEase;
using System;
using System.Linq;

namespace Couchbase.Lite.Sync
{
    public sealed class SyncGateway
    {

        #region Variables

        [NotNull]
        public readonly ISyncGatewayAdminRESTApi Admin;

        [NotNull]
        public readonly ISyncGatewayRESTApi Public;

        [NotNull]
        private readonly Uri _publicUri;

        #endregion

        #region Properties

        public string Session
        {
            get => Public.AuthCookie?.Split('=')?.LastOrDefault();
            set => Public.AuthCookie = $"SyncGatewaySession={value}";
        }

        #endregion

        #region Constructors

        public SyncGateway([NotNull]Uri url, int publicPort, int adminPort)
        {
            var adminUrl = new Uri($"{url.Scheme}://{url.Authority}:{adminPort}");
            _publicUri = new Uri($"{url.Scheme}://{url.Authority}:{publicPort}");
            Public = RestClient.For<ISyncGatewayRESTApi>(_publicUri)
                         ?? throw new Exception("Unable to create public SG REST API");
            Admin = RestClient.For<ISyncGatewayAdminRESTApi>(adminUrl)
                        ?? throw new Exception("Unable to create admin SG REST API");
        }

        #endregion

        #region Public Methods

        [NotNull]
        public Uri GetReplicationUrl(string db)
        {
            var builder = new UriBuilder(_publicUri);
            builder.Scheme = builder.Scheme == "http" ? "blip" : "blips";
            builder.Path = db;
            return builder.Uri;
        }

        #endregion

    }
}