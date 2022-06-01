using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data
{
    public class Constants
    {
        public const string CONNECTION_STRING = "ConnectionStrings";

        public const string USER_INFO_EMAIL = "email";
        public const string USER_RAW_ID = "rawId";
        public const string USER_INFO_PICTURE = "photoUrl";
        public const string PROVIDER_USER_INFO = "providerUserInfo";
        public const string PROVIDER_ID = "providerId";
        public const string GOOGLE_USER_INFO_PROVIDER = "Google";

        public const string USER_INFO_DISPLAY_NAME = "displayName";
        public const string USERS = "users";

        public const string GOOGLE_CREDENTIAL = "GoogleCredential";
        public const string GOOGLE_TOKEN_INFO_URI = "token_info_uri";
        public const string GOOGLE_TOKEN_PARAM = "idToken";

        public const string JWT = "Jwt";
        public const string JWT_EXPIRED_IN = "ExpiredIn";
        public const string JWT_REFRESH_EXPIRED_IN = "RefreshExpiredIn";

        public const string JWT_CLAIM_PICTURE = "Picture";

        public static DateTime JSON_START_DATE = new DateTime(1970, 1, 1);

        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_MANAGER = "Manager";
        public const string ROLE_CUSTOMER = "Customer";

        public const string QUESTION_MARK = "?";


        public const string MESSAGE_AUTHORIZED = "Already authorized";
        public const string MESSAGE_SUCCESS = "Success";
        public const string MESSAGE_INVALID_ACCOUNT = "Invalid account";
        public const string MESSAGE_INVALID_REFRESH_TOKEN = "Invalid refresh token";
        public const string MESSAGE_EXIRED_REFRESH_TOKEN = "Expired refresh token";

        public const string MESSAGE_NOT_FOUND = "Not found";
        public const string MESSAGE_OPERATION_FAILED = "Operation failed";

        public const string CONSTRAIN_REQUEST_MESSAGE_REQUIRED = "Required";
        public const string CONSTRAIN_REQUEST_MESSAGE_INVALID_FIELD = "Invalid field";


        
    }
}
