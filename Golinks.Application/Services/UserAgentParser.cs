using UAParser;

namespace Golinks.Application.Services;

public record ParsedUserAgent(string DeviceType, string? DeviceModel, string Browser, string Os, bool IsBot);

public static class UserAgentParser
{
    private static readonly Parser _parser = Parser.GetDefault();

    public static ParsedUserAgent? Parse(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return null;

        var client = _parser.Parse(userAgent);

        var isBot = client.Device.IsSpider;
        var browser = client.UA.Family ?? "Other";
        var os = client.OS.Family ?? "Other";

        string deviceType;
        string? deviceModel = null;

        if (isBot)
        {
            deviceType = "Bot";
        }
        else
        {
            var ua = userAgent.ToLowerInvariant();
            deviceType = ua.Contains("tablet") || ua.Contains("ipad")
                ? "Tablet"
                : ua.Contains("mobile") || ua.Contains("android") || ua.Contains("iphone") || ua.Contains("ipod")
                    ? "Mobile"
                    : "Desktop";

            if (deviceType is "Mobile" or "Tablet")
            {
                var family = client.Device.Family;
                if (!string.IsNullOrWhiteSpace(family) && family != "Other")
                    deviceModel = family;
            }
        }

        return new ParsedUserAgent(deviceType, deviceModel, browser, os, isBot);
    }
}
