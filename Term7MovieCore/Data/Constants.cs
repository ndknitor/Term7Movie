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

        public const string GOOGLE_USER_INFO_GIVEN_NAME = "given_name";
        public const string GOOGLE_USER_INFO_FAMILY_NAME = "family_name";
        public const string GOOGLE_USER_INFO_EMAIL = "email";
        public const string GOOGLE_USER_INFO_SUB = "sub";
        public const string GOOGLE_USER_INFO_PICTURE = "picture";
        public const string GOOGLE_USER_INFO_PROVIDER = "Google";


        public const string GOOGLE_CREDENTIAL = "GoogleCredential";
        public const string GOOGLE_TOKEN_INFO_URI = "token_info_uri";
        public const string GOOGLE_TOKEN_PARAM = "id_token=";

        public const string JWT = "Jwt";
        public const string JWT_EXPIRED_IN = "ExpiredIn";
        public const string JWT_REFRESH_EXPIRED_IN = "RefreshExpiredIn";

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
    }
}
