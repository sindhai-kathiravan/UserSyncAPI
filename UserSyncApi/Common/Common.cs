using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserSyncApi.Common
{
    public static class Common
    {
        public static class Headers
        {
            public const string CORRELATION_ID = "X-Correlation-Id";
        }
        public static class Messages
        {
            public const string REQUEST_BODY_IS_NULL = "Request body is null";
            public const string USER_RETRIEVED_SUCCESSFULLY = "User retrieved successfully";
            public const string REQUEST_COMPLETED_SUCCESSFULLY = "Request completed successfully";
            public const string INVALID_USER_ID = "Invalid user Id";
            public const string THE_REQUESTED_USER_WAS_NOT_FOUND = "The requested user was not found";
            public const string THE_REQUEST_IS_INVALID = "The request is invalid";
            public const string AUTHORIZATION_HAS_BEEN_DENIED_FOR_THIS_REQUEST = "Authorization has been denied for this request";
            public const string MISSING_AUTHORIZATION_HEADER = "Missing Authorization header";
            public const string USER_CREATED_SUCCESSFULLY = "User Created successfully";
            public const string AN_UNEXPECTED_ERROR_OCCURRED = "An unexpected error occurred";
            public const string AT_LEAST_ONE_TARGET_DATABASE_MUST_BE_SPECIFIED = "At least one target database must be specified";
            public const string INVALID_DATABASE_KEY_FOUND = "Invalid database key found";

        }
        public static class Errors
        {
            public const string ERR_NULL_BODY = "ERR_NULL_BODY";
            public const string ERR_INTERNAL_SERVER = "ERR_INTERNAL_SERVER";
            public const string ERR_NOT_FOUND = "ERR_NOT_FOUND";
            public const string ERR_BAD_REQUEST = "ERR_BAD_REQUEST";
            public const string ERR_UNAUTHORIZED = "ERR_UNAUTHORIZED";
            public const string ERR_VALIDATION_FAILUED = "ERR_VALIDATION_FAILUER";
        }
    }
}