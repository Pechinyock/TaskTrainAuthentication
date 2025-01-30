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
    /* [TODO] delete it and create method in IStorageProviders */
    private readonly PostgreDataProvider _postgreProvider;

    public DatabaseMetaInfoService(IOptions<DatabaseMetaInfoOptions> options)
    {
        if (options == null) 
            throw new ArgumentNullException(nameof(options));
        if (options.Value == null)
            throw new ArgumentNullException(nameof(options.Value));

        _postgreProvider = new PostgreDataProvider(options.Value.ConnectionString);
        _storageProvider = _postgreProvider;
    }

    public string GetCurrentDatabaseName() => _postgreProvider.GetCurrentDatabaseName();

    public string GetDatabaseVendorName() => _storageProvider.GetDatabaseVendorName();
}
