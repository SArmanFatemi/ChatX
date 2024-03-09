using SAF.ChatX.Models;
using System.Collections.Concurrent;

namespace SAF.ChatX.Persistence;

public class DatabaseContext
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;
}