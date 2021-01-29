using System;

namespace DAL.Vulcan.Mongo.Base.Encryption
{
    public static class EncryptionKeys
    {
        public const string Key_Base64String = "rOQJ9XJN2/7lnScAj18AW6gwDFWzOXft2nh9U0iu/Cw=";
        public const string IV_Base64String = "edcLZtODkBrpJnbq8BKNoA==";

        public static byte[] Key = Convert.FromBase64String(Key_Base64String);
        public static byte[] IV = Convert.FromBase64String(IV_Base64String);
    }
}
