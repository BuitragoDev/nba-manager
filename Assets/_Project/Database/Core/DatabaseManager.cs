using System;
using System.IO;
using SQLite;
using UnityEngine;

namespace NBAManager.Database
{
    /// <summary>
    /// Punto de acceso único a SQLite en todo el juego.
    /// Una instancia = una conexión a un archivo .db (una partida).
    /// Ningún otro sistema abre conexiones SQLite directamente.
    /// </summary>
    public class DatabaseManager : IDisposable
    {
        // ─── Singleton de sesión ───────────────────────────────────────────
        // No es un MonoBehaviour singleton. Es una instancia gestionada
        // por GameSession, que controla su ciclo de vida explícitamente.
        private static DatabaseManager _current;
        public static DatabaseManager Current
        {
            get
            {
                if (_current == null)
                    throw new InvalidOperationException(
                        "[DatabaseManager] No hay ninguna partida activa. " +
                        "Llama a GameSession.StartNew() o GameSession.Load() primero.");
                return _current;
            }
        }

        public static bool HasActiveSession => _current != null;

        // ─── Conexión ──────────────────────────────────────────────────────
        private SQLiteConnection _connection;
        private readonly string _dbPath;
        private bool _disposed = false;

        // ─── Constructor ───────────────────────────────────────────────────
        private DatabaseManager(string dbPath)
        {
            _dbPath = dbPath;
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            _connection.BusyTimeout = TimeSpan.FromSeconds(5);
            Debug.Log($"[DatabaseManager] Conexión abierta: {dbPath}");
        }

        // ─── Apertura de sesión ────────────────────────────────────────────
        public static DatabaseManager Open(string dbPath)
        {
            if (_current != null)
            {
                Debug.LogWarning("[DatabaseManager] Cerrando sesión anterior antes de abrir nueva.");
                _current.Dispose();
            }

            _current = new DatabaseManager(dbPath);
            return _current;
        }

        // ─── Acceso a la conexión (solo para Repositories) ────────────────
        /// <summary>
        /// Expone la conexión SOLO a clases dentro del namespace Database.
        /// El resto del juego usa Repositories, nunca esto directamente.
        /// </summary>
        internal SQLiteConnection Connection
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DatabaseManager));
                return _connection;
            }
        }

        // ─── Transacciones ────────────────────────────────────────────────
        /// <summary>
        /// Ejecuta múltiples operaciones como una transacción atómica.
        /// Usar siempre que múltiples tablas se modifiquen juntas.
        /// </summary>
        public void RunInTransaction(Action action)
        {
            Connection.BeginTransaction();
            try
            {
                action();
                Connection.Commit();
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                Debug.LogError($"[DatabaseManager] Transacción fallida, rollback aplicado: {ex.Message}");
                throw;
            }
        }

        // ─── Ejecución directa SQL ────────────────────────────────────────
        /// <summary>
        /// Para migraciones y creación de tablas.
        /// </summary>
        public void Execute(string sql, params object[] args)
        {
            Connection.Execute(sql, args);
        }

        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            return Connection.ExecuteScalar<T>(sql, args);
        }

        // ─── Información de la DB activa ──────────────────────────────────
        public string DbPath => _dbPath;

        // ─── Cierre y limpieza ────────────────────────────────────────────
        public void Dispose()
        {
            if (_disposed) return;

            _connection?.Close();
            _connection = null;
            _disposed = true;

            if (_current == this)
                _current = null;

            Debug.Log($"[DatabaseManager] Conexión cerrada: {_dbPath}");
        }
    }
}