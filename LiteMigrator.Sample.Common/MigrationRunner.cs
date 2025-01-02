using System.Reflection;

namespace LiteMigrator.Sample.Common;

/// <summary>Class for loading the migration scripts in an external DLL.</summary>
public static class MigrationRunner
{
  public static Migrator InitLiteMigrator(string dbPath)
  {
    var assm = Assembly.GetExecutingAssembly();
    var resourceNamespace = "LiteMigrator.Sample.Common.Scripts";
    return new Migrator(dbPath, resourceNamespace, assm);
  }
}
