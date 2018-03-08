namespace TwitchLib.Webhook
{
    public static class TwitchConstants
    {
        public static string ReceiverName => "twitch";

        public static string ChallengeQueryParameterName => "hub.challenge";

        public static int SecretKeyMinLength => 8;

        public static int SecretKeyMaxLength => 128;

        public static string SignatureHeaderName => "X-Hub-Signature";


    }
}
