using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DefendTheWave.Common
{
    /// <summary>
    /// Took from UniRX: https://github.com/mono/rx/blob/master/Rx/NET/Source/System.Reactive.Core/Reactive/Disposables/CompositeDisposable.cs
    /// </summary>
	public sealed class CompositeDisposable : ICollection<IDisposable>
    {
        public int Count { get; private set; }

        public bool IsReadOnly => false;
        
        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        private readonly object _gate = new object();

        private List<IDisposable> _disposables;
        private const int ShrinkThreshold = 64;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.CompositeDisposable"/> class with no disposables contained by it initially.
        /// </summary>
        public CompositeDisposable()
        {
            _disposables = new List<IDisposable>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.CompositeDisposable"/> class with the specified number of disposables.
        /// </summary>
        /// <param name="capacity">The number of disposables that the new CompositeDisposable can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        public CompositeDisposable(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

            _disposables = new List<IDisposable>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is null.</exception>
        public CompositeDisposable(params IDisposable[] disposables)
        {
            if (disposables == null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            _disposables = new List<IDisposable>(disposables);
            Count = _disposables.Count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is null.</exception>
        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            if (disposables == null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            _disposables = new List<IDisposable>(disposables);
            
            Count = _disposables.Count;
        }

        /// <summary>
        /// Adds a disposable to the CompositeDisposable or disposes the disposable if the CompositeDisposable is disposed.
        /// </summary>
        /// <param name="item">Disposable to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public void Add(IDisposable item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var shouldDispose = false;
            lock (_gate)
            {
                shouldDispose = IsDisposed;
                
                if (!IsDisposed)
                {
                    _disposables.Add(item);
                    Count++;
                }
            }
            if (shouldDispose)
            {
                item.Dispose();
            }
        }

        /// <summary>
        /// Removes and disposes the first occurrence of a disposable from the CompositeDisposable.
        /// </summary>
        /// <param name="item">Disposable to remove.</param>
        /// <returns>true if found; false otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public bool Remove(IDisposable item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var shouldDispose = false;

            lock (_gate)
            {
                if (!IsDisposed)
                {
                    //
                    // List<T> doesn't shrink the size of the underlying array but does collapse the array
                    // by copying the tail one position to the left of the removal index. We don't need
                    // index-based lookup but only ordering for sequential disposal. So, instead of spending
                    // cycles on the Array.Copy imposed by Remove, we use a null sentinel value. We also
                    // do manual Swiss cheese detection to shrink the list if there's a lot of holes in it.
                    //
                    var index = _disposables.IndexOf(item);
                    
                    if (index >= 0)
                    {
                        shouldDispose = true;
                        _disposables[index] = null;
                        Count--;

                        if (_disposables.Capacity > ShrinkThreshold && Count < _disposables.Capacity / 2)
                        {
                            var old = _disposables;
                            _disposables = new List<IDisposable>(_disposables.Capacity / 2);

                            foreach (var disposable in old)
                            {
                                if (disposable != null)
                                {
                                    _disposables.Add(disposable);
                                }
                            }
                        }
                    }
                }
            }

            if (shouldDispose)
            {
                item.Dispose();
            }

            return shouldDispose;
        }

        /// <summary>
        /// Disposes all disposables in the group and removes them from the group.
        /// </summary>
        public void Dispose()
        {
            var currentDisposables = default(IDisposable[]);
            lock (_gate)
            {
                if (!IsDisposed)
                {
                    IsDisposed = true;
                    currentDisposables = _disposables.ToArray();
                    _disposables.Clear();
                    Count = 0;
                }
            }

            if (currentDisposables != null)
            {
                foreach (var disposable in currentDisposables)
                {
                    disposable?.Dispose();
                }
            }
        }

        /// <summary>
        /// Removes and disposes all disposables from the CompositeDisposable, but does not dispose the CompositeDisposable.
        /// </summary>
        public void Clear()
        {
            IDisposable[] currentDisposables;
            
            lock (_gate)
            {
                currentDisposables = _disposables.ToArray();
                _disposables.Clear();
                Count = 0;
            }

            foreach (var disposable in currentDisposables)
            {
                disposable?.Dispose();
            }
        }

        /// <summary>
        /// Determines whether the CompositeDisposable contains a specific disposable.
        /// </summary>
        /// <param name="item">Disposable to search for.</param>
        /// <returns>true if the disposable was found; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public bool Contains(IDisposable item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            lock (_gate)
            {
                return _disposables.Contains(item);
            }
        }

        /// <summary>
        /// Copies the disposables contained in the CompositeDisposable to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">Array to copy the contained disposables to.</param>
        /// <param name="arrayIndex">Target index at which to copy the first disposable of the group.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero. -or - <paramref name="arrayIndex"/> is larger than or equal to the array length.</exception>
        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            
            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            lock (_gate)
            {
                Array.Copy(_disposables.Where(d => d != null).ToArray(), 
                    0, array, arrayIndex, array.Length - arrayIndex);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the CompositeDisposable.
        /// </summary>
        /// <returns>An enumerator to iterate over the disposables.</returns>
        public IEnumerator<IDisposable> GetEnumerator()
        {
            IEnumerable<IDisposable> result;

            lock (_gate)
            {
                result = _disposables.Where(d => d != null).ToList();
            }

            return result.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the CompositeDisposable.
        /// </summary>
        /// <returns>An enumerator to iterate over the disposables.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
