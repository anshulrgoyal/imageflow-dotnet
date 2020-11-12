﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Imageflow.Bindings;

namespace Imageflow.Fluent
{
    public interface IOutputDestination : IDisposable
    {
        Task RequestCapacityAsync(int bytes);
        Task WriteAsync(ArraySegment<byte> bytes, CancellationToken cancellationToken);
        Task FlushAsync(CancellationToken cancellationToken);
    }

    public static class IOutputDestinationExtensions
    {
        public static async Task CopyFromStreamAsync(this IOutputDestination dest, Stream stream,
            CancellationToken cancellationToken)
        {
            if (stream.CanRead && stream.CanSeek)
            {
                await dest.RequestCapacityAsync((int) stream.Length);
            }

            const int bufferSize = 81920;
            var buffer = new byte[bufferSize];

            int bytesRead;
            while ((bytesRead =
                       await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await dest.WriteAsync(new ArraySegment<byte>(buffer, 0, bytesRead), cancellationToken)
                    .ConfigureAwait(false);
            }

            await dest.FlushAsync(cancellationToken);
        }
    }

    public class BytesDestination : IOutputDestination
    {
        private MemoryStream _m;
        public void Dispose()
        {
        }

        public Task RequestCapacityAsync(int bytes)
        {
            if (_m == null) _m = new MemoryStream(bytes);
            
            if (_m.Capacity < bytes) _m.Capacity = bytes;
            return Task.CompletedTask;
        }

        public Task WriteAsync(ArraySegment<byte> bytes, CancellationToken cancellationToken)
        {
            return _m.WriteAsync(bytes.Array, bytes.Offset, bytes.Count, cancellationToken);
        }

        public Task FlushAsync(CancellationToken cancellationToken)
        {
            return _m.FlushAsync(cancellationToken);
        }
        
        public ArraySegment<byte> GetBytes()
        {
            if (!_m.TryGetBuffer(out var bytes))
            {
                throw new ImageflowAssertionFailed("MemoryStream TryGetBuffer should not fail here");
            }
            return bytes;
        }
    }

    public class StreamDestination : IOutputDestination
    {
        private readonly Stream _underlying;
        private readonly bool _disposeUnderlying;
        public StreamDestination(Stream underlying, bool disposeUnderlying)
        {
            _underlying = underlying;
            _disposeUnderlying = disposeUnderlying;
        }

        public void Dispose()
        {
            if (_disposeUnderlying) _underlying?.Dispose();
        }

        public Task RequestCapacityAsync(int bytes)
        {
            if (_underlying.CanSeek && _underlying.CanWrite) _underlying.SetLength(bytes);
            return Task.CompletedTask;
        }

        public Task WriteAsync(ArraySegment<byte> bytes, CancellationToken cancellationToken)
        {
            return _underlying.WriteAsync(bytes.Array, bytes.Offset, bytes.Count, cancellationToken);
        }

        public Task FlushAsync(CancellationToken cancellationToken)
        {
            if (_underlying.CanSeek && _underlying.CanWrite && _underlying.Position < _underlying.Length) _underlying.SetLength(_underlying.Position);
            return _underlying.FlushAsync(cancellationToken);
        }
    }
}