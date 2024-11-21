namespace Infrastructure.Constant
{
    public static class CoreConstant
    {
        #region Data Check
        public const char STAR_CHAR = '*';
        public const string STAR_STRING = "*";

        public const char SYSTEM_SEPARATOR_CHAR = '|';
        public const string SYSTEM_SEPARATOR_STRING = "|";

        public const char DEFAULT_EMAIL_SEPARATOR_CHAR = ';';
        public const string DEFAULT_EMAIL_SEPARATOR_STRING = ";";
        public static readonly char[] EMAIL_SEPARATOR_CHARS = new char[] { ',', ';', '|' };

        public const int DEFAULT_MAX_DEGREE_OF_PARALLELISM = 5;
        public const int DEFAULT_PROCESSING_BLOCK_SIZE = 100;
        public const string CONNECTION_STRINGS = "CONNECTION_STRINGS";
        #endregion
    }
}
