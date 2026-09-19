using System;
using Api;
using Godot;

namespace Deckbuilder;

public sealed class GodotLogger : ILogger
{
    private readonly Action<string>? _sink;

    public GodotLogger(Action<string>? sink = null)
    {
        _sink = sink;
    }

    public void Log(string message)
    {
        GD.Print($"[Deckbuilder API] {message}");
        _sink?.Invoke(message);
    }

    public void LogWarning(string message)
    {
        GD.PushWarning($"[Deckbuilder API] {message}");
        _sink?.Invoke($"WARN: {message}");
    }

    public void LogError(string message)
    {
        GD.PushError($"[Deckbuilder API] {message}");
        _sink?.Invoke($"ERROR: {message}");
    }
}
