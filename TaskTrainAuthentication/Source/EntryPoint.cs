using TT.Core;
using TT.Auth;

internal static class EntryPoint
{
    private static ITTApp _authApp = new TaskTrainAuthentication();

    public static void Main(string[] args)
    {
        _authApp.Build(args);
        _authApp.Run();
    }
}