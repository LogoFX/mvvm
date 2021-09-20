using System;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Child Operation Event Arguments
    /// </summary>
    /// <typeparam name="T"> type of child</typeparam>
    public class ChildEventArgs<T> : EventArgs
    {
        private readonly ChildOperation _operation;
        private readonly T _item;
        private readonly int _index;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="item"></param>
        /// <param name="index"></param>
        public ChildEventArgs(ChildOperation operation, T item, int index)
        {
            _operation = operation;
            _item = item;
            _index = index;
        }
        /// <summary>
        /// Added item
        /// </summary>
        public T Item { get { return _item; } }
        /// <summary>
        /// Changed index
        /// </summary>
        public int Index { get { return _index; } }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The action.</value>
        public ChildOperation Action
        {
            get { return _operation; }
        }
    }
}
