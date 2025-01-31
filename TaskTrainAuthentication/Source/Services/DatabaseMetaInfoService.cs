using Microsoft.Extensions.Options;
using TT.Auth.DataProviders;
using TT.Stroage;

namespace TT.Auth;

#region Options class
public class DatabaseMetaInfoOptions 
{
    public string ConnectionString { get; set; }
}
#endregion

public class DatabaseMetaInfoService : IDatabaseMetaInfoService
{
    private readonly ISQLDataProvider _storageProvider;

    public DatabaseMetaInfoService(IOptions<DatabaseMetaInfoOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (options.Value == null)
            throw new ArgumentNullException(nameof(options.Value));

        _storageProvider = new PostgreDataProvider(options.Value.ConnectionString);
    }

    public string GetDefaultDatabaseName() => throw new NotImplementedException();

    public string GetDatabaseVendorName() => throw new NotImplementedException();
}
