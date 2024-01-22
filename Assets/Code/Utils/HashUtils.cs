namespace Runner.Utils {

    /// <summary>
    /// Helper for hash functions
    /// </summary>
    public static class HashUtils {

        public static int GetHashCode(string value) {
            // implementation found somewhere at the internet
            // https://stackoverflow.com/questions/36845430/persistent-hashcode-for-strings

            int hash1 = 5381;
            int hash2 = hash1;

            for (int i = 0; i < value.Length && value[i] != '\0'; i += 2) {
                hash1 = ((hash1 << 5) + hash1) ^ value[i];
                if (i == value.Length - 1 || value[i + 1] == '\0')
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ value[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }

        public static int GetHashCode(params int[] @params) {
            // lazy solution suitable for most cases
            int hc = @params.Length;
            foreach (int val in @params) {
                hc = unchecked(hc * 314159 + val);
            }
            return hc;
        }
    }
}