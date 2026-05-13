namespace NBAManager.Database
{
    /// <summary>
    /// Versión actual del esquema de base de datos.
    /// Incrementar cada vez que se añadan o modifiquen tablas.
    /// Usado por el sistema de migraciones para detectar DBs antiguas.
    /// </summary>
    public static class SchemaVersion
    {
        public const int Current = 1;
    }
}