using System;
using System.IO;

namespace Reloaded.Mod.Loader.Utilities.Steam;

/// <summary>
/// Selects applications for which the managed Steam API detours must not be installed.
/// </summary>
internal static class SteamHookPolicy
{
    /// <summary>
    /// GBFR's steam_api64.dll calls SteamAPI_IsSteamRunning from a DLL_THREAD_ATTACH
    /// routine. A managed detour can re-enter CoreCLR while the new native thread is
    /// still being registered, which results in a runtime fatal error. ASI loading
    /// already bypasses the launcher/DRM path, so Steam launch hooks are unnecessary.
    /// </summary>
    internal static bool ShouldSkip(string applicationPath, bool loadedExternally, string bootstrapperPath)
    {
        var isGbfr = string.Equals(Path.GetFileName(applicationPath),
            "granblue_fantasy_relink.exe", StringComparison.OrdinalIgnoreCase);
        var isAsi = string.Equals(Path.GetExtension(bootstrapperPath),
            ".asi", StringComparison.OrdinalIgnoreCase);
        return isGbfr && (loadedExternally || isAsi);
    }
}
