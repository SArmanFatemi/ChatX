using SAF.ChatX.Models;
using System.Collections.Concurrent;

namespace SAF.ChatX.Persistence;

public class DatabaseContext
{
    public ConcurrentBag<UserConnection> Connections { get; set; } = [];
}