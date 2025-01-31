using Microsoft.Extensions.Options;
using TT.Auth.DataProviders;
using TT.Core;

namespace TT.Auth;

#region Options class
public class DatabaseMetaInfoOptions 
{
    public string ConnectionString { get; set; }
}
#endregion

public class DatabaseMetaInfoService : IDatabaseMetaInfoService
{
    private readonly IStorageProvider _storageProvider;

    public DatabaseMetaInfoService(IOptions<DatabaseMetaInfoOptions> options)
    {
        if (options == null) 
            throw new ArgumentNullException(nameof(options));
        if (options.Value == null)
            throw new ArgumentNullException(nameof(options.Value));

        _storageProvider = new PostgreDataProvider(options.Value.ConnectionString);
    }

    public string GetDefaultDatabaseName() => _storageProvider.GetDefautDatabaseName();

    public string GetDatabaseVendorName() => _storageProvider.GetDatabaseVendorName();
}
