﻿// 
//  ISyncGatewayRESTApi.cs
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Newtonsoft.Json;

using RestEase;

namespace Couchbase.Lite.Sync
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface ISyncGatewayRESTApi
    {
        #region Properties

        [Header("Cookie")]
        string AuthCookie { get; set; }

        [Header("Authorization")]
        string AuthHeader { get; set; }

        #endregion

        #region Public Methods

        [NotNull]
        [ItemNotNull]
        [Delete("{db}/{doc}")]
        Task<PutResponse> DeleteDocumentAsync([Path] string db, [Path] string doc, string rev);

        [NotNull]
        [Delete("{db}/_local/{local_doc}")]
        Task DeleteLocalDocumentAsync([Path] string db, [Path] string local_doc,
            string rev = null, string batch = null);

        [NotNull]
        [Delete("{db}/_session")]
        Task DeleteSessionAsync([Path] string db, [Header("Cookie")] string authCookie);

        [NotNull]
        [ItemNotNull]
        [Get("{db}/_all_docs")]
        Task<AllDocsResponse> GetAllDocsAsync([Path] string db, bool access = false, bool channels = false, bool include_docs = false,
            bool revs = false, bool update_seq = false, int limit = Int32.MaxValue, string[] keys = null,
            string startKey = null, string endKey = null);

        [NotNull]
        [Get("{db}/{doc}/{attachment}")]
        Task<Response<Stream>> GetAttachmentAsync([Path]string db, [Path]string doc, [Path]string attachment, string rev = null);

        [NotNull]
        [ItemNotNull]
        [Get("{db}/")]
        Task<DbResponse> GetDbAsync([Path]string db);

        [NotNull]
        [ItemNotNull]
        [Get("{db}/{doc}")]
        [Header("Accept", "application/json")]
        Task<DocumentResponse> GetDocumentAsync([Path] string db, [Path] string doc,
            bool attachments = false, string atts_since = null, string[] open_revs = null, bool revs = false,
            bool show_exp = false);

        [NotNull]
        [ItemNotNull]
        [Get("{db}/_local/{local_doc}")]
        Task<DocumentResponse> GetLocalDocumentAsync([Path] string db, [Path] string local_doc);

        [NotNull]
        [ItemNotNull]
        [Get("{db}/_oidc")]
        Task<HttpResponseMessage> GetOidc([Path] string db, bool offline = false, string provider = null);

        [NotNull]
        [ItemNotNull]
        [Get("{db}/_oidc_callback")]
        Task<OidcCallbackResponse> GetOidcCallbackAsync([Path] string db, string code, string provider = null);

        [NotNull]
        [ItemNotNull]
        [Get("{db}/_oidc_challenge")]
        Task<HttpResponseMessage> GetOidcChallenge([Path] string db, bool offline = false, string provider = null);

        [NotNull]
        [Get("{db}/_oidc_refresh")]
        Task<OidcRefreshResponse> GetOidcRefreshAsync([Path] string db, string refresh_token, string provider = null);

        [NotNull]
        [ItemNotNull]
        [Get("/")]
        Task<ServerInfoResponse> GetServerInfoAsync();

        [NotNull]
        [ItemNotNull]
        [Post("{db}/_all_docs")]
        Task<AllDocsResponse> PostAllDocsAsync([Path] string db, [Body] IDictionary<string, object> body,
            bool access = false, bool channels = false, bool include_docs = false, bool revs = false,
            bool update_seq = false);

        [NotNull]
        [ItemNotNull]
        [Post("{db}/_bulk_docs")] 
        Task<IReadOnlyList<BulkDocsResponseItem>> PostBulkDocsAsync([Path] string db,
            [Body] IDictionary<string, object> body);

        [NotNull]
        [ItemNotNull]
        [Post("{db}/_bulk_get")]
        Task<HttpResponseMessage> PostBulkGetAsync([Path] string db,
            [Body] IDictionary<string, IReadOnlyList<IDictionary<string, object>>> body,
            bool revs = false, int revs_limit = 0, bool attachments = false);

        [NotNull]
        [ItemNotNull]
        [Post("{db}/")]
        Task<PutResponse> PostDocumentAsync([Path] string db, [Body] IDictionary<string, object> body);

        [NotNull]
        [Post("{db}/_changes")]
        Task<Stream> PostLongChangesFeed([Path]string db, [Body]IDictionary<string, object> body);

        [NotNull]
        [Post("{db}/_changes")]
        Task<ChangesFeedResponse> PostNormalChangesFeed([Path] string db, [Body]IDictionary<string, object> body);

        [NotNull]
        [ItemNotNull]
        [Post("{db}/_session")]
        Task<AdminCreateSessionResponse> PostSessionAsync([Path] string db, [Body] IDictionary<string, string> body);

        [NotNull]
        [Put("{db}/{doc}/{attachment}")]
        Task<PutResponse> PutAttachmentAsync([Path] string db, [Path] string doc, [Path] string attachment, string rev,
            [Body] Stream body, [Header("Content-Type")] string header = null);

        [NotNull]
        [ItemNotNull]
        [Put("{db}/{doc}")]
        Task<PutResponse> PutDocumentAsync([Path] string db, [Path] string doc, [Body] IDictionary<string, object> body,
            string rev = null, bool new_edits = true);

        [NotNull]
        [ItemNotNull]
        [Put("{db}/_local/{local_doc}")]
        Task<PutResponse> PutLocalDocumentAsync([Path] string db, [Path] string local_doc, [Body]IDictionary<string, object> body);

        #endregion
    }

    public interface ISyncGatewayAdminRESTApi : ISyncGatewayRESTApi
    {
        #region Public Methods

        [NotNull]
        [Delete("{db}/_session/{sessionid}")]
        Task AdminDeleteSessionIdAsync([Path] string db, [Path] string sessionid);

        [NotNull]
        [Delete("{db}/_user/{name}/_session")]
        Task AdminDeleteUserSessionAsync([Path] string db, [Path] string name);

        [NotNull]
        [Delete("{db}/_user/{name}/_session/{sessionid}")]
        Task AdminDeleteUserSessionAsync([Path] string db, [Path]string name, [Path] string sessionid);

        [NotNull]
        [ItemNotNull]
        [Post("{db}/_session")]
        Task<AdminCreateSessionResponse> AdminPostSessionAsync([Path] string db,
            [Body] IDictionary<string, object> body);

        [NotNull]
        [ItemNotNull]
        [Get("/_config")]
        Task<IReadOnlyDictionary<string, object>> GetConfigAsync();

        [NotNull]
        [ItemNotNull]
        [Get("/_expvar")]
        Task<IReadOnlyDictionary<string, object>> GetExpvarAsync();

        [NotNull]
        [ItemNotNull]
        [Get("/_logging")]
        Task<IReadOnlyDictionary<string, bool>> GetLoggingAsync();

        [NotNull]
        [ItemNotNull]
        [Get("{db}/_session/{sessionid}")]
        Task<SessionResponse> GetSessionAsync([Path] string db, [Path] string sessionid);

        [NotNull]
        [Post("/_logging")]
        Task PostLoggingAsync([Body]IDictionary<string, bool> body, int level = 1);

        [NotNull]
        [Put("/_logging")]
        Task PutLoggingAsync([Body]IDictionary<string, bool> body, int level = 1);

        #endregion
    }

    public sealed class PutResponse
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("rev")]
        public string Rev { get; set; }

        #endregion
    }

    public sealed class OidcCallbackResponse
    {
        #region Properties

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        #endregion
    }

    public sealed class OidcRefreshResponse
    {
        #region Properties

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        #endregion
    }

    public sealed class DbResponse
    {
        #region Properties

        [JsonProperty("committed_update_seq")]
        public long CommittedUpdateSeq { get; set; }

        [JsonProperty("compact_running")]
        public bool CompactRunning { get; set; }

        [JsonProperty("db_name")]
        public string DbName { get; set; }

        [JsonProperty("disk_format_version")]
        public int DiskFormatVersion { get; set; }

        [JsonProperty("instance_start_time")]
        public string InstanceStartTime { get; set; }

        [JsonProperty("purge_seq")]
        public long PurgeSeq { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("update_seq")]
        public string UpdateSeq { get; set; }

        #endregion
    }

    public sealed class AllDocsResponse
    {
        #region Properties

        [JsonProperty("offset")]
        public string Offset { get; set; }

        [JsonProperty("rows")]
        public IReadOnlyList<AllDocsRow> Rows { get; set; }

        [JsonProperty("total_rows")]
        public int TotalRows { get; set; }

        #endregion

        #region Nested

        public sealed class AllDocsRow
        {
            #region Properties

            [JsonProperty("doc")]
            public DocumentResponse Doc { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("key")]
            public object Key { get; set; }

            [JsonProperty("value")]
            public object Value { get; set; }

            #endregion
        }

        #endregion
    }

    public sealed class BulkDocsResponseItem
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("rev")]
        public string Rev { get; set; }

        #endregion
    }

    public sealed class ChangesFeedResponse
    {
        #region Properties

        [JsonProperty("last_seq")]
        public long LastSeq { get; set; }

        #endregion

        #region Nested

        public sealed class ChangesFeedEntry
        {
            #region Properties

            [JsonProperty("changes")]
            public IReadOnlyList<Change> Changes { get; set; }

            [JsonProperty("doc")]
            public IReadOnlyDictionary<string, object> Doc { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("seq")]
            public long Seq { get; set; }

            #endregion

            #region Nested

            public sealed class Change
            {
                #region Properties

                [JsonProperty("rev")]
                public string Rev { get; set; }

                #endregion
            }

            #endregion
        }

        #endregion
    }

    public sealed class DocumentResponse
    {
        #region Properties

        [JsonProperty("_attachments")]
        public IReadOnlyList<AttachmentEntry> Attachments { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonExtensionData]
        private IDictionary<string, object> Properties { get; set; }

        [JsonProperty("_rev")]
        public string Rev { get; set; }

        [JsonProperty("_revisions")]
        public RevisionDictionary Revisions { get; set; }

        #endregion

        #region Nested

        public sealed class RevisionDictionary
        {
            #region Properties

            [JsonProperty("ids")]
            public IReadOnlyList<string> Ids { get; set; }

            [JsonProperty("start")]
            public long Start { get; set; }

            #endregion
        }

        public sealed class AttachmentEntry
        {
            #region Properties

            [JsonProperty("data")]
            public string Base64Data { get; set; }

            [JsonProperty("content_type")]
            public string ContentType { get; set; }

            [JsonProperty("digest")]
            public string Digest { get; set; }

            [JsonProperty("length")]
            public long Length { get; set; }

            [JsonProperty("revpos")]
            public int RevPos { get; set; }

            [JsonProperty("stub")]
            public bool Stub { get; set; }

            #endregion
        }

        #endregion
    }

    public sealed class ServerInfoResponse
    {
        #region Properties

        [JsonProperty("vendor")]
        public VendorInfo Vendor { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("couchdb")]
        public string WelcomeMessage { get; set; }

        #endregion

        #region Nested

        public sealed class VendorInfo
        {
            #region Properties

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("version")]
            public float Version { get; set; }

            #endregion
        }

        #endregion
    }

    public sealed class AdminCreateSessionResponse
    {
        #region Properties

        [JsonProperty("cookie_name")]
        public string CookieName { get; set; }

        [JsonProperty("expires")]
        public DateTimeOffset Expires { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        #endregion
    }

    public sealed class SessionResponse
    {
        #region Properties

        [JsonProperty("authentication_handlers")]
        public IReadOnlyList<string> AuthHandlers { get; set; }

        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("userCtx")]
        public UserContext UserCtx { get; set; }

        #endregion

        #region Nested

        public sealed class UserContext
        {
            #region Properties

            [JsonProperty("channels")]
            public IReadOnlyDictionary<string, int> Channels { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            #endregion
        }

        #endregion
    }
}